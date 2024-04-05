using System;
using System.Collections.Generic;

namespace func_rocket;

public static class LevelsTask
{
    private static readonly Physics standardPhysics = new();

    private static readonly Vector standartTargetPos = new(600, 200);

    private static Rocket standartRocket => new(new Vector(200, 500), Vector.Zero, -Math.PI / 2);

    private static readonly Gravity whiteHole = (size, v) =>
    {
        var d = v - standartTargetPos;
        return 140 * d / ((d.Length * d.Length) + 1);
    };

    private static readonly Gravity blackHole = (size, v) =>
    {
        var anomaly = (standartTargetPos - standartRocket.Location) / 2 + standartRocket.Location;
        var d = anomaly - v;
        return 300 * d / (d.Length * d.Length + 1);
    };

    private static readonly Gravity blackAndWhiteHoles = (size, v) =>
    {
        return (whiteHole(size, v) + blackHole(size, v)) / 2;
    };

    private static Level CreateLevel(string name, Vector targetPos, Gravity gravity)
    {
        return new Level(name, standartRocket, targetPos, gravity, standardPhysics);
    }

    public static IEnumerable<Level> CreateLevels()
    {
        yield return CreateLevel("Zero", standartTargetPos, (size, v) => Vector.Zero);
        yield return CreateLevel("Heavy", standartTargetPos, (size, v) => new Vector(0, .9));
        yield return CreateLevel("Up", new Vector(700, 500), (size, v) => new Vector(0, -300.0 / (size.Y - v.Y + 300)));
        yield return CreateLevel("WhiteHole", standartTargetPos, whiteHole);
        yield return CreateLevel("BlackHole", standartTargetPos, blackHole);
        yield return CreateLevel("BlackAndWhite", standartTargetPos, blackAndWhiteHoles);
    }
}