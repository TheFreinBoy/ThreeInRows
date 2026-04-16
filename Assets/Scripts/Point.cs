using UnityEngine;

[System.Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void Multiply(int value)
    {
        x*= value;
        y*= value;
    }
    public void Add(Point point)
    {
        x+= point.x;
        y+= point.y;
    }
    public bool Equals(Point point)
    {
        return x == point.x && y == point.y;
    }
    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }
    public static Point FromVector(Vector2 vector)
    {
        return new Point((int)vector.x, (int)vector.y);
    }
    public static Point FromVector(Vector3 vector)
    {
        return new Point((int)vector.x, (int)vector.y);
    }
    public static Point Multiply(Point point, int value)
    {
        return new Point(point.x * value, point.y * value);
    }
    public static Point Add(Point point1, Point point2)
    {
        return new Point(point1.x + point2.x, point1.y + point2.y);
    }
    public static Point Clone(Point point)
    {
        return new Point(point.x, point.y);
    }
    public static Point zero()
    {
        return new Point(0, 0);
    }
    public static Point up()
    {
        return new Point(0, 1);
    }
    public static Point down()
    {
        return new Point(0, -1);
    }
    public static Point left()
    {
        return new Point(-1, 0);
    }
    public static Point right()
    {
        return new Point(1, 0);
    }
}