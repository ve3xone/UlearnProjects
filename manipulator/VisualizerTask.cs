using System;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;

namespace Manipulation;

public static class VisualizerTask
{
    public static double X = 220;
    public static double Y = -100;
    public static double Alpha = 0.05;
    public static double Wrist = 2 * Math.PI / 3;
    public static double Elbow = 3 * Math.PI / 4;
    public static double Shoulder = Math.PI / 2;
    private const double DegreesToRadians = Math.PI / 180;

    public static Brush UnreachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 255, 230, 230));
    public static Brush ReachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 230, 255, 230));
    public static Pen ManipulatorPen = new Pen(Brushes.Black, 3);
    public static Brush JointBrush = new SolidColorBrush(Colors.Gray);

    public static void KeyDown(IDisplay display, KeyEventArgs key)
    {
        switch (key.Key)
        {
            case Key.W:
                Elbow += DegreesToRadians;
                break;
            case Key.A:
                Shoulder -= DegreesToRadians;
                break;
            case Key.S:
                Elbow -= DegreesToRadians;
                break;
            case Key.Q:
                Shoulder += DegreesToRadians;
                break;
        }
        Wrist = -Alpha - Shoulder - Elbow;
        display.InvalidateVisual();
    }

    public static void MouseMove(IDisplay display, PointerEventArgs e)
    {
        var pos = e.GetPosition(display);
        var mathPoint = ConvertWindowToMath(new Point(pos.X, pos.Y), GetShoulderPos(display));
        X = mathPoint.X;
        Y = mathPoint.Y;
        UpdateManipulator();
        display.InvalidateVisual();
    }

    public static void MouseWheel(IDisplay display, PointerWheelEventArgs e)
    {
        Alpha += e.Delta.Y;
        UpdateManipulator();
        display.InvalidateVisual();
    }

    public static void UpdateManipulator()
    {
        double[] angles = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
        if (angles.Any(double.IsNaN))
            return;
        Shoulder = angles[0];
        Elbow = angles[1];
        Wrist = angles[2];
    }

    public static void DrawManipulator(DrawingContext context, Point shoulderPos)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);

        var points = new Point[joints.Length + 1];
        points[0] = shoulderPos;

        DrawReachableZone(context, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);

        var formattedText = new FormattedText($"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}", Typeface.Default, 18,
            TextAlignment.Center, TextWrapping.Wrap, Size.Empty);
        context.DrawText(Brushes.DarkRed, new Point(10, 10), formattedText);

        for (var i = 0; i < joints.Length; i++)
        {
            points[i + 1] = ConvertMathToWindow(joints[i], shoulderPos);
            context.DrawLine(ManipulatorPen, points[i], points[i + 1]);
            context.DrawEllipse(JointBrush, ManipulatorPen, new Point(points[i].X - 5, points[i].Y - 5), 10, 10);
        }
    }

    private static void DrawReachableZone(
        DrawingContext context,
        Brush reachableBrush,
        Brush unreachableBrush,
        Point shoulderPos,
        Point[] joints)
    {
        var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
        var rmax = Manipulator.UpperArm + Manipulator.Forearm;
        var mathCenter = new Point(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
        var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
        context.DrawEllipse(reachableBrush,
            null,
            new Point(windowCenter.X, windowCenter.Y),
            rmax, rmax);
        context.DrawEllipse(unreachableBrush,
            null,
            new Point(windowCenter.X, windowCenter.Y),
            rmin, rmin);
    }

    public static Point GetShoulderPos(IDisplay display)
    {
        return new Point(display.Bounds.Width / 2, display.Bounds.Height / 2);
    }

    public static Point ConvertMathToWindow(Point mathPoint, Point shoulderPos)
    {
        return new Point(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
    }

    public static Point ConvertWindowToMath(Point windowPoint, Point shoulderPos)
    {
        return new Point(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
    }
}