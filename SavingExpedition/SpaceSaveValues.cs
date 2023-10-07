using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceSaveValues
{
    public bool saveAvailable;
    public int day;
    public int month;
    public int year;
    public int hour;
    public int minute;
    public int second;
    public int milisecond;

    public int healthRestores;
    public int fuelRestores;

    #region shipResources

    public float metalShipInventory;
    public float crystalShipInventory;
    public float uraniumShipInventory;

    public float shipHealth;
    public float shipFuel;
    public float shipCapacity;

    public Vector2 shipPosition;
    public Vector3 targetPosition;
    public int targetPositionLevel;
    public float shipSpeed;
    public float travelTime;

    #endregion

    #region planets

    public List<PlanetSaveData> planets = new List<PlanetSaveData>();

    #endregion

    public int saveCounter = 0;

    public bool tutorialOver = false;

}
