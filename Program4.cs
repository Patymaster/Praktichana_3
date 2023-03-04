using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введіть шлях до директорії з текстовими файлами: ");
            string directoryPath = Console.ReadLine();

            Console.Write("Введіть шлях до файлу зі словами: ");
            string wordFilePath = Console.ReadLine();

            var fileNames = Directory.GetFiles(directoryPath);

            var words = File.ReadAllText(wordFilePath)
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.ToLower());

            Func<string, IEnumerable<string>> tokenize = file => File.ReadLines(file)
                .SelectMany(line => line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));

            Func<IEnumerable<string>, IDictionary<string, int>> calculateWordFrequencies = Words => Words
                .GroupBy(word => word.ToLower())
                .ToDictionary(group => group.Key, group => group.Count());

            Action<IDictionary<string, int>> displayReport = wordFrequencies =>
            {
                Console.WriteLine("Word Frequencies:");
                foreach (var pair in wordFrequencies.OrderByDescending(pair => pair.Value))
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
            };

            var wordFrequenciesAcrossFiles = fileNames
                .Select(file => tokenize(file))
                .Select(Words => Words.Where(word => Words.Contains(word)))
                .Select(Words => calculateWordFrequencies(Words))
                .Aggregate((left, right) => left.Concat(right)
                    .GroupBy(pair => pair.Key)
                    .ToDictionary(group => group.Key, group => group.Sum(pair => pair.Value)));

            displayReport(wordFrequenciesAcrossFiles);

            Console.ReadKey();
        }
    }
}