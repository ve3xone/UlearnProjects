using System;
using System.Collections.Generic;

namespace func_rocket;

public static class LevelsTask
{
    private const double PIDivTwo = (-Math.PI / 2);

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

    public static IEnumerable<Level> CreateLevels()
    {
        yield return new Level("Zero",
            new Rocket(standartRocketPos, Vector.Zero, PIDivTwo),
            standartTargetPos,
            (size, v) => Vector.Zero, standardPhysics);

        yield return new Level("Heavy",
            new Rocket(standartRocketPos, Vector.Zero, PIDivTwo),
            standartTargetPos,
            (size, v) => new Vector(0, .9), standardPhysics);

        yield return new Level("Up",
            new Rocket(standartRocketPos, Vector.Zero, PIDivTwo),
            new Vector(700, 500),
            (size, v) => new Vector(0, -300.0 / (size.Y - v.Y + 300)),
            standardPhysics);

        yield return new Level("WhiteHole",
            new Rocket(standartRocketPos, Vector.Zero, PIDivTwo),
            standartTargetPos,
            whiteHole,
            standardPhysics);

        yield return new Level("BlackHole",
            new Rocket(standartRocketPos, Vector.Zero, PIDivTwo),
            standartTargetPos,
            blackHole,
            standardPhysics);

        yield return new Level("BlackAndWhite",
            new Rocket(standartRocketPos, Vector.Zero, PIDivTwo),
            standartTargetPos,
            (size, v) => (whiteHole(size, v) + blackHole(size, v)) / 2,
            standardPhysics);
    }
}