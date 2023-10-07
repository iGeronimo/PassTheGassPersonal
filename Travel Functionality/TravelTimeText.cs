using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TravelTimeText : MonoBehaviour
{
    public GameObject spaceShip;
    private PlanetTravel PTscript;

    private Color alphaOff;
    private Color alphaOn;

    private Image panelImage;

    void Start()
    {
        panelImage = GetComponentInParent<Image>();
        PTscript = spaceShip.GetComponent<PlanetTravel>();
        
        //setup alpha on/off
        alphaOff = panelImage.color;
        alphaOn = panelImage.color;
        alphaOff.a = 0;
        alphaOn.a = 1;

    }

    // Update is called once per frame
    void Update()
    {
        displayTime(PTscript.secondsLeft);
        if(PTscript.secondsLeft <= 0)
        {
            SpaceUIManager.spaceUIManager.TMPTravelTimeText.text = "";
            panelImage.color = alphaOff;
        }
    }

    void displayTime(float displayTime)
    {
        panelImage.color = alphaOn;
        displayTime += 1;
        float minutes = Mathf.FloorToInt(displayTime / 60);
        float seconds = Mathf.FloorToInt(displayTime % 60);
        SpaceUIManager.spaceUIManager.TMPTravelTimeText.text = string.Format("Travel Time Left: " + "{0:00}:{1:00}", minutes, seconds);
    }
}
