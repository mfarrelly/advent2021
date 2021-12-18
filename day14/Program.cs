// See https://aka.ms/new-console-template for more information

using System.Text;

var fileInfo = new FileInfo(args[0]);

if (fileInfo.Exists)
{
    var allLines = File
        .ReadAllLines(fileInfo.FullName)
        .ToArray();

   
    // Part1(allLines, iterations: 10); 
    // Part1(allLines, iterations: 40, logMessages: false);

    Part1_2(allLines, iterations: 10);
    
    Part1_2(allLines, iterations: 40);
}

void Part1_2(string[] lines, int iterations, bool logMessages = true)
{
    var polymerTemplate = lines[0];

    var pairInsertions = new List<(string pattern, string add)>();
    for (var i = 2; i < lines.Length; i++)
    {
        var parts = lines[i].Split(" -> ", StringSplitOptions.TrimEntries);
        pairInsertions.Add((parts[0], parts[1]));

        if (logMessages)
        {
            Console.WriteLine($"rules: {parts[0]}, {parts[1]}");
        }
    }

    var result = polymerTemplate;
    for (var i = 0; i < iterations; i++)
    {
        Console.WriteLine($"{i}: {DateTimeOffset.Now.ToString("O")}");
        result = ReplaceWithRules(result, pairInsertions);
        // Console.WriteLine($"{i}: {result}");
    }
    // NCNBCHB
    // NCNBCB
    // NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB

    Console.WriteLine(result);
    PrintOutput(result);
}

void Part1(string[] lines, int iterations, bool logMessages = true)
{
    var polymerTemplate = lines[0];

    var pairInsertions = new List<(string pattern, string add)>();
    for (var i = 2; i < lines.Length; i++)
    {
        var parts = lines[i].Split(" -> ", StringSplitOptions.TrimEntries);
        pairInsertions.Add((parts[0], parts[1]));

        if (logMessages)
        {
            Console.WriteLine($"rules: {parts[0]}, {parts[1]}");
        }
    }
    
    Console.WriteLine($"0: {polymerTemplate}");
    
    Console.WriteLine(string.Join(",", SplitBy(polymerTemplate)));

    var sb1 = polymerTemplate;
    var result = Enumerable
        .Range(start: 1, iterations)
        .Aggregate(sb1, (current, x) =>
        {
            // var sb = new StringBuilder();
            for (var i = 0; i + 2 - 1 < current.Length; i++)
            {
                var chunk = $"{current[i]}{current[i+1]}";
                var (pattern, add) = pairInsertions
                    .Where(p => p.pattern.Equals(chunk, StringComparison.OrdinalIgnoreCase))
                    .SingleOrDefault(defaultValue: (pattern: "--", add: "--"));

                if (!pattern.Equals("--", StringComparison.OrdinalIgnoreCase))
                {
                    current = current.Insert(i + 1, add);
                    i++;
                }
                // .Append(!pattern.Equals("--", StringComparison.OrdinalIgnoreCase)
                    // ? $"{chunk[index: 0]}{add}"
                    // : chunk);
            }

            // .Append(current[^1]);
            // var temp = string
            //     .Join(string.Empty,
            //         SplitBy(current)
            //             .Select(c =>
            //             {
            //                 var (pattern, add) = pairInsertions
            //                     .Where(p => p.pattern.Equals(c, StringComparison.OrdinalIgnoreCase))
            //                     .SingleOrDefault(defaultValue: (pattern: "--", add: "--"));
            //
            //                 return !pattern.Equals("--", StringComparison.OrdinalIgnoreCase)
            //                     ? $"{c[index: 0]}{add}"
            //                     : c;
            //             }));
            if (logMessages)
            {
                Console.WriteLine($"{x}: {sb1.ToString()}");
            }
            else
            {
                Console.WriteLine($"{x}: {DateTimeOffset.Now.ToString("O")}");
            }

            return current;
        });

    PrintOutput(result);
}

void PrintOutput(string result)
{
    // find most and least common characters.
    var mostCommon = result
        .ToString()
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

string ReplaceWithRules(string input, List<(string pattern, string add)> rules)
{
    var result = input;
    for (var i = 0; i + 1 < result.Length; i++)
    {
        var item = result.Substring(i, length: 2);
        // Console.WriteLine($"> {item}");
        foreach (var (pattern, add) in rules)
        {
            if (item.Equals(pattern, StringComparison.OrdinalIgnoreCase))
            {
                result = result.Insert(i + 1, add);
                // Console.WriteLine($"  > {add}/{pattern}/{i}");
                // i+=2;
                i++;
                break;
            }
        }
    }

    return result;
}