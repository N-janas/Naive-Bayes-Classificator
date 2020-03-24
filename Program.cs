using System;

namespace Lab_4_NaiveBayes
{
    class Program
    {
        static void Main(string[] args)
        {
            string[][] dataSet = NaiveBayes.LoadData(@"data.txt");
            NaiveBayes.Predict(dataSet, new string[] {"deszczowo", "21", "slaby"});
            NaiveBayes.Predict(dataSet, new string[] {"pochmurno", "14", "mocny"});
        }
    }
}
