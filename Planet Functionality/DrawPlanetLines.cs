using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPlanetLines : MonoBehaviour
{
    public GameObject previousOB;
    public GameObject currentOB;   
    public GameObject nextOB;
    
    private LineRenderer previousLines;
    private LineRenderer currentLines;
    private LineRenderer nextLines;
    private PlanetBootup linePositions;

    public float width = 1.0f;

    public bool lineSet = false;

    void Start()
    {
        linePositions = this.GetComponent<PlanetBootup>();
        
        //previous
        previousLines = previousOB.GetComponent<LineRenderer>();
        //previousLines.startColor = Color.red;
        //previousLines.endColor = Color.red;
        //previousLines.material.color = Color.red;
        previousLines.startWidth = width;
        previousLines.endWidth = width;

        //current
        currentLines = currentOB.GetComponent<LineRenderer>();
        //currentLines.startColor = Color.green;
        //currentLines.endColor = Color.green;
        //currentLines.material.color = Color.green;
        currentLines.startWidth = width;
        currentLines.endWidth = width;

        //next
        nextLines = nextOB.GetComponent<LineRenderer>();
        //nextLines.startColor = Color.white;
        //nextLines.endColor = Color.white;
        //nextLines.material.color = Color.white;
        nextLines.startWidth = width;
        nextLines.endWidth = width;
        
    }

    private void setLines()
    {
        bool usedPrevious = false;
        bool usedCurrent = false;
        bool usedNext = false;

        if (linePositions.alteredPrevious) { usedPrevious = true; }
        if (linePositions.alteredCurrent) { usedCurrent = true; }
        if (linePositions.alteredNext) { usedNext = true; }

        if (usedPrevious)
        {
            Vector3[] points = new Vector3[2];
            previousLines.positionCount = 2;
            points[0] = linePositions.planetPosition;
            points[1] = linePositions.previousEndingLinePosition;
            previousLines.SetPositions(points);
        }
        if (usedCurrent)
        {
            Vector3[] points = new Vector3[2];
            currentLines.positionCount = 2;
            points[0] = linePositions.planetPosition;
            points[1] = linePositions.currentEndingLinePosition;
            currentLines.SetPositions(points);
        }
        if (usedNext && linePositions.ringIndex != 3)
        {
            Vector3[] points = new Vector3[2];
            nextLines.positionCount = 2;
            points[0] = linePositions.planetPosition;
            points[1] = linePositions.nextEndingLinePosition;
            nextLines.SetPositions(points);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(PlanetManager.planetManager.linesGenerated)
        {
            if(!lineSet)
            {
                setLines();
                lineSet = true;
            }
        }
    }
}
