// See https://aka.ms/new-console-template for more information

var fileInfo = new FileInfo(args[0]);

if (fileInfo.Exists)
{
    var allLines = File
        .ReadAllLines(fileInfo.FullName)
        .ToArray();

   
    Part1(allLines, iterations: 10);
    //Part2(allLines);
}

void Part1(string[] lines, int iterations)
{
    var polymerTemplate = lines[0];

    var pairInsertions = new List<(string pattern, string add)>();
    for (var i = 2; i < lines.Length; i++)
    {
        var parts = lines[i].Split(" -> ", StringSplitOptions.TrimEntries);
        pairInsertions.Add((parts[0], parts[1]));
        
        Console.WriteLine($"rules: {parts[0]}, {parts[1]}");
    }
    
    Console.WriteLine($"0: {polymerTemplate}");
    
    Console.WriteLine(string.Join(",", SplitBy(polymerTemplate)));

    var result = Enumerable
        .Range(start: 1, iterations)
        .Aggregate(polymerTemplate, (current, i) =>
        {
            var temp = string
                .Join(string.Empty,
                    SplitBy(current)
                        .Select(c =>
                        {
                            var (pattern, add) = pairInsertions
                                .Where(p => p.pattern.Equals(c, StringComparison.OrdinalIgnoreCase))
                                .SingleOrDefault(defaultValue: (pattern: "--", add: "--"));

                            return !pattern.Equals("--", StringComparison.OrdinalIgnoreCase)
                                ? $"{c[index: 0]}{add}"
                                : c;
                        }));
            Console.WriteLine($"{i}: {temp}");
            return temp;
        });
    // NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB
    // NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB
    
    // find most and least common characters.
    var mostCommon = result
        .GroupBy(c => c)
        .OrderByDescending(c => c.Count())
        .ToArray();

    var most = mostCommon[0];
    var least = mostCommon[^1];
    
    Console.WriteLine($"Result = {result}, M {most.Key}, L {least.Key}. {most.Count() - least.Count()}");
}

string[] SplitBy(string input, int chunkSize = 2)
{
    var chunks = new List<string>();
    for (var i = 0; i + chunkSize - 1 < input.Length; i++)
    {
        chunks.Add(input.Substring(i, chunkSize));
    }
    chunks.Add(input[^1].ToString());
    return chunks.ToArray();
}