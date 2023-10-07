using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelRestrictions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    //Didn't work out 


    //public void canTravelV2(Vector2 position)
    //{
    //    UniversalLists.activeLines.Clear();
    //    foreach (GameObject planet in PlanetManager.planetManager.planets)
    //    {
    //        if (planet != null)
    //        {
    //            planet.GetComponent<PlanetScript>().canTravel = false;
    //            if ((Vector2)planet.transform.position == position)
    //            {
    //                LineRenderer[] lines = planet.GetComponentsInChildren<LineRenderer>();
    //                foreach (LineRenderer line in lines)
    //                {
    //                    for (int i = 0; i < line.positionCount; i++)
    //                    {
    //                        UniversalLists.activeLines.Add((Vector2)line.GetPosition(i));
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    foreach (GameObject planet in PlanetManager.planetManager.planets)
    //    {
    //        if (planet != null)
    //        {

    //            foreach (Vector2 linePosition in UniversalLists.activeLines)
    //            {
    //                if (linePosition == (Vector2)planet.transform.position)
    //                {
    //                    planet.GetComponent<PlanetScript>().canTravel = true;
    //                }
    //            }
    //        }
    //    }
    //}

    public void canTravel(Vector2 position)
    {
        PlanetManager.planetManager.activeLines.Clear();
        foreach (GameObject planet in PlanetManager.planetManager.planets)
        {
            if (planet != null)
            {
                PlanetScript setPlanet = planet.GetComponent<PlanetScript>();
                setPlanet.canTravel = false;
                LineRenderer[] lines;
                lines = planet.GetComponentsInChildren<LineRenderer>();
                Debug.Log(lines.Length + " line renderers found");
                bool possibleToTravel = false;
                foreach (LineRenderer line in lines)
                {
                    bool changeLineColor = false;

                    Vector3[] linePositions = new Vector3[line.positionCount];
                    for (int i = 0; i < linePositions.Length; i++)
                    {
                        linePositions[i] = line.GetPosition(i);
                        if ((Vector2)linePositions[i] == position)
                        {
                            changeLineColor = true;
                            possibleToTravel = true;
                        }
                    }
                    if (changeLineColor)
                    {
                        for (int i = 0; i < line.positionCount; i++)
                        {
                            PlanetManager.planetManager.activeLines.Add(line.GetPosition(i));
                        }
                    }
                    else
                    {
                    }
                    if (!possibleToTravel)
                    {
                        planet.GetComponent<PlanetScript>().canTravel = false;
                    }
                }
            }
        }
        foreach (GameObject planet in PlanetManager.planetManager.planets)
        {
            if (planet != null)
            {
                foreach (Vector2 linePosition in PlanetManager.planetManager.activeLines)
                {
                    if (linePosition == (Vector2)planet.transform.position)
                    {
                        planet.GetComponent<PlanetScript>().canTravel = true;
                    }
                }
            }
        }
    }
}
