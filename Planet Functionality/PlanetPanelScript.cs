using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetPanelScript : MonoBehaviour
{
    public GameObject SpaceShip;
    public GameObject BackPanel;
    private PlanetTravel SpaceTravelScript;
    public Vector3 travelPosition;
    private SpaceShipResources resources;

    public GameObject CollectButton;

    public bool resourceTransfer = false;

    void Start()
    {
        SpaceTravelScript = SpaceShip.GetComponent<PlanetTravel>();
        resources = SpaceShip.GetComponent<SpaceShipResources>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            BackPanel.SetActive(true);
        }
    }

    public void TravelToPlanet()
    {
        if (!SpaceTravelCamera.dragging)
        {
            SpaceTravelScript.SetTargetPosition(PlanetManager.planetManager.currentPlanet.transform.position, PlanetManager.planetManager.currentPlanet.GetComponent<PlanetBootup>().ringIndex);
            FollowShip.switchFollowMode();
            this.gameObject.SetActive(false);
            BackPanel.SetActive(false);
        }
    }

    public void TransferResources()
    {
        Debug.Log("Collecting Resources");
        PlanetVariation pVar = PlanetManager.planetManager.currentPlanet.GetComponent<PlanetVariation>();
        //resources.shipMetal += pVar.metalResource;
        //pVar.metalResource = 0;
        //resources.shipCrystal += pVar.crystalResource;
        //pVar.crystalResource = 0;
        //resources.shipUranium += pVar.uraniumResource;
        //pVar.uraniumResource = 0;

        bool canTransfer = true;
        bool uraniumFull = false;
        bool crystalFull = false;
        bool metalFull = false;

        int i = 0;

        if (resources.currentCapacity < resources.maxCapacity)
        {
            while (canTransfer)
            {
                Debug.Log("Entered Loop");
                if (pVar.uraniumResource > 0 && !uraniumFull) //if there is uranium left on planet and space left on ship for uranium
                {
                    if (resources.currentCapacity + pVar.uraniumResource * resources.uraniumWeight <= resources.maxCapacity) //if all leftover uranium still fits on ship
                    {
                        resources.shipUranium += pVar.uraniumResource;
                        resources.currentCapacity += resources.uraniumWeight * pVar.uraniumResource;
                        pVar.uraniumResource = 0;                       
                        Debug.Log("Added Uranium " + resources.currentCapacity);
                        uraniumFull = true; //all uranium collected
                    }
                    else //else calculate how much uranium could still fit on the ship
                    {
                        int uraniumThatCouldFit = (resources.maxCapacity - (int)resources.currentCapacity) / resources.uraniumWeight;
                        pVar.uraniumResource -= uraniumThatCouldFit;
                        resources.shipUranium += uraniumThatCouldFit;
                        resources.currentCapacity += resources.uraniumWeight * uraniumThatCouldFit;
                        uraniumFull = true; //otherwise there is no space left for uranium
                    }
                }
                if (pVar.crystalResource > 0 && !crystalFull) //if there is crystal left on planet and space left on ship
                {
                    if (resources.currentCapacity + pVar.crystalResource * resources.crystalWeight <= resources.maxCapacity) //if all leftover uranium still fits on ship
                    {
                        resources.shipCrystal += pVar.crystalResource;
                        resources.currentCapacity += resources.crystalWeight * pVar.crystalResource;
                        pVar.crystalResource = 0;
                        Debug.Log("Added Crystal" + resources.currentCapacity);
                        crystalFull = true; //all uranium collected
                    }
                    else //else calculate how much uranium could still fit on the ship
                    {
                        int crystalThatCouldFit = (resources.maxCapacity - (int)resources.currentCapacity) / resources.crystalWeight;
                        pVar.crystalResource -= crystalThatCouldFit;
                        resources.shipCrystal += crystalThatCouldFit;
                        resources.currentCapacity += resources.crystalWeight * crystalThatCouldFit;
                        crystalFull = true; //otherwise there is no space left for uranium
                    }
                }
                if (pVar.metalResource > 0 && !metalFull) //if there is metal left on planet and space left on ship
                {
                    if (resources.currentCapacity + pVar.metalResource * resources.metalWeight <= resources.maxCapacity) //if all leftover uranium still fits on ship
                    {
                        resources.shipMetal += pVar.metalResource;
                        resources.currentCapacity += resources.metalWeight * pVar.metalResource;
                        pVar.metalResource = 0;
                        Debug.Log("Added Metal" + resources.currentCapacity);
                        metalFull = true; //all uranium collected
                    }
                    else //else calculate how much uranium could still fit on the ship
                    {
                        int metalThatCouldFit = (resources.maxCapacity - (int)resources.currentCapacity) / resources.metalWeight;
                        pVar.metalResource -= metalThatCouldFit;
                        resources.shipMetal += metalThatCouldFit;
                        resources.currentCapacity += resources.metalWeight * metalThatCouldFit;
                        metalFull = true; //otherwise there is no space left for uranium
                    }
                }
                if ((metalFull && crystalFull && uraniumFull) || (pVar.metalResource == 0 && pVar.crystalResource == 0 && pVar.uraniumResource == 0) || resources.currentCapacity == resources.maxCapacity)
                {
                    canTransfer = false;
                }
                i++;
            }
        }
        Debug.LogWarning("finsihed in " + i + " iteration");
        resources.updateResources();
        SpaceUIManager.spaceUIManager.setPlanetInspectPanel();
    }

}
