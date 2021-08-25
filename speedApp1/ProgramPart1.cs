using System;
using System.Collections.Generic;

namespace speedApp1
{
    class ProgramPart1
    {
        private const string FilePath = @"d:\work\speed-coding-2021\input\test.txt"; 
        static void MainPart1(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var lines = System.IO.File.ReadLines(FilePath);
            var allText = System.IO.File.ReadAllText(FilePath);

            var dice = new List<int>
            {
                4,4,4,4,4,4,4,4,4,4,
                6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,
                8,8,8,8,8,8,8,8,8,8,
                10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,
                12,12,12,12,12,12,12,12,12,12,12,
                20,20,20,20,20,20,20,20,20,20,20,20,
                30
            };

            var probabilities = new Dictionary<int, decimal> { { 0, 1 } };
            var newProbabilities = new Dictionary<int, decimal>();

            foreach (var die in dice)
            {
                foreach (var total in probabilities)
                {
                    for (var i = 1; i <= die; i++)
                    {
                        var newTotal = total.Key + i;
                        var newProbability = total.Value * (1m / ((decimal)die));

                        if (newProbabilities.ContainsKey(newTotal))
                        {
                            var storedProbability = newProbabilities[newTotal];
                            newProbabilities[newTotal] = (newProbability + storedProbability);
                        }
                        else
                        {
                            newProbabilities[newTotal] = newProbability;
                        }
                    }
                }

                probabilities = newProbabilities;
                newProbabilities = new Dictionary<int, decimal>();
            }
            
            Console.WriteLine(probabilities[500]);
        }
    }
}