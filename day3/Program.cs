// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


string GetGamma(Dictionary<int, (int countOf1, int countOf0)> values)
{
    var result = "";
    foreach (var d in values)
    {
        result += d.Value.countOf0 > d.Value.countOf1 ? "0" : "1";
    }

    return result;
}

string GetEpsilon(Dictionary<int, (int countOf1, int countOf0)> values)
{
    var result = "";
    foreach (var d in values)
    {
        result += d.Value.countOf0 > d.Value.countOf1 ? "1" : "0";
    }

    return result;
}


// Run the program.
var fileInfo = new FileInfo(args[0]);

void Part1(List<string>? list)
{
    var counts = new Dictionary<int, (int countOf1, int countOf0)>();
    foreach (var line in list)
    {
        for (var digit = 0; digit < line.Length; digit++)
        {
            var value = line[digit];
            if (!counts.ContainsKey(digit))
            {
                counts.Add(digit, (0, 0));
            }
            else
            {
                var iValue = int.Parse(value.ToString());
                var dValue = counts[digit];
                if (iValue == 0)
                {
                    dValue.countOf0++;
                }
                else
                {
                    dValue.countOf1++;
                }

                counts[digit] = dValue;
            }
        }
    }

    Console.WriteLine("Finished");
    foreach (var d in counts)
    {
        Console.WriteLine($"d {d.Key} = {d.Value.countOf0}x{d.Value.countOf1}");
    }

    var g = GetGamma(counts);
    var e = GetEpsilon(counts);
    var gAsInt = Convert.ToInt32(g, fromBase: 2);
    var eAsInt = Convert.ToInt32(e, fromBase: 2);
    Console.WriteLine($"g:{g}, {gAsInt}, e:{e}, {eAsInt} == {gAsInt * eAsInt}");
}

(int countOf0, int countOf1) GetCounts(string[] strings, int i)
{
    var valueTuple = (countOf0: 0, countOf1: 0);

    foreach (var d in strings)
    {
        if (d[i].Equals(obj: '0'))
        {
            valueTuple.countOf0++;
        }
        else
        {
            valueTuple.countOf1++;
        }
    }

    return valueTuple;
}

string GetO2Internal(string[] values, int digit)
{
    if (values.Length == 1)
    {
        return values.First(); 
    }

    var (countOf0, countOf1) = GetCounts(values, digit);

    var topDigit = countOf1 >= countOf0
        // in the event of a tie, choose 1.
        ? '1' 
        : '0';
    // choose all items with that
    var finalValues = values
        .Where(v => v[digit] == topDigit)
        .ToArray();
    return GetO2Internal(finalValues, digit + 1);
}


string GetCO2Internal(string[] values, int digit)
{
    // base case:
    if (values.Length == 1)
    {
        return values.First(); 
    }

    // otherwise count the digits.
    var (countOf0, countOf1) = GetCounts(values, digit);

    var leastDigit = countOf0 <= countOf1
        // in the event of a tie, choose 0.
        ? '0' 
        : '1';
    // choose all items with that
    var finalValues = values
        .Where(v => v[digit] == leastDigit)
        .ToArray();
    // Console.WriteLine($"CO2: tv:{leastDigit} [{string.Join(",",finalValues.Select(v => v))}]");
    return GetCO2Internal(finalValues, digit + 1);
}

string GetO2(List<string> values)
{
    return GetO2Internal(values.ToArray(), digit: 0);
}


string GetCO2(List<string> values)
{
    return GetCO2Internal(values.ToArray(), digit: 0);
}

void Part2(List<string> list)
{
    var o2value = GetO2(list);
    var o2int = Convert.ToInt32(o2value, fromBase: 2);
    var co2value = GetCO2(list);
    var co2int = Convert.ToInt32(co2value, fromBase: 2);
    
    Console.WriteLine($"o2:{o2value}, {o2int}");
    
    Console.WriteLine($"co2:{co2value}, {co2int}");
    Console.WriteLine($"total: {o2int * co2int}");
}

if (fileInfo.Exists)
{
    var allLines = File.ReadAllLines(fileInfo.FullName);
    var allValues = allLines.Select(line =>
    {
        return line;
    }).ToList();

    
    Part1(allValues);
    Part2(allValues);
}