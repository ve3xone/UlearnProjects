using System;
using System.Collections.Generic;

namespace func_rocket;

public static class LevelsTask
{
    private const double PiDivTwo = (-Math.PI / 2);

    private static readonly Physics standardPhysics = new();

    private static readonly Vector standartRocketPos = new(200, 500);
    private static readonly Vector standartTargetPos = new(600, 200);

    private static readonly Gravity whiteHole = (size, v) =>
    {
        var d = v - standartTargetPos;
        var l = d.Length;
        return d.Normalize() * (140 * l / (l * l + 1));
    };

    private static readonly Gravity blackHole = (size, v) =>
    {
        var g = (standartTargetPos - standartRocketPos) / 2 + standartRocketPos;
        var d = (g - v).Length;
        return (g - v).Normalize() * (300 * d / (d * d + 1));
    };

    private static readonly Gravity blackAndWhiteHoles = (size, v) =>
    {
        return (whiteHole(size, v) + blackHole(size, v)) / 2;
    };

    private static Rocket InitializeRocket() => new(standartRocketPos, Vector.Zero, PiDivTwo);

    private static Level CreateLevel(string name, Vector targetPos, Gravity gravity)
    {
        return new Level(name, InitializeRocket(), targetPos, gravity, standardPhysics);
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