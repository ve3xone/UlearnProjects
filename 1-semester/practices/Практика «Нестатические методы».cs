using System;

namespace Geometry;

public class Geometry
{
    public static double GetLength(Vector vec)
    {
        return Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
    }

    public static double GetLength(Segment s)
    {
        return GetLength(Vector.Subtract(s.End, s.Begin));
    }

    public static Vector Add(Vector vec1, Vector vec2)
    {
        return new Vector() { X = vec1.X + vec2.X, Y = vec1.Y + vec2.Y };
    }

    public static bool IsVectorInSegment(Vector vec, Segment seg)
    {
		if (((vec.X == seg.Begin.X) || (vec.X == seg.End.X)) 
			&& ((vec.Y == seg.End.Y) || (vec.Y == seg.Begin.Y)))
		{
			return true;
		}
        
		return ((vec.X - seg.Begin.X) * (vec.X - seg.End.X) <= 0) 
				 && ((vec.Y - seg.Begin.Y) * (vec.Y - seg.End.Y) < 0);
    }
}

public class Vector
{
    public double X;
	public double Y;

    public static Vector Subtract(Vector vec1, Vector vec2)
    {
        return new Vector { X = vec1.X - vec2.X, Y = vec1.Y - vec2.Y };
    }

    public double GetLength()
    {
        return Geometry.GetLength(this);
    }


    public Vector Add(Vector v)
    {
        return Geometry.Add(this, v);
    }


    public bool Belongs(Segment s)
    {
        return Geometry.IsVectorInSegment(this, s);
    }
}

public class Segment
{
    public Vector Begin;
    public Vector End;

    public double GetLength()
    {
        return Geometry.GetLength(this);
    }

    public bool Contains(Vector v)
    {
        return Geometry.IsVectorInSegment(v, this);
    }
}