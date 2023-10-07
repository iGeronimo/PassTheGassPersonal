using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButtonBehaviour : MonoBehaviour
{
    public GameObject spaceSave;

    private PlanetTravel planetTravel;

    [SerializeField] GameObject ResourceDelivery;

    //Ad
    private void Awake()
    {
        Advertisements.Instance.Initialize();
    }

    private void Start()
    {
        planetTravel= GetComponent<PlanetTravel>();
    }
    void Update()
    {
        SetLostSpaceGame();
    }

    void checkForActive()
    {
        if(planetTravel.timeStarted)
        {
            SpaceUIManager.spaceUIManager.ReturnToStation.SetActive(false);
        }
        else
        {
            SpaceUIManager.spaceUIManager.ReturnToStation.SetActive(true);
        }
    }

    void SetLostSpaceGame()
    {
        if(StateManager.shipNoHp)
        {
            SpaceUIManager.spaceUIManager.TMPLostText.text = "Your ship has taken too much damage!\n" +
                "To make it back you will have to sell all your resources :(";
            SpaceUIManager.spaceUIManager.LostSpaceGame.SetActive(true);

        }
        else if(StateManager.shipNoFuel) 
        {
            SpaceUIManager.spaceUIManager.TMPLostText.text = "Your ship has ran out of fuel...\n" +
                "Hitchhiking back home will cost you all your resources :(";
            SpaceUIManager.spaceUIManager.LostSpaceGame.SetActive(true);
        }
        else
        {
            SpaceUIManager.spaceUIManager.LostSpaceGame.SetActive(false);
        }
    }

    public void lostButtonPress()
    {
        //Lose so all resources become 0
        SpaceShipResources ssr = GetComponent<SpaceShipResources>();
        ssr.shipMetal = 0;
        ssr.shipCrystal = 0;
        ssr.shipUranium = 0;

        ssr.currentHealth = 1;
        ssr.currentFuel = 1;

        StateManager.shipNoHp = false;
        StateManager.shipNoFuel = false;
        ReturnButtonPress(true);
    }

    public void ReturnButtonPress(bool lost = false)
    {
        if(lost)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("on_ship_destroyed", "died_return", "1");
        }
        else
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("on_ship_destroyed", "safe_return", "1");
        }
        GameObject rd = (GameObject)Instantiate(ResourceDelivery);
        DeliverResources delivery = rd.GetComponent<DeliverResources>();
        SpaceShipResources ssr = GetComponent<SpaceShipResources>();
        if (ssr.shipMetal > 0 && ssr.shipCrystal > 0 && ssr.shipUranium > 0)
        {
            delivery.LoadUpDelivery((int)ssr.shipMetal, (int)ssr.shipCrystal, (int)ssr.shipUranium);
        }
        PlanetManager.planetManager.planets = new GameObject[0];
        StateManager.saveAvailable = false;
        
        switchScene();
    }

    #region Ads

    /// <summary>
    /// Show rewarded video, assigned from inspector
    /// </summary>
    public void ShowRewardedVideo()
    {
        Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
    }

    private void CompleteMethod(bool completed)
    {
        if (completed)
        {
            SpaceShipResources ssr = GetComponent<SpaceShipResources>();
            if (StateManager.shipNoFuel && spaceSave.GetComponent<SpaceSave>().spaceSaveValues.fuelRestores < 2)
            {
                ssr.currentFuel = ssr.maxFuel;
                spaceSave.GetComponent<SpaceSave>().spaceSaveValues.fuelRestores += 1;
            }
            if (StateManager.shipNoHp && spaceSave.GetComponent<SpaceSave>().spaceSaveValues.healthRestores < 1)
            {
                ssr.currentHealth = ssr.maxHealth;
                spaceSave.GetComponent<SpaceSave>().spaceSaveValues.healthRestores += 1;
            }
            Firebase.Analytics.FirebaseAnalytics.LogEvent("watch_ad_to_revive_in_spacex_successful", "on_ad_button_pressed_success", "1");
        }
            Firebase.Analytics.FirebaseAnalytics.LogEvent("watch_ad_to_revive_in_spacex", "on_ad_button_pressed", "1");
    }
    #endregion

    //public void SaveByAdd()
    //{
    //    //Watch an add

    //    //After watched add
    //    SpaceShipResources ssr = GetComponent<SpaceShipResources>();
    //    if(StateManager.shipNoFuel && spaceSave.GetComponent<SpaceSave>().spaceSaveValues.fuelRestores < 2)
    //    {
    //        ssr.currentFuel = ssr.maxFuel;
    //        spaceSave.GetComponent<SpaceSave>().spaceSaveValues.fuelRestores += 1;
    //    }
    //    if(StateManager.shipNoHp && spaceSave.GetComponent<SpaceSave>().spaceSaveValues.healthRestores < 1)
    //    {
    //        ssr.currentHealth = ssr.maxHealth;
    //        spaceSave.GetComponent<SpaceSave>().spaceSaveValues.healthRestores += 1;
    //    }
    //}

    public void switchScene()
    {
        spaceSave.GetComponent<SpaceSave>().Save();
        
    }


}
