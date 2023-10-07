using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBootup : MonoBehaviour
{
    public Vector2 planetPosition = Vector2.zero;


    //Additional positions
    public Vector2 previousEndingLinePosition;
    public Vector2 currentEndingLinePosition;
    public Vector2 nextEndingLinePosition;

    public bool alteredPrevious = false;
    public bool alteredCurrent = false;
    public bool alteredNext = false;

    public int ringIndex = 0;

    private void Awake()
    {
        alteredPrevious = false;
        alteredCurrent = false;
        alteredNext = false;

        previousEndingLinePosition = Vector2.zero;
        currentEndingLinePosition = Vector2.zero;
        nextEndingLinePosition = Vector2.zero;
    }



    public void setPrevious(Vector2 newEnding)
    {
        previousEndingLinePosition = newEnding;
        alteredPrevious = true;
    }

    public void setCurrent(Vector2 newEnding)
    {
        currentEndingLinePosition = newEnding;
        alteredCurrent = true;
    }

    public void setNext(Vector2 newEnding)
    {
        nextEndingLinePosition = newEnding;
        alteredNext = true;
    }





}
