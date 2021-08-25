using System;
using System.Collections.Generic;
using System.Linq;

namespace speedApp1
{
    class Part3
    {
        private const string WordsPath = @"d:\work\speed-coding-2021\input\part3\code-number-words.txt";
        private const string DictionaryPath = @"d:\work\speed-coding-2021\input\part3\dictionary.txt";

        public static void Run()
        {
            var codedWords = System.IO.File.ReadLines(WordsPath).Select(w => w.Split(' ').Select(n => Int32.Parse(n)).ToList()).ToList().OrderByDescending(w => w.Count).ToList();
            var dictionaryByLength = System.IO.File.ReadLines(DictionaryPath).GroupBy(w => w.Length).ToDictionary(w => w.Key, w => w.ToList());

            var mapping = new Dictionary<int, char>
            {
                {4, 'd'},
                {10, 'g'}
            };

            var count = mapping.Count;

            while (mapping.Count < 23)
            {
                var possibleMatchesByWord =
                    codedWords.ToDictionary(cw => cw, cw => FindMatches(cw, dictionaryByLength[cw.Count], mapping));

                var wordsWithSingleOptions = possibleMatchesByWord.Where(kvp => kvp.Value.Count == 1).ToList();
                if (wordsWithSingleOptions.Count > 0)
                {
                    mapping = wordsWithSingleOptions.First().Value.Single().Value;
                    codedWords.Remove(wordsWithSingleOptions.First().Key);
                }
            }

            Console.WriteLine(String.Join('\n', mapping.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key}={kvp.Value}")));
        }

        private static Dictionary<string, Dictionary<int, char>> FindMatches(List<int> codedWord, List<string> dictionaryPossibilities, Dictionary<int, char> mapping)
        {
            var ret = new Dictionary<string, Dictionary<int, char>>();
            foreach (var option in dictionaryPossibilities)
            {
                var newMapping = mapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                var isMatch = true;
                for (int i = 0; i < codedWord.Count; i++)
                {
                    if (newMapping.ContainsKey(codedWord[i]))
                    {
                        var ch = newMapping[codedWord[i]];
                        if (option[i] != ch)
                        {
                            isMatch = false;
                            break;
                        }
                    }
                    else
                    {
                        newMapping[codedWord[i]] = option[i];
                    }
                }

                if (isMatch)
                {
                    ret[option] = newMapping;
                }
            }

            return ret;
        }
    }
}