// See https://aka.ms/new-console-template for more information

// Run the program.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var fileInfo = new FileInfo(args[0]);

int[][] Transform(int[][] board) =>
    Enumerable
        .Range(start: 0, count: 5)
        .Select(column => board.Select(line => line[column]).ToArray())
        .ToArray();

bool IsSolved(int[] calledNumbers, int[][] board)
{
    // test rows
    if (board.Any(row => calledNumbers.All(row.Contains)))
    {
        return true;
    }

    var partitionedData = Transform(board);
    
    if (partitionedData.Any(columnRow => calledNumbers.All(columnRow.Contains)))
    {
        return true;
    }

    return false;

    // test columns
}

Dictionary<int, int[][]> GetBoards(List<string> allLines)
{
    var result = new Dictionary<int, int[][]>();
    var id = 0;
    for (var i = 1; i < allLines.Count; i += 6)
    {
        // First line will be blank
        var boardLines = allLines.GetRange(i + 1, count: 6);
        var board = boardLines.Select(l => l.Split(",", StringSplitOptions.TrimEntries).Select(int.Parse).ToArray()).ToArray();
        result.Add(id++, board);
    }

    return result;
}

void Part1(List<string> allLines)
{
    var callNumbers = allLines[0].Split(",").Select(int.Parse).ToList();
    var boards = GetBoards(allLines);

    var result = (nums: Array.Empty<int>(), board: Array.Empty<int[]>());
    
    var wonBoards = callNumbers
        .Aggregate(
            Array.Empty<int>(), 
            (acc, next) =>
            {
                var currentNumbers = acc.Append(next).ToArray();
                foreach (var board in boards)
                {
                    var solved = IsSolved(currentNumbers, board.Value);
                    if (solved)
                    {
                        result = (currentNumbers, board.Value);
                        break;
                    }
                }

                return currentNumbers;
            }, 
            acc => acc);
    
    Console.WriteLine($"Solved with {string.Join(",", result.nums)}");
    Console.WriteLine($"Board with {string.Join(",", result.board.Select(r => string.Join("|", r)))}");
}

if (fileInfo.Exists)
{
    var allLines = File
        .ReadAllLines(fileInfo.FullName)
        .ToList();

   
    Part1(allLines);
    // Part2(allValues);
}