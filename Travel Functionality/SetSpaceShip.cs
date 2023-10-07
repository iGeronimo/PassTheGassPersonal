using System;
using TMPro;
using UnityEngine;

public class SetSpaceShip : MonoBehaviour
{
    PlanetTravel pTravel;
    SpaceShipResources shipResources;

    private void Awake()
    {

    }
    void Start()
    {
        pTravel = GetComponent<PlanetTravel>();
        shipResources = GetComponent<SpaceShipResources>();
    }



    public void setSpaceShip(bool sAvailable, SpaceSaveValues sSave)
    {
        if (sAvailable)
        {
            shipResources.shipMetal = sSave.metalShipInventory;
            shipResources.shipCrystal = sSave.crystalShipInventory;
            shipResources.shipUranium = sSave.uraniumShipInventory;

            shipResources.currentHealth = sSave.shipHealth;
            shipResources.currentFuel = sSave.shipFuel;
            shipResources.currentCapacity = sSave.shipCapacity;

            this.transform.position = sSave.shipPosition;
            pTravel.targetPosition = sSave.targetPosition;
            pTravel.targetLevel = sSave.targetPositionLevel;
            pTravel.currentSpeed = sSave.shipSpeed;

            foreach(GameObject planet in PlanetManager.planetManager.planets)
            {
                if(planet.transform.position == pTravel.targetPosition)
                {
                    PlanetManager.planetManager.currentPlanet = planet;
                }
            }

            if(this.transform.position != pTravel.targetPosition)
            {
                DateTime lastTime = new DateTime(sSave.year, sSave.month, sSave.day, sSave.hour, sSave.minute, sSave.second, sSave.milisecond);
                Debug.Log(DateTime.Now.Subtract(lastTime).TotalSeconds);

                float secondsPassed = (float)DateTime.Now.Subtract(lastTime).TotalSeconds;

                float secondsLeft = sSave.travelTime;
                if (secondsPassed >= secondsLeft)
                {
                    float fuelLevel = shipResources.currentFuel;
                    fuelLevel -= (secondsLeft / shipResources.FuelConsumeRate);
                    if (fuelLevel < 0)
                    {
                        float outOfFuelAt = fuelLevel * shipResources.FuelConsumeRate;
                        secondsPassed += outOfFuelAt;
                        shipResources.currentFuel = 0;

                        secondsLeft -= (float)secondsPassed;
                        pTravel.secondsLeft = secondsLeft;
                        pTravel.timeStarted = true;
                        this.transform.position += Vector3.Normalize(pTravel.targetPosition - this.transform.position) * (pTravel.currentSpeed * (float)secondsPassed * 50);
                    }
                    else
                    {
                        pTravel.secondsLeft = 0;
                        shipResources.currentFuel = fuelLevel;
                        this.transform.position = pTravel.targetPosition;
                    }

                }
                if(secondsPassed < secondsLeft)
                {
                    float fuelLevel = shipResources.currentFuel;
                    fuelLevel -= ((float)secondsPassed / shipResources.FuelConsumeRate);
                    if(fuelLevel < 0)
                    {
                        float outOfFuelAt = fuelLevel * shipResources.FuelConsumeRate;
                        secondsPassed += outOfFuelAt;
                        shipResources.currentFuel = 0;
                    }
                    else
                    {
                        shipResources.currentFuel = fuelLevel;
                    }

                    secondsLeft -= (float)secondsPassed;
                    pTravel.secondsLeft = secondsLeft;
                    pTravel.timeStarted = true;
                    this.transform.position += Vector3.Normalize(pTravel.targetPosition - this.transform.position) * (pTravel.currentSpeed * (float)secondsPassed * 50);
                }
            }
            else
            {
                pTravel.damageCheck = true;
            }
        }
    }

}
