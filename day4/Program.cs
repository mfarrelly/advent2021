﻿var fileInfo = new FileInfo(args[0]);

string FlattenBoard(int[][] board) =>
    string.Concat(
        from t in board 
        from j in t 
        select j.ToString()
    );

int[][] Transform(int[][] board) =>
    Enumerable
        .Range(start: 0, count: 5)
        .Select(column => board.Select(line => line[column]).ToArray())
        .ToArray();


void PrintBoard(int[][] board) =>
    Console.WriteLine(
        $"Board with {string.Join(Environment.NewLine, board.Select(r => string.Join(" ", r)))}");

bool IsSolved(int[] calledNumbers, int[][] board)
{
    if (calledNumbers.Length < 5)
    {
        return false;
    }
    // test rows
    if (board.Any(row => row.All(calledNumbers.Contains)))
    {
        return true;
    }

    var partitionedData = Transform(board);
    
    if (partitionedData.Any(row => row.All(calledNumbers.Contains)))
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
        var boardLines = allLines.GetRange(i + 1, count: 5);
        var board = boardLines.Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();
        result.Add(id++, board);
    }

    return result;
}

void Part1(List<string> allLines)
{
    var callNumbers = allLines[0].Split(",").Select(int.Parse).ToList();
    Console.WriteLine($"callNumbers {callNumbers}");
    var boards = GetBoards(allLines);

    var result = new List<ResultBoard>();
    
    callNumbers
        .Aggregate(
            Array.Empty<int>(), 
            (acc, next) =>
            {
                Console.WriteLine($"NEXT {next}");
                var currentNumbers = acc.Append(next).ToArray();
                foreach (var board in boards)
                {
                    var solved = IsSolved(currentNumbers, board.Value);
                    if (solved && !result.Exists(r => FlattenBoard(r.Board).Equals(FlattenBoard(board.Value))))
                    {
                        result.Add(new ResultBoard(currentNumbers, board.Value));
                    }
                }

                return currentNumbers;
            }, 
            acc => acc);

    if (result is null) return;

    var bestBoard = result
        .OrderBy(b => b.Nums.Length)
        .First();
    var worstBoard = result
        .OrderByDescending(b => b.Nums.Length)
        .First();
    
    void WriteLog(ResultBoard board)
    {
        var a = GetUnmarked(board);
        Console.WriteLine($"Solved with {string.Join(",", board.Nums)}");
        Console.WriteLine($"Sum = {a}");
    }

    WriteLog(bestBoard);
    WriteLog(worstBoard);
}

if (fileInfo.Exists)
{
    var allLines = File
        .ReadAllLines(fileInfo.FullName)
        .ToList();

   
    Part1(allLines);
    // Part2(allValues);
}

int GetUnmarked(ResultBoard board)
{
    var unmarked = (
        from t in board.Board 
        from j in t 
        where !board.Nums.Contains(j) 
        select j).ToList();

    return unmarked.Sum(i => i) * board.Nums.Last();
}

record ResultBoard(int[] Nums, int[][] Board);
