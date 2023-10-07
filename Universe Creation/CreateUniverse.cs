using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class CreateUniverse : MonoBehaviour
{
    public PlanetNode[] planets;

    public GameObject planetPrefab;

    public float gizmoRadius = 1;

    private LineRenderer lineRenderer;

    static Material lineMaterial;



    private void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        spawnPlanets();
        //drawPlanetLines();
    }

    void spawnPlanets()
    {
        foreach(PlanetNode planet in planets)
        {
            Transform spawnTransform = this.transform;
            spawnTransform.position = new Vector3(planet.planetPosition.x, planet.planetPosition.y, 0);
            GameObject.Instantiate(planetPrefab, spawnTransform.position, spawnTransform.rotation);
            Debug.Log("spawned planet at" + spawnTransform.position);
        }
    }

    void drawPlanetLines()
    {
        lineRenderer.positionCount = 0;
        foreach(PlanetNode planet in planets)
        {
            foreach(TravelLine travelLine in planet.travelLines)
            {
                lineRenderer.positionCount += 2;
                lineRenderer.SetPosition(lineRenderer.positionCount - 2, travelLine.startingPlanetPosition);
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, travelLine.endingPlanetPosition);
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach (PlanetNode planet in planets)
            Gizmos.DrawSphere(planet.planetPosition, gizmoRadius);
        drawPlanetLinesGizmo();
    }

    private void drawPlanetLinesGizmo()
    {
        foreach(PlanetNode planet in planets)
        {
            foreach(TravelLine travelLine in planet.travelLines)
            {
                travelLine.startingPlanetPosition = planet.planetPosition;
                Gizmos.DrawLine(travelLine.startingPlanetPosition, travelLine.endingPlanetPosition);
            }
        }
    }


    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);
        //fix random deplacement
        transform.position = new Vector3(0, 0, 0);

        // Draw lines
        GL.Begin(GL.LINES);

        foreach (PlanetNode planet in planets)
        {
            foreach (TravelLine travelLine in planet.travelLines)
            {
                travelLine.startingPlanetPosition = planet.planetPosition;
                GL.Vertex(travelLine.startingPlanetPosition);
                GL.Vertex(travelLine.endingPlanetPosition);
            }
        }
        GL.End();
        GL.PopMatrix();
    }
}
