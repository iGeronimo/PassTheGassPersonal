using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Transactions;

public class SpaceShipResources : MonoBehaviour
{
    [Header("Current Resources")]
    public float shipMetal;
    public float shipCrystal;
    public float shipUranium;

    private float _shipMetal;
    private float _shipCrystal;
    private float _shipUranium;

    public float currentFuel;
    private float _currentFuel;

    public float currentHealth;
    private float _currentHealth;

    public float currentCapacity;
    private float _currentCapacity;

    [Header("Resource Weight")]
    public int metalWeight = 1;
    public int crystalWeight = 2;
    public int uraniumWeight = 3;

    [Header("Capacity")]
    public int maxCapacity;
    public float maxFuel;
    public float maxHealth;

    [Header("Impact on Travel")]
    public float FuelConsumeRate = 1;

    [Header("UI references")]
    private GameObject spaceShip;
    private PlanetTravel ssTravelScript;

    Gradient barFilling = new Gradient();

    bool newStart = false;

    private bool initialize = false;

    private float second;

    private void Awake()
    {

    }

    void Start()
    {
        spaceShip = GameObject.FindGameObjectWithTag("SpaceShip");
        ssTravelScript = spaceShip.GetComponent<PlanetTravel>();


    }

    public void NewStart(bool b)
    {
        if (b) { }
        else
        {
            newLaunch();
        }
        setFilling();
        updateResources();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpaceUIManager.spaceUIManager.variablesSet)
        {
            if (!newStart)
            {
                Debug.Log("newStart");
                NewStart(StateManager.saveAvailable);
                newStart = true;
            }
        }
        if(newStart)
        {
            ConsumeFuel();
            setFuelBar();
            setHealthBar();
            setCapacityBar();
            ContainHP();
            updateResources();
            if (!initialize)
            {
                initialize = true;
            }
        }
        

    }

    void ContainHP()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            setHealthBar();
            StateManager.shipNoHp = true;
        }
        else
        {
            StateManager.shipNoHp = false;
        }
    }

    void newLaunch()
    {
        currentFuel = maxFuel;
        _currentFuel = currentFuel;
        currentHealth = maxHealth;
        _currentHealth = currentHealth;
        currentCapacity = 0;
        _currentCapacity = 0.01f;
    }

    void setFilling()
    {
        GradientColorKey[] colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.green;
        colorKey[1].time = 1.0f;

        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        barFilling.SetKeys(colorKey, alphaKey);
    }

    public void updateResources()
    {
        lerpResources();
        setMetal();
        setCrystal();
        setUranium();
    }

    public void ConsumeFuel()
    {
        if (ssTravelScript.timeStarted)
        {
            second += Time.deltaTime;
            if (second > FuelConsumeRate)
            {
                currentFuel -= 1;
                second = 0;
            }
            if (currentFuel <= 0)
            {
                currentFuel = 0;
                StateManager.shipNoFuel = true;
            }
            else
            {
                StateManager.shipNoFuel = false;
            }
        }
        else
        {
            second = 0;
        }
    }

    void setMetal()
    {
        SpaceUIManager.spaceUIManager.TMPShipMetalText.text = ((int)_shipMetal).ToString();
    }

    void setCrystal()
    {
        SpaceUIManager.spaceUIManager.TMPShipCrystalText.text = ((int)_shipCrystal).ToString();
    }

    void setUranium()
    {
        SpaceUIManager.spaceUIManager.TMPShipUraniumText.text = ((int)_shipUranium).ToString();
    }

    public void setHealthBar()
    {
        if (_currentHealth != currentHealth || !initialize)
        {
            if (_currentHealth > currentHealth - .1f)
            {
                _currentHealth = currentHealth;
            }
            _currentHealth = Mathf.Lerp(_currentHealth, currentHealth, Time.deltaTime);
            SpaceUIManager.spaceUIManager.ShipHealthSlider.value = (float)(_currentHealth / maxHealth);
            SpaceUIManager.spaceUIManager.ShipHealthFillerImage.color = barFilling.Evaluate(_currentHealth / maxHealth);
            SpaceUIManager.spaceUIManager.TMPShipHealthNumber.text = ((int)_currentHealth).ToString();
        }
    }

    void setFuelBar()
    {

        if (_currentFuel != currentFuel || !initialize)
        {
            if (_currentFuel > currentFuel - .1f)
            {
                _currentFuel = currentFuel;
            }
            _currentFuel = Mathf.Lerp(_currentFuel, currentFuel, Time.deltaTime);
            SpaceUIManager.spaceUIManager.ShipFuelSlider.value = (float)(_currentFuel / maxFuel);
            SpaceUIManager.spaceUIManager.ShipFuelFillerImage.color = barFilling.Evaluate(_currentFuel / maxFuel);
            SpaceUIManager.spaceUIManager.TMPShipFuelNumber.text = ((int)_currentFuel).ToString();
        }
    }

    void setCapacityBar()
    {
        if (_currentCapacity != currentCapacity || !initialize)
        {
            _currentCapacity = Mathf.Lerp(_currentCapacity, currentCapacity, Time.deltaTime);
            if (_currentCapacity > currentCapacity - .1f)
            {
                _currentCapacity = currentCapacity;
            }
            SpaceUIManager.spaceUIManager.ShipCapacitySlider.value = (float)(_currentCapacity / maxCapacity);
            SpaceUIManager.spaceUIManager.ShipCapacityFillerImage.color = barFilling.Evaluate(1 - (_currentCapacity / maxCapacity));
            SpaceUIManager.spaceUIManager.TMPShipCapacityNumber.text = (int)_currentCapacity + "/" + maxCapacity;

        }
    }

    void lerpResources()
    {
        if (_shipMetal != shipMetal)
        {
            _shipMetal = Mathf.Lerp(_shipMetal, shipMetal, Time.deltaTime);
            if (_shipMetal > shipMetal - 0.1f)
            {
                _shipMetal = shipMetal;
            }
        }

        if (_shipCrystal != shipCrystal)
        {
            _shipCrystal = Mathf.Lerp(_shipCrystal, shipCrystal, Time.deltaTime);
            if (_shipCrystal > shipCrystal - 0.1f)
            {
                _shipCrystal = shipCrystal;
            }
        }

        if (_shipUranium != shipUranium)
        {
            _shipUranium = Mathf.Lerp(_shipUranium, shipUranium, Time.deltaTime);
            if (_shipUranium > shipUranium - 0.1f)
            {
                _shipUranium = shipUranium;
            }
        }
    }


}
