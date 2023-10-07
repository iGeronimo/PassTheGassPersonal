using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetNode
{
    public Vector2 planetPosition;
    public List<TravelLine> travelLines;
    public PlanetNode()
    {
        this.travelLines = new List<TravelLine>();
    }
}
