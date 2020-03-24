using System;
using System.IO;
using System.Collections.Generic;

namespace Lab_4_NaiveBayes
{
    class NaiveBayes
    {
        #region DataPreparation
        private static void CompressAtribute(string[][] dataSet)
        {
            for(int i=0; i<dataSet.Length; i++)
            {
                if(double.Parse(dataSet[i][1]) < 16)
                {
                    dataSet[i][1] = "chlodno";
                }
                else if(double.Parse(dataSet[i][1]) > 20)
                {
                    dataSet[i][1] = "goraco";
                }
                else
                {
                    dataSet[i][1] = "cieplo";
                }
            }
        }
        public static string[][] LoadData(string path)
        {
            string[] rows = File.ReadAllLines(path);
            string[][] dataSet = new string[rows.Length][];
            for(int i=0; i<rows.Length; i++)
            {
                dataSet[i] = rows[i].Split(",");
            }

            CompressAtribute(dataSet);
            return dataSet;
        }
        
        #endregion
   
        #region BayesTheorem
        private static double ZeroExceptionHandler(double clsLength, string[][] dataSet, int column)
        {
            HashSet<string> set = new HashSet<string>();
            for(int i=0; i<dataSet.Length; i++)
            {
                set.Add(dataSet[i][column]);
            }
            return 1/(clsLength + set.Count);
        }

        private static double CalculateClassPropability(string[][] cls, string[][] dataSet, string[] test)
        {
            // Class propability P(C=C_i)
            double elementsCount = dataSet.Length;
            double pClass = cls.Length/elementsCount; 
            // Features propabilities
            double pWeather = 0, pTemp=0, pWind=0;

            for(int i=0; i<cls.Length; i++)
            {
                if(cls[i][0] == test[0]) pWeather++;
                if(cls[i][1] == test[1]) pTemp++;
                if(cls[i][2] == test[2]) pWind++;
            }

            pWeather /= cls.Length;
            pTemp /= cls.Length;
            pWind /= cls.Length;

            if(pWeather == 0) pWeather = ZeroExceptionHandler(cls.Length, dataSet, 0); // ... ... col_number
            if(pTemp == 0) pTemp = ZeroExceptionHandler(cls.Length, dataSet, 1);
            if(pWind == 0) pWind = ZeroExceptionHandler(cls.Length, dataSet, 2);
            
            return pClass*pWeather*pTemp*pWind;
        }

        #endregion

        private static string SeparateByClass(string[][] dataSet, string[] test)
        {  
            // Count classes
            int y=0, n=0;
            for(int i=0; i<dataSet.Length; i++)
            {
                if(dataSet[i][3] == "tak") y++;
                else n++;
            }
            // Separate by class
            string[][] yes = new string[y][];
            string[][] no = new string[n][];
            y=n=0;

            for(int i=0; i<dataSet.Length; i++)
            {
                if(dataSet[i][3] == "tak")
                {
                    yes[y++] = new string[] {
                        dataSet[i][0],
                        dataSet[i][1], 
                        dataSet[i][2]
                        };
                }
                else
                {
                    no[n++] = new string[] {
                        dataSet[i][0],
                        dataSet[i][1], 
                        dataSet[i][2]
                        };
                }
            }

            double pFirstClass = CalculateClassPropability(yes, dataSet, test);
            double pSecondClass = CalculateClassPropability(no, dataSet, test);

            if(pFirstClass > pSecondClass) return "tak";
            else if(pFirstClass < pSecondClass) return "nie";
            else return "*wybor dowolny*";

        }
        public static void Predict(string[][] dataSet, string[] test)
        {
            string number = test[1];
            if(double.Parse(number) < 16)
            {
                test[1] = "chlodno";
            }
            else if(double.Parse(number) > 20)
            {
                test[1] = "goraco";
            }
            else
            {
                test[1] = "cieplo";
            }

            string result = SeparateByClass(dataSet, test); 
            System.Console.WriteLine(
                $"Obiekt Test[{test[0]}, {number}, {test[2]}] powienien być w klasie decyzji o spacerze o nazwie: {result}"
                );
        }
    }
}
