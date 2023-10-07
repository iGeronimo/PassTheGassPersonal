using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct InfiniteLine
{
    /// <summary>
    /// The line at the horizon (not on the Eucledian plane).
    /// </summary>
    public static readonly InfiniteLine Horizon = new InfiniteLine(0, 0, 1);

    public InfiniteLine(float a, float b, float c) : this()
    {
        this.Coeff = (a, b, c);
        float m = (float)Mathf.Sqrt(a * a + b * b);
        this.IsFinite = m > GeometryTools.TINY;
    }
    /// <summary>
    /// The (a,b,c) coefficients define a line by the equation: <code>a*x+b*y+c=0</code>
    /// </summary>
    public (float a, float b, float c) Coeff { get; }

    /// <summary>
    /// True if line is in finite space, False if line is at horizon.
    /// </summary>
    public bool IsFinite { get; }

    /// <summary>
    /// Check if point belongs to the infinite line.
    /// </summary>
    /// <param name="point">The target point.</param>
    /// <returns>True if point is one the line.</returns>
    public bool Contains(Vector2 point)
    {
        return IsFinite
            && Mathf.Abs(Coeff.a * point.x + Coeff.b * point.y + Coeff.c) <= GeometryTools.TINY;
    }

    /// <summary>
    /// Projects a target point onto the line.
    /// </summary>
    /// <param name="target">The target point.</param>
    /// <returns>The point on the line closest to the target.</returns>
    /// <remarks>If line is not finite the resulting point has NaN or Inf coordinates.</remarks>
    public Vector2 Project(Vector2 target)
    {
        (float a, float b, float c) = Coeff;
        float m2 = a * a + b * b;
        float px = b * (b * target.x - a * target.y) - a * c;
        float py = a * (a * target.y - b * target.x) - b * c;
        return new Vector2(px / m2, py / m2);
    }
    public override string ToString() => $"({Coeff.a})*x+({Coeff.b})*y+({Coeff.c})=0";
}