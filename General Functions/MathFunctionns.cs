using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public struct Line
{
    public Vector2 startingPoint;
    public Vector2 endingPoint;

    public Line(Vector2 start, Vector2 end)
    {
        this.startingPoint = new Vector2(start.x, start.y);
        this.endingPoint = new Vector2(end.x, end.y);
    }
}

public static class MathFunctions
{
    public static bool CheckLinesIntersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        float d1 = direction(C, D, A);
        float d2 = direction(C, D, B);
        float d3 = direction(A, B, C);
        float d4 = direction(A, B, D);

        if (((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) && ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0))) return true;
        else if (d1 == 0 && onSegment(C, D, A)) return true;
        else if (d2 == 0 && onSegment(C, D, B)) return true;
        else if (d3 == 0 && onSegment(A, B, C)) return true;
        else if (d4 == 0 && onSegment(A, B, D)) return true;
        else return false;
    }


    private static float crossProduct(Vector2 p1, Vector2 p2)
    {
        return (p1.x * p2.y) - (p2.x * p1.y);
    }

    private static float direction(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return crossProduct(new Vector2(p3.x - p1.x, p3.y - p1.y), new Vector2(p2.x - p1.x, p2.y - p1.y));
    }

    private static bool onSegment(Vector2 p1, Vector2 p2, Vector2 p)
    {
        if (Mathf.Min(p1.x, p2.x) <= p.x && p.x <= Mathf.Max(p1.x, p2.x) && Mathf.Min(p1.y, p2.y) <= p.y && p.y <= Mathf.Max(p1.y, p2.y))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool simpleCheckLinesIntersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {

        return ccw(A,C,D) != ccw(B,C,D) && ccw(A,B,C) != ccw(A,B,D);
    }

    private static bool ccw(Vector2 A, Vector2 B, Vector2 C)
    {
        return (C.y - A.y) * (B.x - A.x) > (B.y - A.y) * (C.x - A.x);
    }

    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public static bool onLine(Line l1, Vector2 p)
    {   //check whether p is on the line or not
        if (p.x <= Mathf.Max(l1.startingPoint.x, l1.endingPoint.x) && p.x >= Mathf.Min(l1.startingPoint.x, l1.endingPoint.x) &&
           (p.y <= Mathf.Max(l1.startingPoint.y, l1.endingPoint.y) && p.y >= Mathf.Min(l1.startingPoint.y, l1.endingPoint.y)) && l1.startingPoint != p && l1.endingPoint != p)
            return true;

        return false;
    }

    public static int directionV2(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);  
        if (val == 0)
            return 0;     //colinear
        else if (val < 0)
            return 2;    //anti-clockwise direction
        return 1;    //clockwise direction
    }

    public static bool isIntersect(Line l1, Line l2)
    {
        //four direction for two lines and points of other line
        int dir1 = directionV2(l1.startingPoint, l1.endingPoint, l2.startingPoint);
        int dir2 = directionV2(l1.startingPoint, l1.endingPoint, l2.endingPoint);
        int dir3 = directionV2(l2.startingPoint, l2.endingPoint, l1.startingPoint);
        int dir4 = directionV2(l2.startingPoint, l2.endingPoint, l1.endingPoint);

        if (dir1 != dir2 && dir3 != dir4)
            return true; //they are intersecting

        if (dir1 == 0 && onLine(l1, l2.startingPoint)) //when p2 of line2 are on the line1
            return true;

        if (dir2 == 0 && onLine(l1, l2.endingPoint)) //when p1 of line2 are on the line1
            return true;

        if (dir3 == 0 && onLine(l2, l1.startingPoint)) //when p2 of line1 are on the line2
            return true;

        if (dir4 == 0 && onLine(l2, l1.endingPoint)) //when p1 of line1 are on the line2
            return true;

        return false;
    }
}
