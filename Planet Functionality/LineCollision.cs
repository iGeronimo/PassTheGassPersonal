using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class LineCollision : MonoBehaviour
{
    LineRenderer lr;
    PolygonCollider2D pCollider;
    List<Vector2> colliderPoints;

    bool linesSet = false;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        pCollider = GetComponent<PolygonCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        onLinesReady();
    }

    void onLinesReady()
    {
        if (!linesSet)
        {
            if (PlanetManager.planetManager.linesGenerated)
            {
                setCollision();
                pCollider.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
                linesSet = true;
            }
        }
    }

    void setCollision()
    {
        Vector3[] points = new Vector3[lr.positionCount];
        lr.GetPositions(points);

        float width = lr.startWidth * 0.01f;

        float m = (points[1].y - points[0].y) / (points[1].x - points[0].x);
        float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        Vector3[] offsets = new Vector3[2];
        offsets[0] = new Vector3(-deltaX, deltaY);
        offsets[1] = new Vector3(deltaX, -deltaY);

        colliderPoints = new List<Vector2>
        {
            points[0] + offsets[0],
            points[1] + offsets[0],
            points[1] + offsets[1],
            points[0] + offsets[1]
        };
    }
}
