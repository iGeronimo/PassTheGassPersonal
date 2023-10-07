using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeometryTools
{
    /// <summary>
    /// The value of 2^-19 is tiny
    /// </summary>
    public const float TINY = 1f / 524288;

    /// <summary>
    /// Try to join two points with an infinite line.
    /// </summary>
    /// <param name="A">The first point.</param>
    /// <param name="B">The second point.</param>
    /// <param name="line">The line joining the two points.</param>
    /// <returns>False if the two points are coincident, True otherwise.</returns>
    public static bool TryJoin(Vector2 A, Vector2 B, out InfiniteLine line)
    {
        float dx = B.x - A.x, dy = B.y - A.y;
        float m = A.x * B.y - A.y * B.x;
        line = new InfiniteLine(-dy, dx, m);
        return line.IsFinite;
    }
    /// <summary>
    /// Try to find the point where two infinite lines meet.
    /// </summary>
    /// <param name="L">The fist line.</param>
    /// <param name="M">The second line.</param>
    /// <param name="point">The point where the two lines meet.</param>
    /// <returns>False if the two lines are parallel, True othrwise.</returns>
    public static bool TryMeet(InfiniteLine L, InfiniteLine M, out Vector2 point)
    {
        (float a1, float b1, float c1) = L.Coeff;
        (float a2, float b2, float c2) = M.Coeff;
        float d = a1 * b2 - a2 * b1;
        if (d != 0)
        {
            point = new Vector2((b1 * c2 - b2 * c1) / d, (a2 * c1 - a1 * c2) / d);
            return true;
        }
        point = Vector2.zero;
        return false;
    }
}
