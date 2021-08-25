using System;
using System.Collections.Generic;

namespace speedApp1
{
    class ProgramPart3
    {
        private const string FilePath = @"d:\work\speed-coding-2021\input\test.txt"; 
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadLines(FilePath);
            var allText = System.IO.File.ReadAllText(FilePath);

            
            Console.WriteLine("qq");
        }
    }
}