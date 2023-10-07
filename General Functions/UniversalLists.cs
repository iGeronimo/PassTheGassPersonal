using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UniversalLists 
{
    public static GameObject[] planets = null;
    public static GameObject currentPlanet = null;
    public static List<Vector2> activeLines = new List<Vector2>();
    public static List<string> PlanetNames = new List<string>();
    public static SpaceSaveValues saveValues = new SpaceSaveValues();

    public static void SetPlanets(GameObject[] newPlanets)
    {
        planets = newPlanets;
     
    }

    public static void setCurrentPlanet(GameObject newPlanet)
    {
        currentPlanet = newPlanet;
    }

    public static void resetLists()
    {
        planets= null;
        currentPlanet= null;
        activeLines = new List<Vector2>();
        PlanetNames = new List<string>();
    }
}
