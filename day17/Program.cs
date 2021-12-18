// See https://aka.ms/new-console-template for more information


using System.Text.RegularExpressions;


var part1Input = "target area: x=201..230, y=-99..-65";
var part1TestInput = "target area: x=20..30, y=-10..-5";

// The probe's x,y position starts at 0,0. Then, it will follow some trajectory by moving in steps. On each step, these changes occur in the following order:
//
//     The probe's x position increases by its x velocity.
//     The probe's y position increases by its y velocity.
//     Due to drag, the probe's x velocity changes by 1 toward the value 0; that is, it decreases by 1 if it is greater than 0, increases by 1 if it is less than 0, or does not change if it is already 0.
//     Due to gravity, the probe's y velocity decreases by 1.

Part1(part1Input);

HitResult ShootYaShot(Velocity start_d, Area target)
{
    var pos = (x: 0, y: 0, xd: start_d.X, yd: start_d.Y);

    var maxY = int.MinValue;
    for (var i = 0; i < 500; i++)
    {
        pos.x += pos.xd;
        pos.y += pos.yd;
        pos.xd -= pos.xd > 0 ? 1 : 0;
        pos.yd -= 1;
        maxY = pos.y > maxY ? pos.y : maxY;

        // Console.Write($"{i}: {pos.x},{pos.y}");

        if (pos.x >= target.X
            && pos.x <= target.X1
            && pos.y >= target.Y
            && pos.y <= target.Y1)
        {
            // Console.WriteLine("  HIT");
            // hits.Add((start_d.X, start_d.X, maxY));
            return new HitResult(maxY, IsHit: true);
        }
        // break;
        // record height of y.
        // }
        // else
        // {
            // Console.WriteLine("");
        // }
    }

    return new HitResult(maxY, IsHit: false);


}
void Part1(string input)
{
    var inputParts = input.Split(" ", StringSplitOptions.TrimEntries);
    var xPartMatches = Regex.Match(input, @"x=(\-?[0-9]*)\.\.(\-?[0-9]*),");
    var yPartMatches = Regex.Match(input, @"y=(\-?[0-9]*)\.\.(\-?[0-9]*)");
    

    var target_x = (start: int.Parse(xPartMatches.Groups[groupnum: 1].Value), end: int.Parse(xPartMatches.Groups[groupnum: 2].Value));
    var target_y = (start: int.Parse(yPartMatches.Groups[groupnum: 1].Value), end: int.Parse(yPartMatches.Groups[groupnum: 2].Value));
    var target = new Area(target_x.start, target_x.end, target_y.start, target_y.end);

    var d = (x: 0, y: 0);


    var xs = Enumerable.Range(start: -500, count: 1000);
    var ys = Enumerable.Range(start: -500, count: 1000);

    var hits = (
            from x in xs
            from y in ys
            select new Velocity(x, y) into nextVelocity 
            let results = ShootYaShot(nextVelocity, target) 
            where results.IsHit 
            select new LogResult(nextVelocity, results.MaxHeight)
        )
        .ToList();

    // What is the highest y position it reaches on this trajectory?
    Console.WriteLine("HITS");
    var (initialVelocity, maxHeight) = hits.OrderByDescending(x => x.MaxHeight).First();
    var countOfHits = hits.Count;
    Console.WriteLine($"{maxHeight}: {initialVelocity.X}x{initialVelocity.Y}");
    Console.WriteLine(countOfHits);
}

record Position(int x, int y);
record Area(int X, int X1, int Y, int Y1);
record Velocity(int X, int Y);
record HitResult(int MaxHeight, bool IsHit);

record LogResult(Velocity Velocity, int MaxHeight);