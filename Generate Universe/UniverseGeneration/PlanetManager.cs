using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager planetManager { get; private set; }
    private void OnEnable()
    {
        if (planetManager != null && planetManager != this)
        {
            Destroy(this);
        }
        else
        {
            planetManager = this;
        }
    }

    #region variables
    #region public

    public bool loadFinished = false;
    public SpaceSaveValues spaceSaveValues;
    public GameObject currentPlanet;
    public GameObject[] planets;
    public List<Vector2> activeLines = new List<Vector2>();

    public GameObject planetPrefab;

    [Header("Creation Parameters")]
    public int amountOfCircles;
    public int planetsInFirstRing;
    public float distanceIncreasePerCircle;
    public float minDistanceBetweenPlanets;
    public float minAngleDifference;
    public float firstCircleRadius;


    public float failSafeAttempts;

    [Header("Gloabl Dependancies")]
    public bool linesGenerated = false;



    #endregion

    #region private

    private List<Vector2> currentCirclesPlanets = new List<Vector2>();
    private List<Vector2> lastCirclesPlanets = new List<Vector2>();
    private List<float> usedAngles = new List<float>();
    private List<LineSegment> existingLines = new List<LineSegment>();
    private int currentCircle = 0;
    private int currentPlanetDivision;
    private bool generated = false;

    #endregion

    #endregion

    void loadOnSave(bool saveAvailable)
    {
        UniversalLists.PlanetNames.Clear();
        if (saveAvailable)
        {
            foreach (PlanetSaveData planetSave in spaceSaveValues.planets)
            {
                GameObject ob = Instantiate(planetPrefab) as GameObject;
                ob.transform.position = planetSave.planetPosition;

                ob.GetComponent<PlanetBootup>().planetPosition = planetSave.planetPosition;
                ob.GetComponent<PlanetBootup>().previousEndingLinePosition = planetSave.planetPreviousLine;
                ob.GetComponent<PlanetBootup>().currentEndingLinePosition = planetSave.planetCurrentLine;
                ob.GetComponent<PlanetBootup>().nextEndingLinePosition = planetSave.planetNextLine;

                ob.GetComponent<PlanetBootup>().alteredPrevious = planetSave.alteredPrevious;
                ob.GetComponent<PlanetBootup>().alteredCurrent = planetSave.alteredCurrent;
                ob.GetComponent<PlanetBootup>().alteredNext = planetSave.alteredNext;

                ob.GetComponent<PlanetBootup>().ringIndex = planetSave.planetRingIndex;

                ob.GetComponent<PlanetVariation>().metalResource = planetSave.planetMetal;
                ob.GetComponent<PlanetVariation>().crystalResource = planetSave.planetCrystal;
                ob.GetComponent<PlanetVariation>().uraniumResource = planetSave.planetUranium;

                ob.GetComponent<PlanetVariation>().spriteNumber = planetSave.planetSprite;
                ob.name = planetSave.planetName;
                ob.GetComponent<PlanetScript>().TravelRisk = planetSave.travelRisk;
            }
            Debug.Log("Loaded planets from save");
            
        }

        if (!saveAvailable)
        {
            generatePlanet();
            setLines();
            setPlanetVariation();
            setPlanetRisk();
            Debug.Log("Loaded new planets");
        }
    }

    #region planetGeneration
    private void generatePlanet()
    {
        //Start of planet generation
        lastCirclesPlanets.Add(Vector2.zero); //start from 0,0

        for (int i = 0; i < amountOfCircles; i++)
        {
            currentCircle = i;
            foreach (Vector2 planet in lastCirclesPlanets)
            {
                planetDivision();
                usedAngles.Clear();
                for (int j = 0; j < currentPlanetDivision; j++)
                {
                    for (int k = 0; k < failSafeAttempts; k++)
                    {
                        Vector2 newPosition = planetPosition(planet);
                        if (planetPositionValidate(newPosition, planet))
                        {
                            addUsedAngle(newPosition, planet);  // add angle for future check
                            makePlanet(newPosition);            //create planet
                            break;
                        }
                    }

                }
            }
            lastCirclesPlanets.Clear();
            lastCirclesPlanets = new List<Vector2>(currentCirclesPlanets);
            currentCirclesPlanets.Clear();
        }
        planets = GameObject.FindGameObjectsWithTag("Planet");
    }

    void makePlanet(Vector2 pPosition)
    {
        currentCirclesPlanets.Add(pPosition);
        GameObject ob = Instantiate(planetPrefab) as GameObject;
        ob.transform.position = pPosition;
        ob.GetComponent<PlanetBootup>().ringIndex = currentCircle;
    }

    Vector2 planetPosition(Vector2 lastPlanet)
    {
        Vector2 returningVector = lastPlanet;
        Vector2 angle = lastPlanet.normalized;

        angle = MathFunctions.rotate(angle, Random.Range(-180, 181) * Mathf.Deg2Rad); //rotate angle randomly between range

        Vector2 offset = new Vector2(Random.Range(-CircleRadius(), CircleRadius()), Random.Range(-CircleRadius(), CircleRadius()));
        float differenceInAngle = Mathf.Atan2(offset.y - angle.y, offset.x - angle.x);

        returningVector += offset;

        return returningVector;
    }


    #region planetValidation
    private bool planetPositionValidate(Vector2 position, Vector2 lastPosition)
    {
        if (!checkUsedAngles(position, lastPosition) || !checkNeighbourDistance(position) || !checkCorrectRing(position))
        {
            return false;
        }
        return true;
    }

    private bool checkUsedAngles(Vector2 position, Vector2 lastPosition)
    {
        foreach (float angle in usedAngles)
        {
            if (Mathf.Abs(angle - Vector2.Angle(lastPosition, position)) <= minAngleDifference && Mathf.Abs(angle - Vector2.Angle(lastPosition, position)) != 0)
            {
                return false;
            }
        }
        return true;
    }

    private void addUsedAngle(Vector2 position, Vector2 lastPosition)
    {
        float angle = Vector2.Angle(lastPosition, position);
        usedAngles.Add(angle);
    }

    private bool checkNeighbourDistance(Vector2 position)
    {
        foreach (Vector2 planet in currentCirclesPlanets)
        {
            if (Vector2.Distance(planet, position) < minDistanceBetweenPlanets)
            {
                return false;
            }
        }
        foreach (Vector2 planet in lastCirclesPlanets)
        {
            if (Vector2.Distance(planet, position) < minDistanceBetweenPlanets)
            {
                return false;
            }
        }
        return true;
    }

    private bool checkCorrectRing(Vector2 position)
    {
        if (Mathf.Pow(position.x, 2) + Mathf.Pow(position.y, 2) < Mathf.Pow(CircleRadius(currentCircle + 2), 2) &&
                !(Mathf.Pow(position.x, 2) + Mathf.Pow(position.y, 2) < Mathf.Pow(CircleRadius(currentCircle), 2)))
        {
            return true;
        }
        return false;
    }

    #endregion
    #endregion

    #region generation rate

    void planetDivision()
    {
        currentPlanetDivision = planetsInFirstRing / (currentCircle + 1);
        if (currentPlanetDivision <= 0)
        {
            currentPlanetDivision = 0;
        }
    }

    float CircleRadius()
    {
        float currentRadius;

        currentRadius = firstCircleRadius * ((currentCircle * distanceIncreasePerCircle) + 1);

        return currentRadius;
    }

    float CircleRadius(int targetCircle)
    {
        float currentRadius;

        currentRadius = firstCircleRadius * ((targetCircle * distanceIncreasePerCircle) + 1);

        return currentRadius;
    }

    #endregion

    #region lineGeneration

    void setLines()
    {
        setPrevious();
        setNext();
        setCurrent();
        linesGenerated = true;
    }

    void setPrevious()
    {
        //previous lines
        for (int i = 0; i < planets.Length; i++)
        {
            bool alteredEnding = false;
            float shortest = float.MaxValue;
            Vector2 newEnding = Vector2.zero;
            PlanetBootup iBootup = planets[i].GetComponent<PlanetBootup>();
            iBootup.planetPosition = planets[i].transform.position;//set its own position
            if (iBootup.ringIndex == 0)
            {
                iBootup.alteredPrevious = true;
                iBootup.previousEndingLinePosition = newEnding;
                existingLines.Add(new LineSegment(planets[i].transform.position, newEnding));
            }
            else
            {
                for (int j = 0; j < planets.Length; j++)
                {
                    PlanetBootup jBootup = planets[j].GetComponent<PlanetBootup>();
                    if (j != i)
                    {
                        if (iBootup.ringIndex == jBootup.ringIndex + 1)
                        {
                            if (Vector2.Distance(planets[i].transform.position, planets[j].transform.position) < shortest)
                            {
                                bool noIntersects = true;
                                foreach (LineSegment line in existingLines)
                                {
                                    if (line.TryIntersect(new LineSegment(planets[i].transform.position, planets[j].transform.position), out Vector2 point, false))
                                    {
                                        noIntersects = false;
                                    }
                                }
                                if (noIntersects)
                                {
                                    shortest = Vector2.Distance(planets[i].transform.position, planets[j].transform.position);
                                    newEnding = planets[j].transform.position;
                                    alteredEnding = true;
                                }
                            }
                        }
                    }
                }
                if (alteredEnding)
                {
                    iBootup.setPrevious(newEnding);
                    existingLines.Add(new LineSegment(planets[i].transform.position, newEnding));
                }
            }
        }
    }

    void setNext()
    {
        //next lines
        for (int i = 0; i < planets.Length; i++)
        {
            bool alteredEnding = false;
            float shortest = float.MaxValue;
            Vector2 newEnding = Vector2.zero;
            PlanetBootup iBootup = planets[i].GetComponent<PlanetBootup>();
            if (iBootup.ringIndex == amountOfCircles)
            {
                return;
            }
            else
            {
                for (int j = 0; j < planets.Length; j++)
                {
                    PlanetBootup jBootup = planets[j].GetComponent<PlanetBootup>();
                    if (j != i)
                    {
                        if (iBootup.ringIndex == jBootup.ringIndex - 1)
                        {
                            if (Vector2.Distance(planets[i].transform.position, planets[j].transform.position) < shortest && (Vector2)planets[j].transform.position != iBootup.previousEndingLinePosition)
                            {
                                bool noIntersects = true;
                                foreach (LineSegment line in existingLines)
                                {
                                    if (line.TryIntersect(new LineSegment(planets[i].transform.position, planets[j].transform.position), out Vector2 point, false))
                                    {
                                        noIntersects = false;
                                    }
                                }
                                if (noIntersects)
                                {
                                    shortest = Vector2.Distance(planets[i].transform.position, planets[j].transform.position);
                                    newEnding = planets[j].transform.position;
                                    alteredEnding = true;
                                }
                            }
                        }
                    }
                }
                if (alteredEnding)
                {
                    iBootup.setNext(newEnding);
                    existingLines.Add(new LineSegment(planets[i].transform.position, newEnding));
                }
            }
        }
    }

    void setCurrent()
    {
        //current lines
        for (int i = 0; i < planets.Length; i++)
        {
            bool alteredEnding = false;
            float shortest = float.MaxValue;
            Vector2 newEnding = Vector2.zero;
            PlanetBootup iBootup = planets[i].GetComponent<PlanetBootup>();
            if (iBootup.ringIndex == amountOfCircles)
            {
                return;
            }
            else
            {
                for (int j = 0; j < planets.Length; j++)
                {
                    PlanetBootup jBootup = planets[j].GetComponent<PlanetBootup>();
                    if (j != i)
                    {
                        if (iBootup.ringIndex == jBootup.ringIndex)
                        {
                            if (Vector2.Distance(planets[i].transform.position, planets[j].transform.position) < shortest && (Vector2)planets[i].transform.position != jBootup.currentEndingLinePosition)
                            {
                                bool noIntersects = true;
                                foreach (LineSegment line in existingLines)
                                {
                                    if (line.TryIntersect(new LineSegment(planets[i].transform.position, planets[j].transform.position), out Vector2 point, false))
                                    {
                                        noIntersects = false;
                                    }
                                }
                                if (noIntersects)
                                {
                                    shortest = Vector2.Distance(planets[i].transform.position, planets[j].transform.position);
                                    newEnding = planets[j].transform.position;
                                    alteredEnding = true;
                                }
                            }
                        }
                    }
                }
                if (alteredEnding)
                {
                    iBootup.setCurrent(newEnding);
                    existingLines.Add(new LineSegment(planets[i].transform.position, newEnding));
                }
            }
        }
    }
    #endregion

    #region setupPlanets

    void setPlanetVariation()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].GetComponent<PlanetVariation>().setVariation();
        }
    }
    void setPlanetRisk()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].GetComponent<PlanetScript>().setRisk();
        }
    }

    #endregion

    private void Update()
    {
        if (!generated)
        {
            if (loadFinished)
            {
                loadOnSave(spaceSaveValues.saveAvailable);
                planets = GameObject.FindGameObjectsWithTag("Planet");
                linesGenerated = true;
                GameObject.FindGameObjectWithTag("SpaceShip").GetComponent<SetSpaceShip>().setSpaceShip(spaceSaveValues.saveAvailable ,spaceSaveValues);
                UniversalLists.PlanetNames.Clear();
                generated = true;
            }
        }
    }
}



[System.Serializable]
public class PlanetSaveData
{
    public Vector2 planetPosition;
    public Vector2 planetPreviousLine;
    public Vector2 planetCurrentLine;
    public Vector2 planetNextLine;

    public bool alteredPrevious;
    public bool alteredCurrent;
    public bool alteredNext;

    public int planetRingIndex;

    public int planetMetal;
    public int planetCrystal;
    public int planetUranium;

    public int travelRisk;

    public int planetSprite;
    public string planetName;

    public PlanetSaveData()
    {

    }

    public PlanetSaveData(Vector2 pos, Vector2 prevLine, Vector2 curLine, Vector2 nextLine, int index, int metal, int crystal, int uranium, int spriteNumber, string name, int tRisk, bool altPrevious, bool altCurrent, bool altNext)
    {
        planetPosition = pos;
        planetPreviousLine = prevLine;
        planetCurrentLine = curLine;
        planetNextLine = nextLine;

        alteredPrevious = altPrevious;
        alteredCurrent = altCurrent;
        alteredNext = altNext;

        planetRingIndex = index;

        planetMetal = metal;
        planetCrystal = crystal;
        planetUranium = uranium;

        planetSprite = spriteNumber;
        planetName = name;
        travelRisk = tRisk;
    }
}
