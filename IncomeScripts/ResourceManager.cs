using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager resourceManager { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (resourceManager != null && resourceManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            resourceManager = this;
        }
        fillDictionaries();
        fillIconDict();
        test t = GetComponent<test>();
        t.onLoadingSaveFile += applySavedResources;
    }

    

    [SerializeField] List<ResourceStyleUI> icons = new List<ResourceStyleUI>();

    private Dictionary<ResourceManager.ResourceType, ResourceStyleUI> IconDict = new Dictionary<ResourceManager.ResourceType, ResourceStyleUI>();

    public enum ResourceType { KCHING, SOLARDOLLAR, METAL, CRYSTAL, URANIUM, BLUEFUEL, GREENFUEL, BLACKFUEL}


    public static event Action onResourceUpdate;

    public event Action <int> buyRequestGranted;
    public event Action <int> buyRequestDenied;

    BuildingManager bm;

    List<IBuilding> Buildings = new List<IBuilding>();
    public float globalStepTime;

    [Header("Income")] //amount that is to be added
    public ResourceAmount metalIncome = new ResourceAmount(ResourceType.METAL, 0);
    public ResourceAmount crystalIncome = new ResourceAmount(ResourceType.CRYSTAL, 0);
    public ResourceAmount uraniumIncome = new ResourceAmount(ResourceType.URANIUM, 0);
    public ResourceAmount blackFuelIncome = new ResourceAmount(ResourceType.BLACKFUEL, 0);
    public ResourceAmount blueFuelIncome = new ResourceAmount(ResourceType.BLUEFUEL, 0);
    public ResourceAmount greenFuelIncome = new ResourceAmount(ResourceType.GREENFUEL, 0);
    public ResourceAmount kChingIncome = new ResourceAmount(ResourceType.KCHING, 0);
    public ResourceAmount solarDollarIncome = new ResourceAmount(ResourceType.SOLARDOLLAR, 0);

    [Header("Capacity")] //how much you can have of a resource at any point
    public ResourceAmount metalCapacity = new ResourceAmount(ResourceType.METAL, 0);
    public ResourceAmount crystalCapacity = new ResourceAmount(ResourceType.CRYSTAL, 0);
    public ResourceAmount uraniumCapacity = new ResourceAmount(ResourceType.URANIUM, 0);
    public ResourceAmount blackFuelCapacity = new ResourceAmount(ResourceType.BLACKFUEL, 0);
    public ResourceAmount blueFuelCapacity = new ResourceAmount(ResourceType.BLUEFUEL, 0);
    public ResourceAmount greenFuelCapacity = new ResourceAmount(ResourceType.GREENFUEL, 0);
    public ResourceAmount kChingCapacity = new ResourceAmount(ResourceType.KCHING, 0);
    public ResourceAmount solarDollarCapacity = new ResourceAmount(ResourceType.SOLARDOLLAR, 0);

    [Header ("Inventory")] //how much you have of a resource right now
    public ResourceAmount metalInventory = new ResourceAmount(ResourceType.METAL, 0);
    public ResourceAmount crystalInventory = new ResourceAmount(ResourceType.CRYSTAL, 0);
    public ResourceAmount uraniumInventory = new ResourceAmount(ResourceType.URANIUM, 0);
    public ResourceAmount blackFuelInventory = new ResourceAmount(ResourceType.BLACKFUEL, 0);
    public ResourceAmount blueFuelInventory = new ResourceAmount(ResourceType.BLUEFUEL, 0);
    public ResourceAmount greenFuelInventory = new ResourceAmount(ResourceType.GREENFUEL, 0);
    public ResourceAmount kChingInventory = new ResourceAmount(ResourceType.KCHING, 0);
    public ResourceAmount solarDollarInventory = new ResourceAmount(ResourceType.SOLARDOLLAR, 0);

    Dictionary<ResourceType, ResourceAmount> Income = new Dictionary<ResourceType, ResourceAmount>();
    Dictionary<ResourceType, ResourceAmount> Capacity = new Dictionary<ResourceType, ResourceAmount>();
    Dictionary<ResourceType, ResourceAmount> Inventory = new Dictionary<ResourceType, ResourceAmount>();

    public MileStones myMilestones;




    
    void fillDictionaries()
    {
        Income.Add(ResourceType.METAL, metalIncome);
        Income.Add(ResourceType.CRYSTAL, crystalIncome);
        Income.Add(ResourceType.URANIUM, uraniumIncome);
        Income.Add(ResourceType.BLACKFUEL, blackFuelIncome);
        Income.Add(ResourceType.BLUEFUEL, blueFuelIncome);
        Income.Add(ResourceType.GREENFUEL, greenFuelIncome);
        Income.Add(ResourceType.KCHING, kChingIncome);
        Income.Add(ResourceType.SOLARDOLLAR, solarDollarIncome);

        Capacity.Add(ResourceType.METAL, metalCapacity);
        Capacity.Add(ResourceType.CRYSTAL, crystalCapacity);
        Capacity.Add(ResourceType.URANIUM, uraniumCapacity);
        Capacity.Add(ResourceType.BLACKFUEL, blackFuelCapacity);
        Capacity.Add(ResourceType.BLUEFUEL, blueFuelCapacity);
        Capacity.Add(ResourceType.GREENFUEL, greenFuelCapacity);
        Capacity.Add(ResourceType.KCHING, kChingCapacity);
        Capacity.Add(ResourceType.SOLARDOLLAR, solarDollarCapacity);

        Inventory.Add(ResourceType.METAL, metalInventory);
        Inventory.Add(ResourceType.CRYSTAL, crystalInventory);
        Inventory.Add(ResourceType.URANIUM, uraniumInventory);
        Inventory.Add(ResourceType.BLACKFUEL, blackFuelInventory);
        Inventory.Add(ResourceType.BLUEFUEL, blueFuelInventory);
        Inventory.Add(ResourceType.GREENFUEL, greenFuelInventory);
        Inventory.Add(ResourceType.KCHING, kChingInventory);
        Inventory.Add(ResourceType.SOLARDOLLAR, solarDollarInventory);
    }


    private void Start()
    {
        //IBuilding.onIncomeReceived += setValues;





        bm = BuildingManager.buildingManager;
        Buildings = bm.GetBuildings();
        AlienBuildingIncome.onIncomeReceived += addIncome;
        Distributor.onIncomeReceived += addIncome;
        Generator.onIncomeReceived += addIncome;
        SpaceStation.onIncomeReceived += addIncome;
        GameManager.gameManager.onAwayGainResurces += addIncome;
        //IIncomeBuilding.onIncomeReceived += addIncome;
        keepCapacity();
        if (onResourceUpdate != null)
        {
            onResourceUpdate();
        }

        myMilestones = new MileStones();
        //CheckList();
    }

    private void OnDestroy()
    {
        AlienBuildingIncome.onIncomeReceived -= addIncome;
        Distributor.onIncomeReceived -= addIncome;
        Generator.onIncomeReceived -= addIncome;
        SpaceStation.onIncomeReceived -= addIncome;
        GameManager.gameManager.onAwayGainResurces -= addIncome;
    }


    private void keepCapacity()
    {
        if(metalInventory.amount > metalCapacity.amount)
        {
            metalInventory.amount = metalCapacity.amount;
        }
        if (crystalInventory.amount > crystalCapacity.amount)
        {
            crystalInventory.amount = crystalCapacity.amount;
        }
        if (uraniumInventory.amount > uraniumCapacity.amount)
        {
            uraniumInventory.amount = uraniumCapacity.amount;
        }
        if (blackFuelInventory.amount > blackFuelCapacity.amount)
        {
            blackFuelInventory.amount = blackFuelCapacity.amount;
        }
        if (blueFuelInventory.amount > blueFuelCapacity.amount)
        {
            blueFuelInventory.amount = blueFuelCapacity.amount;
        }
        if (greenFuelInventory.amount > greenFuelCapacity.amount)
        {
            greenFuelInventory.amount = greenFuelCapacity.amount;
        }
        if (kChingInventory.amount > kChingCapacity.amount)
        {
            kChingInventory.amount = kChingCapacity.amount;
        }
        if (solarDollarInventory.amount > solarDollarCapacity.amount)
        {
            solarDollarInventory.amount = solarDollarCapacity.amount;
        }
    }

    public void increaseCapacity(ResourceAmount ra)
    {
        Capacity[ra.type].amount = ra.amount;
    }

    private void fillIconDict()
    {
        foreach (ResourceStyleUI icon in icons)
        {
            IconDict.Add(icon.resource.type, icon);
        }
    }

    private void addIncome(ResourceAmount ra)
    {
        Income[ra.type].amount += ra.amount;
        Inventory[ra.type].amount += ra.amount;
        keepCapacity();
        onResourceUpdate();
        switch(ra.type)
        {
            case ResourceType.KCHING:
                GameManager.gameManager.data.incomeKChing += ra.amount;
                if(Income[ra.type].amount + ra.amount >= 50000)
                {
                    if (!myMilestones.reachedKChing50k)
                    {
                        Firebase.Analytics.FirebaseAnalytics.LogEvent("50k_ching_milestone", "milestones", "1");
                        myMilestones.reachedKChing50k = true;
                    }
                    if(Income[ra.type].amount + ra.amount >= 150000)
                    {
                        if (!myMilestones.reachedKChing150k)
                        {
                            Firebase.Analytics.FirebaseAnalytics.LogEvent("150k_ching_milestone", "milestones", "1");
                            myMilestones.reachedKChing150k = true;
                        }
                        if (!myMilestones.reachedKChing300k && Income[ra.type].amount + ra.amount >= 300000)
                        {
                            Firebase.Analytics.FirebaseAnalytics.LogEvent("300k_ching_milestone", "milestones", "1");
                            myMilestones.reachedKChing300k = true;
                        }
                    }
                }
                break;
            case ResourceType.METAL:
                GameManager.gameManager.data.metalIncome += ra.amount;
                if(!myMilestones.reachedMetal30k && Income[ra.type].amount + ra.amount >= 30000)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("30k_metal_milestone", "milestones", "1");
                    myMilestones.reachedMetal30k = true;
                }
                break;
            case ResourceType.CRYSTAL:
                GameManager.gameManager.data.crystalIncome += ra.amount;
                if (!myMilestones.reachedMetal30k && Income[ra.type].amount + ra.amount >= 20000)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("20k_crystal_milestone", "milestones", "1");
                    myMilestones.reachedCrystal20k = true;
                }
                break;
            case ResourceType.URANIUM:
                GameManager.gameManager.data.uraniumIncome += ra.amount;
                if (!myMilestones.reachedMetal30k && Income[ra.type].amount + ra.amount >= 10000)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("10k_uranium_milestone", "milestones", "1");
                    myMilestones.reachedUranium10k = true;
                }
                break;
        }

    }

    private void substractPayment(ResourceAmount ra)
    {
        Inventory[ra.type].amount -= ra.amount;
        onResourceUpdate();
    }

    public ResourceStyleUI getResourceStyle(ResourceType type)
    {
        return IconDict[type];
    }


    public bool handleBuyRequest(ResourceAmount ra)
    {
        if (isResourceAvailable(ra))
        {
            substractPayment(ra);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isResourceAvailable(ResourceAmount ra)
    {
        keepCapacity();
        if (Inventory[ra.type].amount >= ra.amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckList()
    {
        Buildings.Clear();
        Buildings = bm.GetBuildings();

    }
    
    public void updateBuildingList()
    {
        CheckList();
    }


    public void GetResourceDelivery(int metal, int crystal, int uranium)
    {
        Inventory[ResourceType.METAL].amount += metal;
        crystalInventory.amount += crystal;
        uraniumInventory.amount += uranium;
        keepCapacity();
        onResourceUpdate();
    }

    public MainUIResourceDisplay.capacityLevel isCapacityGettingLow(ResourceType type)
    {
        if(Inventory[type].amount >= Capacity[type].amount * 0.9f && Inventory[type].amount < Capacity[type].amount)
        {
            return MainUIResourceDisplay.capacityLevel.CLOSETOFULL;

        }
        else if(Inventory[type].amount >= Capacity[type].amount)
        {
            return MainUIResourceDisplay.capacityLevel.FULL;
        }
        else
        {
            return MainUIResourceDisplay.capacityLevel.NORMAL;
        }
    }

    private void applySavedResources(SavedValues values)
    {
        metalCapacity.amount = values.metalCapacity;
        crystalCapacity.amount = values.crystalCapacity;
        uraniumCapacity.amount = values.uraniumCapacity;
        blackFuelCapacity.amount = values.blackFuelCapacity;
        blueFuelCapacity.amount = values.blackFuelCapacity;
        greenFuelCapacity.amount = values.greenFuelCapacity;
        kChingCapacity.amount = values.kChingICapacity;
        solarDollarCapacity.amount = values.solarDollarCapacity;

        metalIncome.amount = values.metalIncome;
        crystalIncome.amount = values.crystalIncome;
        uraniumIncome.amount = values.uraniumIncome;
        blackFuelIncome.amount = values.blackFuelIncome;
        blueFuelIncome.amount = values.blueFuelIncome;
        greenFuelIncome.amount = values.greenFuelIncome;
        kChingIncome.amount = values.kChingIncome;
        solarDollarIncome.amount = values.solarDollarIncome;

        metalInventory.amount = values.metalInventory;
        crystalInventory.amount = values.crystalInventory;
        uraniumInventory.amount = values.uraniumInventory;
        blackFuelInventory.amount = values.blackfuelInventory;
        blueFuelInventory.amount = values.blueFuelInventory;
        greenFuelInventory.amount = values.greenFuelInventory;
        kChingInventory.amount = values.kChingInventory;
        solarDollarInventory.amount = values.solarDollarInventory;
    }
}


[System.Serializable]
public class ResourceAmount
{
    public ResourceManager.ResourceType type;
    public int amount;
    public float fAmount;

    public ResourceAmount(ResourceManager.ResourceType t, float am)
    {
        type = t;
        amount = (int)am;
        fAmount = am;
    }

    public float getFAmount()
    {
        return fAmount;
    }

    public void setFAmount(float amount)
    {
        this.amount = (int)amount;
        fAmount = amount;
    }

    public override string ToString()
    {
        return $"{type}: {amount}";
    }
}

[System.Serializable]
public class MileStones
{
    public bool reachedKChing50k;
    public bool reachedKChing150k;
    public bool reachedKChing300k;

    public bool reachedMetal30k;
    public bool reachedCrystal20k;
    public bool reachedUranium10k;

    public bool reachedFuel7500k;


    public MileStones()
    {
        reachedKChing50k = false;
        reachedKChing150k = false;
        reachedKChing300k = false;

        reachedMetal30k = false;
        reachedCrystal20k = false;
        reachedUranium10k = false;

        reachedFuel7500k = false;
    }
}