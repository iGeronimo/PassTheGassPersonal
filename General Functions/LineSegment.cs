using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct LineSegment
{
    public LineSegment(Vector2 from, Vector2 to) : this()
    {
        From = from;
        To = to;
    }

    public Vector2 From { get; }
    public Vector2 To { get; }

    public float Length { get => Vector2.Distance(From, To); }
    public Vector2 Direction { get => (To - From).normalized; }
    public Vector2 Normal { get => new Vector2(-(To.y - From.y), (To.x - From.x)).normalized; }

    public bool IsFinite
    {
        get => !float.IsNaN(From.x) && !float.IsNaN(From.y)
            && !float.IsNaN(To.x) && !float.IsNaN(To.y);
    }
    /// <summary>
    /// Check if a point is contained within the line segment
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool Contains(Vector2 target, bool inclusive = true)
    {
        if (GeometryTools.TryJoin(From, To, out var line))
        {
            target = line.Project(target);
            Vector2 dir = Direction;
            float t = Vector2.Dot(dir, target - From) / Vector2.Dot(dir, To - From);
            return inclusive ? t >= 0 && t <= 1 : t > GeometryTools.TINY && t < 1 - GeometryTools.TINY;
        }
        return false;
    }
    /// <summary>
    /// Try to intersect two line segments
    /// </summary>
    /// <param name="other">The other line segment.</param>
    /// <param name="point">The intersection point.</param>
    /// <returns>True if they intersect, False otherwise</returns>
    public bool TryIntersect(LineSegment other, out Vector2 point, bool inclusive = true)
    {
        point = Vector2.zero;

        if (GeometryTools.TryJoin(From, To, out InfiniteLine thisLine)
            && GeometryTools.TryJoin(other.From, other.To, out InfiniteLine otherLine))
        {
            if (GeometryTools.TryMeet(thisLine, otherLine, out point))
            {
                return Contains(point, inclusive) && other.Contains(point, inclusive);
            }
        }
        return false;
    }
    public override string ToString() => $"[{From}-{To}]";
}
