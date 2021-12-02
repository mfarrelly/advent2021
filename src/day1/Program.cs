// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            // args[0] should be a valid file.
            var fileInfo = new FileInfo(args[0]);

            if (fileInfo.Exists)
            {
                var allLines = File.ReadAllLines(fileInfo.FullName);
                var allValues = allLines.Select(int.Parse).ToList();

                var data = Program
                    .AnalyzeDataPart1(allValues)
                    .ToArray();

                var countOfIncreasing = data.Count(a => a.Status == RadarState.Increasing);

                // Report
                // foreach (var (value, status) in data)
                // {
                //     Console.WriteLine($"{value} ({status.ToReadableString()})");
                // }

                Console.WriteLine($"There are {countOfIncreasing} increasing events.");
                
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                // Part 2
                var data2 = Program
                    .AnalyzeDataPart2(allValues, slidingWindowSize: 3)
                    .ToArray();

                var countOfIncreasing2 = data2.Count(a => a.Status == RadarState.Increasing);

                // Report
                // foreach (var (value, status) in data2)
                // {
                //     Console.WriteLine($"{value} ({status.ToReadableString()})");
                // }

                Console.WriteLine($"There are {countOfIncreasing2} increasing events.");
            }
            else
            {
                Console.WriteLine($"Couldn't read file {fileInfo.FullName}");
            }

            return 0;
        }


        /// <summary>
        /// Output the state of the radar data.
        /// </summary>
        private static RadarState GetStatus(int? previous, int value) =>
            previous is null
                ? RadarState.Unknown
                : value > previous
                    ? RadarState.Increasing
                    : value < previous
                        ? RadarState.Decreasing
                        : RadarState.Unknown;

        /// <summary>
        /// Analyze the raw data into a set of RadarDatas.
        /// </summary>
        private static IEnumerable<(int Value, RadarState Status)> AnalyzeDataPart1(IEnumerable<int> values)
        {
            var result = new List<(int Value, RadarState Status)>();

            int? previous = null;
            foreach (var value in values)
            {
                var status = Program.GetStatus(previous, value);
                result.Add((value, status));
                previous = value;
            }

            return result;
        }
        
        /// <summary>
        /// Analyze the raw data into a set of RadarDatas.
        /// </summary>
        private static IEnumerable<(int Value, RadarState Status)> AnalyzeDataPart2(List<int> values, int slidingWindowSize)
        {
            var result = new List<(int Value, RadarState Status)>();

            int? previous = null;
            for (var i = 0; i < values.Count; i++)
            {
                var itemsToGet = i + slidingWindowSize > values.Count ? values.Count - i: slidingWindowSize;
                var sums = values.GetRange(i, itemsToGet).Sum();
                var status = Program.GetStatus(previous, sums);
                result.Add((sums, status));
                previous = sums;
            }

            return result;
        }
    }

}
