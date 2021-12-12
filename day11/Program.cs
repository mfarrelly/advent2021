// See https://aka.ms/new-console-template for more information

var fileInfo = new FileInfo(args[0]);

if (fileInfo.Exists)
{
    var allLines = File
        .ReadAllLines(fileInfo.FullName)
        .ToArray();

   
    Part1(allLines);
    Part2(allLines);
}


int[][] ToMatrix(string[] lines) =>
    lines
        .Select(line => line
            .Select(c => int.Parse(c.ToString()))
            .ToArray())
        .ToArray();

int[][] ShallowCopy(int[][] board) =>
    board
        .Select(line => line
            .ToArray())
        .ToArray();


int UpdateValue(int[][] board, int rowIndex, int colIndex)
{

    if (rowIndex < 0 
        || rowIndex >= board.Length
        || colIndex >= board[0].Length
        || colIndex < 0)
    {
        return 0;
    }
    if (board[rowIndex][colIndex] == 0)
    {
        return 0;
    }

    board[rowIndex][colIndex]++;

    if (board[rowIndex][colIndex] > 9)
    {
        return Flash(board, rowIndex, colIndex);
    }

    return 0;
}

int Flash(int[][] board, int rowIndex, int colIndex)
{
    // rowIndex/colIndex set to 0.
    board[rowIndex][colIndex] = 0;
    var flashes = 1;

    flashes += UpdateValue(board, rowIndex - 1, colIndex - 1);
    flashes += UpdateValue(board, rowIndex - 1, colIndex);
    flashes += UpdateValue(board, rowIndex - 1, colIndex + 1);
    
    flashes += UpdateValue(board, rowIndex, colIndex - 1);
    flashes += UpdateValue(board, rowIndex, colIndex + 1);
    
    flashes += UpdateValue(board, rowIndex + 1, colIndex - 1);
    flashes += UpdateValue(board, rowIndex + 1, colIndex);
    flashes += UpdateValue(board, rowIndex + 1, colIndex + 1);

    return flashes;
}

(int[][] board, int flashes) Boost(int[][] step)
{
    var cloned = ShallowCopy(step);
    var flashes = 0;
    foreach (var t in cloned)
    {
        for (var colIndex = 0; colIndex < t.Length; colIndex++)
        {
            t[colIndex]++;
        }
    }
    // do checks for flash
    for (var rowIndex = 0; rowIndex < cloned.Length; rowIndex++)
    {
        for (var colIndex = 0; colIndex < cloned[rowIndex].Length; colIndex++)
        {
            if (cloned[rowIndex][colIndex] > 9)
            {
                flashes += Flash(cloned, rowIndex, colIndex);
            }
        }
    }

    return (cloned, flashes);
}


void Print(int[][] step)
{
    var cloned = ShallowCopy(step);
    foreach (var row in cloned)
    {
        foreach (var column in row)
        {
            var originalColor = Console.ForegroundColor;
            if (column == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.Write(column);
            if (column == 0)
            {
                Console.ForegroundColor = originalColor;
            }
        }
        Console.WriteLine("");
    }
}

void Part1(string[] lines)
{
    var start = ToMatrix(lines);
    // var startPosition = Console.GetCursorPosition();
    Console.WriteLine($"Step {0}");
    Print(start);
    var flashes = 0;
    for (var step = 1; step <= 100; step++)
    {
        // Console.SetCursorPosition(startPosition.Left, startPosition.Top);
        var (board, i) = Boost(start);
        start = board;
        flashes += i;
        Console.WriteLine($"Step {step} Flashes {i}");
        Print(start);
    }
    
    Console.WriteLine($"Total flashes {flashes}");
}

void Part2(string[] lines)
{
    var start = ToMatrix(lines);
    Console.WriteLine($"Step {0}");
    Print(start);
    var flashes = 0;
    var firstAllFlash = -1;
    var step = 1;
    while (firstAllFlash == -1)
    {
        var (board, i) = Boost(start);
        start = board;
        flashes += i;
        if (i == 100 && firstAllFlash == -1)
        {
            firstAllFlash = step;
        }
        Console.WriteLine($"Step {step} Flashes {i}");
        Print(start);
        step++;
    }
    
    Console.WriteLine($"Total flashes {flashes}");
    Console.WriteLine($"All Flashed at {firstAllFlash}");
}