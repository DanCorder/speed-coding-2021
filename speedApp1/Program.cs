using System;

namespace speedApp1
{
    class Program
    {
        private const string FilePath = @"d:\work\speed-coding-2021\input\"; 
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var lines = System.IO.File.ReadLines(FilePath);
        }
    }
}