using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RefactorMe;

class Drawer
{
    private static float x, y;
    private static Graphics graphics;

    public static void Init(Graphics newGraphics)
    {
        graphics = newGraphics;
        graphics.SmoothingMode = SmoothingMode.None;
        graphics.Clear(Color.Black);
    }

    public static void SetPosition(float x0, float y0)
    {
        x = x0;
        y = y0;
    }

    public static void Move(double length, double corner)
    {
        x = (float)(x + length * Math.Cos(corner));
        y = (float)(y + length * Math.Sin(corner));
    }

    public static void DrawStep(Pen pen, double length, double corner)
    {
        //Делает шаг длиной length в направлении corner и рисует пройденную траекторию
        var x1 = (float)(x + length * Math.Cos(corner));
        var y1 = (float)(y + length * Math.Sin(corner));
        graphics.DrawLine(pen, x, y, x1, y1);
        x = x1;
        y = y1;
    }
}

public class ImpossibleSquare
{
    private const double SquareRatio = 0.375;
    private const double OffsetRatio = 0.04;

    public static void Draw(int width, int height, double angleofRotation, Graphics graphics)
    {
        // angleofRotation пока не используется, но будет использоваться в будущем
        Drawer.Init(graphics);

        var sz = Math.Min(width, height);

        var diagonalLength = Math.Sqrt(2) * (sz * SquareRatio + sz * OffsetRatio) / 2;
        var x0 = (float)(diagonalLength * Math.Cos(Math.PI / 4 + Math.PI)) + width / 2f;
        var y0 = (float)(diagonalLength * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;

        Drawer.SetPosition(x0, y0);
        DrawSquare(sz);
    }

    private static void DrawSquare(int sz)
    {
        DrawSide(sz, 0);
        DrawSide(sz, -Math.PI / 2);
        DrawSide(sz, Math.PI);
        DrawSide(sz, Math.PI / 2);
    }

    private static void DrawSide(int sz, double corner)
    {
        Drawer.DrawStep(Pens.Yellow, sz * SquareRatio, corner);
        Drawer.DrawStep(Pens.Yellow, sz * OffsetRatio * Math.Sqrt(2), corner + Math.PI / 4);
        Drawer.DrawStep(Pens.Yellow, sz * SquareRatio, corner + Math.PI);
        Drawer.DrawStep(Pens.Yellow, sz * SquareRatio - sz * OffsetRatio, corner + Math.PI / 2);

        Drawer.Move(sz * OffsetRatio, corner - Math.PI);
        Drawer.Move(sz * OffsetRatio * Math.Sqrt(2), corner + 3 * Math.PI / 4);
    }
}