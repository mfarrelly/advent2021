// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Gets the depth (PART1)
int GetDepthPart1(List<(CommandName command, int value)> valueTuples)
{
    if (valueTuples is null) throw new ArgumentNullException(nameof(valueTuples));

    var location = (depth: 0, distance: 0);
    foreach (var (command, value) in valueTuples)
    {
        switch (command)
        {
            case CommandName.Forward:
                location.distance += value;
                break;
            case CommandName.Up:
                location.depth -= value;
                break;
            case CommandName.Down:
                location.depth += value;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    return location.depth * location.distance;
}

// Gets the depth (PART2) - uses aim.
int GetDepthPart2(List<(CommandName command, int value)> valueTuples)
{
    if (valueTuples is null) throw new ArgumentNullException(nameof(valueTuples));

    var location = (depth: 0, distance: 0, aim: 0);
    foreach (var (command, value) in valueTuples)
    {
        switch (command)
        {
            case CommandName.Forward:
                location.distance += value;
                location.depth += location.aim * value;
                break;
            case CommandName.Up:
                location.aim -= value;
                break;
            case CommandName.Down:
                location.aim += value;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    return location.depth * location.distance;
}

// Convert a string command name to enum CommandName
CommandName AsCommandName(string value) =>
    value.ToUpperInvariant() switch
    {
        "FORWARD" => CommandName.Forward,
        "UP" => CommandName.Up,
        "DOWN" => CommandName.Down,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, message: null)
    };

// Run the program.
var fileInfo = new FileInfo(args[0]);

if (fileInfo.Exists)
{
    var allLines = File.ReadAllLines(fileInfo.FullName);
    var allValues = allLines.Select(line =>
    {
        var parts = line.Split(" ");
        return (command: AsCommandName(parts[0]), value: int.Parse(parts[1]));
    }).ToList();

    var depth1 = GetDepthPart1(allValues);
    Console.WriteLine($"Part 1 Location: {depth1}");

    var depth2 = GetDepthPart2(allValues);
    Console.WriteLine($"Part 2 Location: {depth2}");
}

internal enum CommandName
{
    Unknown = 0,
    Forward = 1,
    Up = 2,
    Down = 3
}