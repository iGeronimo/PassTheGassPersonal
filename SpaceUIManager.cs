using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpaceUIManager : MonoBehaviour
{
    public static SpaceUIManager spaceUIManager { get; private set; }
    private void OnEnable()
    {
        if (spaceUIManager != null && spaceUIManager != this)
        {
            Destroy(this);
        }
        else
        {
            spaceUIManager = this;
        }
    }

    private GameObject spaceShip;
    private GameObject _lastCurrentPlanet;

    public bool isTraveling = false;
    public bool variablesSet = false;

    #region UI variables

    [Header("PlanetInspectUI")]
    #region PlanetInspect
    #region GameObjects

    public GameObject BackPanel;
    public GameObject PlanetInspectUI;
    public GameObject TravelButton;
    public GameObject TravelButtonText;
    public GameObject PlanetImage;
    public GameObject PlanetName;
    public GameObject CollectResources;
    public GameObject CollectResourcesText;
    public GameObject planetResources;
    public GameObject planetMetal;
    public GameObject planetMetalText;
    public GameObject planetCrystal;
    public GameObject planetCrystalText;
    public GameObject planetUranium;
    public GameObject planetUraniumText;
    public GameObject planetTravelTimeText;
    public GameObject planetDanger;
    public GameObject planetRisk;
    public GameObject planetDangerText;
    public GameObject planetRiskText;

    #endregion // GameObjects

    #region TMPText

    [System.NonSerialized] public TMP_Text TMPTravelButtonText;
    [System.NonSerialized] public TMP_Text TMPPlanetName;
    [System.NonSerialized] public TMP_Text TMPCollectResourcesText;
    [System.NonSerialized] public TMP_Text TMPPlanetMetalText;
    [System.NonSerialized] public TMP_Text TMPPlanetCrystalText;
    [System.NonSerialized] public TMP_Text TMPPlanetUraniumText;
    [System.NonSerialized] public TMP_Text TMPPlanetTravelTimeText;
    [System.NonSerialized] public TMP_Text TMPDangerLevelText;
    [System.NonSerialized] public TMP_Text TMPRiskLevelText;

    private TMP_Text _TMPTravelButtonText;
    private TMP_Text _TMPPlanetName;
    private TMP_Text _TMPCollectResourcesText;
    private TMP_Text _TMPPlanetMetalText;
    private TMP_Text _TMPPlanetCrystalText;
    private TMP_Text _TMPPlanetUraniumText;
    private TMP_Text _TMPPlanetTravelTimeText;
    private TMP_Text _TMPDangerLevelText;
    private TMP_Text _TMPRiskLevelText;

    #endregion // TMP text

    #region Sprites

    [System.NonSerialized] public Image planetImageSprite;

    private Image _planetImageSprite;

    #endregion // Images

    #endregion // PlanetInspect
    [Header("ShipVariables")]
    #region ShipVariables

    #region GameObjects
    public GameObject ShipResourcesBackground;
    public GameObject ShipMetal;
    public GameObject ShipMetalText;
    public GameObject ShipCrystal;
    public GameObject ShipCrystalText;
    public GameObject ShipUranium;
    public GameObject ShipUraniumText;
    public GameObject ShipHealth;
    public GameObject ShipFuel;
    public GameObject ShipCapacity;
    public GameObject ShipHealthFiller;
    public GameObject ShipFuelFiller;
    public GameObject ShipCapacityFiller;

    #endregion // GameObjects

    #region TMPText

    [System.NonSerialized] public TMP_Text TMPShipMetalText;
    [System.NonSerialized] public TMP_Text TMPShipCrystalText;
    [System.NonSerialized] public TMP_Text TMPShipUraniumText;
    [System.NonSerialized] public TMP_Text TMPShipHealthNumber;
    [System.NonSerialized] public TMP_Text TMPShipFuelNumber;
    [System.NonSerialized] public TMP_Text TMPShipCapacityNumber;

    private TMP_Text _TMPShipMetalText;
    private TMP_Text _TMPShipCrystalText;
    private TMP_Text _TMPShipUraniumText;
    private TMP_Text _TMPShipHealthNumber;
    private TMP_Text _TMPShipFuelNumber;
    private TMP_Text _TMPShipCapacityNumber;

    #endregion // TMPText

    #region Sliders

    [System.NonSerialized] public Slider ShipHealthSlider;
    [System.NonSerialized] public Slider ShipFuelSlider;
    [System.NonSerialized] public Slider ShipCapacitySlider;

    private Slider _ShipHealthSlider;
    private Slider _ShipFuelSlider;
    private Slider _ShipCapacitySlider;

    #endregion // Sliders

    #region Filler
    [System.NonSerialized] public Image ShipHealthFillerImage;
    [System.NonSerialized] public Image ShipFuelFillerImage;
    [System.NonSerialized] public Image ShipCapacityFillerImage;

    private Image _ShipHealthFillerImage;
    private Image _ShipFuelFillerImage;
    private Image _ShipCapacityFillerImage;
    #endregion

    #endregion // ShipVariables

    [Header("MiscVariables")]
    #region MiscVariables

    #region GameObjects

    public GameObject TimePanel;
    public GameObject TravelTime;
    public GameObject FollowShip;
    public GameObject FollowText;
    public GameObject ReturnToStation;
    public GameObject LostSpaceGame;
    public GameObject LostText;
    public GameObject AdButtonGO;
    public GameObject switchScene;

    #endregion // Gameobjects

    #region TMPText

    [System.NonSerialized] public TMP_Text TMPTravelTimeText;
    [System.NonSerialized] public TMP_Text TMPFollowText;
    [System.NonSerialized] public TMP_Text TMPLostText;

    private TMP_Text _TMPTravelTimeText;
    private TMP_Text _TMPFollowText;
    private TMP_Text _TMPLostText;

    #endregion // TMPText

    #region buttons

    [System.NonSerialized] public Button AdButton;
    private Button _AdButton;

    #endregion

    #endregion // MiscVariables

    #endregion // UI variables

    private void Start()
    {
        spaceShip = GameObject.FindGameObjectWithTag("SpaceShip");
        setVariables();
    }

    void setVariables()
    {
        //Planet inspect UI
        _TMPTravelButtonText = TravelButtonText.GetComponent<TMP_Text>();
        _TMPPlanetName = PlanetName.GetComponent<TMP_Text>();
        _TMPCollectResourcesText = CollectResourcesText.GetComponent<TMP_Text>();
        _TMPPlanetMetalText = planetMetalText.GetComponent<TMP_Text>();
        _TMPPlanetCrystalText = planetCrystalText.GetComponent<TMP_Text>();
        _TMPPlanetUraniumText = planetUraniumText.GetComponent<TMP_Text>();
        _TMPPlanetTravelTimeText = planetTravelTimeText.GetComponent<TMP_Text>();
        _TMPDangerLevelText = planetDangerText.GetComponent<TMP_Text>();
        _TMPRiskLevelText = planetRiskText.GetComponent<TMP_Text>();

        TMPTravelButtonText = _TMPTravelButtonText;
        TMPPlanetName = _TMPPlanetName;
        TMPCollectResourcesText = _TMPCollectResourcesText;
        TMPPlanetMetalText = _TMPPlanetMetalText;
        TMPPlanetCrystalText = _TMPPlanetCrystalText;
        TMPPlanetUraniumText = _TMPPlanetUraniumText;
        TMPPlanetTravelTimeText = _TMPPlanetTravelTimeText;
        TMPDangerLevelText = _TMPDangerLevelText;
        TMPRiskLevelText = _TMPRiskLevelText;

        _planetImageSprite = PlanetImage.GetComponent<Image>();
        planetImageSprite = _planetImageSprite;

        //Ship Variables
        _TMPShipMetalText = ShipMetal.GetComponentInChildren<TMP_Text>();
        _TMPShipCrystalText = ShipCrystal.GetComponentInChildren<TMP_Text>();
        _TMPShipUraniumText = ShipUranium.GetComponentInChildren<TMP_Text>();

        TMPShipMetalText = _TMPShipMetalText;
        TMPShipCrystalText = _TMPShipCrystalText;
        TMPShipUraniumText = _TMPShipUraniumText;

        _TMPShipHealthNumber = ShipHealth.GetComponentInChildren<TMP_Text>();
        _TMPShipFuelNumber = ShipFuel.GetComponentInChildren<TMP_Text>();
        _TMPShipCapacityNumber = ShipCapacity.GetComponentInChildren<TMP_Text>();

        TMPShipHealthNumber = _TMPShipHealthNumber;
        TMPShipFuelNumber = _TMPShipFuelNumber;
        TMPShipCapacityNumber = _TMPShipCapacityNumber;

        _ShipHealthSlider = ShipHealth.GetComponentInChildren<Slider>();
        _ShipFuelSlider = ShipFuel.GetComponentInChildren<Slider>();
        _ShipCapacitySlider = ShipCapacity.GetComponentInChildren<Slider>();

        ShipHealthSlider = _ShipHealthSlider;
        ShipFuelSlider = _ShipFuelSlider;
        ShipCapacitySlider = _ShipCapacitySlider;

        _ShipHealthFillerImage = ShipHealthFiller.GetComponent<Image>();
        _ShipFuelFillerImage = ShipFuelFiller.GetComponent<Image>();
        _ShipCapacityFillerImage = ShipCapacityFiller.GetComponent<Image>();

        ShipHealthFillerImage = _ShipHealthFillerImage;
        ShipFuelFillerImage = _ShipFuelFillerImage;
        ShipCapacityFillerImage = _ShipCapacityFillerImage;

        //Misc Variables
        _TMPTravelTimeText = TravelTime.GetComponentInChildren<TMP_Text>();
        _TMPFollowText = FollowText.GetComponent<TMP_Text>();
        _TMPLostText = LostText.GetComponent<TMP_Text>();

        TMPTravelTimeText = _TMPTravelTimeText;
        TMPFollowText = _TMPFollowText;
        TMPLostText = _TMPLostText;

        _AdButton = AdButtonGO.GetComponent<Button>();
        AdButton = _AdButton;

        variablesSet = true;
    }

    private void Update()
    {
        if (PlanetInspectUI.activeSelf)
        {
            BackPanel.SetActive(true);
        }
        travelTime();
        updatePlanetPanel();
        shipAtPlanet();
        onCurrentPlanetChange();
        updateTraveling();
        switchButton();
        adButton();
    }

    void updateTraveling()
    {
        if (spaceShip.transform.position != spaceShip.GetComponent<PlanetTravel>().targetPosition && spaceShip.transform.position != Vector3.zero)
        {
            isTraveling = true;
        }
        else
        {
            isTraveling = false;
        }
    }

    bool checkPlanetPanelChange()
    {
        //Check if any variables changed
        if
        (_TMPTravelButtonText != TMPTravelButtonText ||
        _TMPPlanetName != TMPPlanetName ||
        _TMPCollectResourcesText != TMPCollectResourcesText ||
        _TMPPlanetMetalText != TMPPlanetMetalText ||
        _TMPPlanetCrystalText != TMPPlanetCrystalText ||
        _TMPPlanetUraniumText != TMPPlanetUraniumText ||
        _TMPPlanetTravelTimeText != TMPPlanetTravelTimeText ||
        _TMPDangerLevelText != TMPDangerLevelText ||
        _TMPRiskLevelText != TMPRiskLevelText ||

        _planetImageSprite != planetImageSprite ||

        _TMPShipMetalText != TMPShipMetalText ||
        _TMPShipCrystalText != TMPShipCrystalText ||
        _TMPShipUraniumText != TMPShipUraniumText ||
        _TMPShipHealthNumber != TMPShipHealthNumber ||
        _TMPShipFuelNumber != TMPShipFuelNumber ||
        _TMPShipCapacityNumber != TMPShipCapacityNumber ||

        _ShipHealthSlider != ShipHealthSlider ||
        _ShipFuelSlider != ShipFuelSlider ||
        _ShipCapacitySlider != ShipCapacitySlider ||

        _ShipHealthFillerImage != ShipHealthFillerImage ||
        _ShipFuelFillerImage != ShipFuelFillerImage ||
        _ShipCapacityFillerImage != ShipCapacityFillerImage ||

        _TMPTravelTimeText != TMPTravelTimeText ||
        _TMPFollowText != TMPFollowText ||
        _TMPLostText != TMPLostText ||

        _AdButton != AdButton)
        {
            return true;
        }

        return false;
    }

    void updatePlanetPanel()
    {
        if (!checkPlanetPanelChange())
        {
            return;
        }
        setPlanetPanel();
    }

    void setPlanetPanel()
    {
        _TMPShipMetalText = TMPShipMetalText;
        _TMPShipCrystalText = TMPShipCrystalText;
        _TMPShipUraniumText = TMPShipUraniumText;

        _TMPShipHealthNumber = TMPShipHealthNumber;
        _TMPShipFuelNumber = TMPShipFuelNumber;
        _TMPShipCapacityNumber = TMPShipCapacityNumber;

        _ShipHealthSlider = ShipHealthSlider;
        _ShipFuelSlider = ShipFuelSlider;
        _ShipCapacitySlider = ShipCapacitySlider;

        _TMPTravelTimeText = TMPTravelTimeText;
        _TMPFollowText = TMPFollowText;
        _TMPLostText = TMPLostText;

        _AdButton = AdButton;
    }

    void onCurrentPlanetChange()
    {
        if (_lastCurrentPlanet != PlanetManager.planetManager.currentPlanet && _lastCurrentPlanet != null)
        {
            _lastCurrentPlanet = PlanetManager.planetManager.currentPlanet;
            setPlanetInspectPanel();
        }
    }

    private float expectedFuelCost()
    {
        float travelTime;
        float speed = 0;
        if (PlanetManager.planetManager.currentPlanet.GetComponent<PlanetBootup>().ringIndex == 0)
        {
            speed = spaceShip.GetComponent<PlanetTravel>().speedToLevel1;
        }
        if (PlanetManager.planetManager.currentPlanet.GetComponent<PlanetBootup>().ringIndex == 1)
        {
            speed = spaceShip.GetComponent<PlanetTravel>().speedToLevel2;
        }
        if (PlanetManager.planetManager.currentPlanet.GetComponent<PlanetBootup>().ringIndex == 2)
        {
            speed = spaceShip.GetComponent<PlanetTravel>().speedToLevel3;
        }
        if (PlanetManager.planetManager.currentPlanet.GetComponent<PlanetBootup>().ringIndex == 3)
        {
            speed = spaceShip.GetComponent<PlanetTravel>().speedToLevel4;
        }
        travelTime = Vector2.Distance(spaceShip.transform.position, PlanetManager.planetManager.currentPlanet.transform.position) / Vector3.Distance(new Vector2(0, 0), Vector3.Normalize(PlanetManager.planetManager.currentPlanet.transform.position - spaceShip.transform.position) * speed);
        float fuelCost = (travelTime / 50) / spaceShip.GetComponent<SpaceShipResources>().FuelConsumeRate;
        return (int)(fuelCost);
    }

    void shipAtPlanet()
    {
        if (PlanetManager.planetManager.currentPlanet != null)
        {
            if (PlanetManager.planetManager.currentPlanet.transform.position == spaceShip.transform.position)
            {
                CollectResources.SetActive(true);
                planetDanger.SetActive(false);
                planetRisk.SetActive(false);
            }
            else
            {
                CollectResources.SetActive(false);
                planetDanger.SetActive(true);
                planetRisk.SetActive(true);
            }
        }
    }

    public void setPlanetInspectPanel()
    {
        _planetImageSprite.sprite = PlanetManager.planetManager.currentPlanet.GetComponent<SpriteRenderer>().sprite;

        _TMPPlanetName.text = PlanetManager.planetManager.currentPlanet.name;

        _TMPPlanetMetalText.text = PlanetManager.planetManager.currentPlanet.GetComponent<PlanetVariation>().metalResource.ToString();
        _TMPPlanetCrystalText.text = PlanetManager.planetManager.currentPlanet.GetComponent<PlanetVariation>().crystalResource.ToString();
        _TMPPlanetUraniumText.text = PlanetManager.planetManager.currentPlanet.GetComponent<PlanetVariation>().uraniumResource.ToString();

        _TMPRiskLevelText.text = PlanetManager.planetManager.currentPlanet.GetComponent<PlanetScript>().getRiskLevel();
        _TMPDangerLevelText.text = PlanetManager.planetManager.currentPlanet.GetComponent<PlanetScript>().getDangerLevel();


        if (PlanetManager.planetManager.currentPlanet.GetComponent<PlanetScript>().canTravel)
        {
            _TMPTravelButtonText.text = "Travel";
            TravelButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            _TMPTravelButtonText.text = "Not In Reach";
            TravelButton.GetComponent<Button>().interactable = false;
        }

        if (isTraveling)
        {
            _TMPTravelButtonText.text = "Wait for arrival";
            TravelButton.GetComponent<Button>().interactable = false;
        }


        if (PlanetManager.planetManager.currentPlanet.transform.position == spaceShip.transform.position)
        {
            _TMPPlanetTravelTimeText.text = "";
            TravelButton.SetActive(false);
        }
        else
        {
            _TMPPlanetTravelTimeText.text = "Expected Fuel Cost: " + ((int)expectedFuelCost()).ToString();
            TravelButton.SetActive(true);
        }

        PlanetInspectUI.SetActive(true);
    }

    private void switchButton()
    {
        if (PlanetInspectUI.activeSelf)
        {
            //switchScene.SetActive(false);
        }
        else
        {
            //switchScene.SetActive(true);
        }
    }

    private void travelTime()
    {
        if (spaceShip.GetComponent<PlanetTravel>().secondsLeft > 0)
        {
            TravelTime.SetActive(true);
        }
        else
        {
            TravelTime.SetActive(false);
        }
    }

    private void adButton()
    {
        if (variablesSet)
        {
            if (StateManager.shipNoFuel)
            {
                if (PlanetManager.planetManager.spaceSaveValues.fuelRestores > 1)
                {
                    AdButton.interactable = false;
                }
                else
                {
                    AdButton.interactable = true;
                }
            }
            if (StateManager.shipNoHp)
            {
                if (variablesSet)
                    if (PlanetManager.planetManager.spaceSaveValues.healthRestores > 0)
                    {
                        AdButton.interactable = false;
                    }
                    else
                    {
                        AdButton.interactable = true;
                    }
            }
        }

    }
}
