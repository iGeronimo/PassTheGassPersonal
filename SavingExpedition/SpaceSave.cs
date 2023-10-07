using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSave : MonoBehaviour
{
    const string fileName = "spaceSaveFile";

    const string mainFileName = "saveFile";
    string mainPath;
    SavedValues mainSavedValues;

    string path;

    public GameObject spaceShip;
    private SpaceShipResources shipResources;

    public SpaceSaveValues spaceSaveValues;

    public event Action<SpaceSaveValues> onLoadingSaveFile;

    public bool saveGameOnQuit = false;

    private void Awake()
    {

        path = Application.persistentDataPath + "/" + fileName;
        mainPath = Application.persistentDataPath + "/" + mainFileName;
        Debug.Log(path);

    }

    private void Start()
    {
        shipResources = spaceShip.GetComponent<SpaceShipResources>();
        Load();
    }


    #region MainFile progress Save
    public void SaveMainFile()
    {
        SaveManager.Instance.Load<SavedValues>(mainPath, LoadMainComplete, false);
        mainSavedValues.expeditionInProgress = spaceSaveValues.saveAvailable;
        SaveManager.Instance.Save(mainSavedValues, mainPath, MainSaveComplete, false);
    }

    private void LoadMainComplete(SavedValues data, SaveResult result, string message)
    {
        //Debug.Log("loading values");
        if (result == SaveResult.Success)
        {
            mainSavedValues = data;            
        }
        else
        {
            Debug.LogError("couldnt load values");
        }
    }

    private void MainSaveComplete(SaveResult result, string msg)
    {
        if (result == SaveResult.Error)
        {
            Debug.LogError("Save Error " + msg);
        }
    }

    #endregion

    public void Save()
    {
        saveGeneral();
        saveShip();
        savePlanets();
        if (PlanetManager.planetManager.planets.Length <= 0)
        {
            spaceSaveValues = new SpaceSaveValues();
            spaceSaveValues.saveAvailable = false;
        }
        else
        {
            spaceSaveValues.saveAvailable = true;
        }
        SaveMainFile();
        SaveManager.Instance.Save(spaceSaveValues, path, SaveComplete, false);
    }

    public void saveGeneral()
    {
        spaceSaveValues.day = DateTime.Now.Day;
        spaceSaveValues.month = DateTime.Now.Month;
        spaceSaveValues.year = DateTime.Now.Year;
        spaceSaveValues.hour = DateTime.Now.Hour;
        spaceSaveValues.minute = DateTime.Now.Minute;
        spaceSaveValues.second = DateTime.Now.Second;
        spaceSaveValues.milisecond = DateTime.Now.Millisecond;
        Debug.Log(new DateTime(spaceSaveValues.year, spaceSaveValues.month, spaceSaveValues.day, spaceSaveValues.hour, spaceSaveValues.minute, spaceSaveValues.second, spaceSaveValues.milisecond));
    }

    public void saveShip()
    {
        spaceSaveValues.metalShipInventory = shipResources.shipMetal;
        spaceSaveValues.crystalShipInventory = shipResources.shipCrystal;
        spaceSaveValues.uraniumShipInventory = shipResources.shipUranium;

        spaceSaveValues.shipHealth = shipResources.currentHealth;
        spaceSaveValues.shipFuel = shipResources.currentFuel;
        spaceSaveValues.shipCapacity = shipResources.currentCapacity;

        spaceSaveValues.shipPosition = (Vector2)spaceShip.transform.position;
        spaceSaveValues.targetPosition = spaceShip.GetComponent<PlanetTravel>().targetPosition;
        if (PlanetManager.planetManager.currentPlanet != null)
        {
            spaceSaveValues.targetPositionLevel = PlanetManager.planetManager.currentPlanet.GetComponent<PlanetBootup>().ringIndex;
        }

        spaceSaveValues.shipSpeed = spaceShip.GetComponent<PlanetTravel>().currentSpeed;
        spaceSaveValues.travelTime = spaceShip.GetComponent<PlanetTravel>().secondsLeft;
    }

    public void savePlanets()
    {

        if (PlanetManager.planetManager.planets.Length != 0)
        {
            spaceSaveValues.planets.Clear();
            foreach (GameObject planet in PlanetManager.planetManager.planets)
            {
                if (planet != null)
                {
                    PlanetSaveData newPlanetSave = new PlanetSaveData();
                    newPlanetSave.planetMetal = planet.GetComponent<PlanetVariation>().metalResource;
                    newPlanetSave.planetCrystal = planet.GetComponent<PlanetVariation>().crystalResource;
                    newPlanetSave.planetUranium = planet.GetComponent<PlanetVariation>().uraniumResource;

                    newPlanetSave.planetPosition = (Vector2)planet.transform.position;
                    newPlanetSave.planetCurrentLine = planet.GetComponent<PlanetBootup>().currentEndingLinePosition;
                    newPlanetSave.planetNextLine = planet.GetComponent<PlanetBootup>().nextEndingLinePosition;
                    newPlanetSave.planetPreviousLine = planet.GetComponent<PlanetBootup>().previousEndingLinePosition;

                    newPlanetSave.alteredCurrent = planet.GetComponent<PlanetBootup>().alteredCurrent;
                    newPlanetSave.alteredNext = planet.GetComponent<PlanetBootup>().alteredNext;
                    newPlanetSave.alteredPrevious = planet.GetComponent<PlanetBootup>().alteredPrevious;

                    newPlanetSave.travelRisk = planet.GetComponent<PlanetScript>().TravelRisk;

                    newPlanetSave.planetName = planet.name;

                    newPlanetSave.planetRingIndex = planet.GetComponent<PlanetBootup>().ringIndex;

                    newPlanetSave.planetSprite = planet.GetComponent<PlanetVariation>().spriteNumber;

                    spaceSaveValues.planets.Add(newPlanetSave);
                }
            }
        }
        
       
    }

    private void SaveComplete(SaveResult result, string msg)
    {
        if (result == SaveResult.Error)
        {
            Debug.LogError("Save Error: " + msg);
        }
    }

    public void Load()
    {
        SaveManager.Instance.Load<SpaceSaveValues>(path, LoadComplete, false);
    }

    private void LoadComplete(SpaceSaveValues data, SaveResult result, string message)
    {
        Debug.Log("loading space Values");
        if (result == SaveResult.Success)
        {
            Debug.Log("Load Succesful");
            spaceSaveValues = data;

        }
        if (result == SaveResult.Error || result == SaveResult.EmptyData)
        {
            Debug.Log(message);
            spaceSaveValues = new SpaceSaveValues();
        }

        if(spaceSaveValues.day != DateTime.Now.Day)
        {
            spaceSaveValues.fuelRestores = 0;
            spaceSaveValues.healthRestores = 0;
        }
        StateManager.saveAvailable = spaceSaveValues.saveAvailable;
        PlanetManager.planetManager.spaceSaveValues = spaceSaveValues;
        PlanetManager.planetManager.loadFinished = true;
        //universeGenScript.startUniverse(spaceSaveValues.saveAvailable, spaceSaveValues.planets);
        //spaceShip.GetComponent<SetSpaceShip>().setSpaceShip(spaceSaveValues.saveAvailable, spaceSaveValues);
    }

    private void OnApplicationQuit()
    {
        if (saveGameOnQuit)
        {
            Save();
        }
        else
        {
            spaceSaveValues = new SpaceSaveValues();
            SaveManager.Instance.Save(spaceSaveValues, path, SaveComplete, false);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (saveGameOnQuit && !focus)
        {
            Save();
        }
    }
}
