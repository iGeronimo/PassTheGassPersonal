using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingIncome : MonoBehaviour
{

 




    [Header("Builing Materials")]
    public float MetalIncome;
    public float CrystalIncome;
    public float UraniumIncome;

    [Header("Material Capacity")]
    public float MetalCapacity;
    public float CrystalCapacity;
    public float UraniumCapacity;

    [Header("Fuels")]
    public float BlackFuel;
    public float BlueFuel;
    public float GreenFuel;

    [Header("Fuels Capacity")]
    public float BlackFuelCapacity;
    public float BlueFuelCapacity;
    public float GreenFuelCapacity;

    [Header("Currency")]
    public float kChing;
    public float SolarDollar;

    [Header("Currency Capacity")]
    public float KChingCapacity;
    public float SolarDollarCapacity;

    displayGroupGains gGains;





    

    public float metalIncome()
    {
        return MetalIncome;
    }

    public float crystalIncome()
    {
        return CrystalIncome;
    }

    public float uraniumIncome()
    {
        return UraniumIncome;
    }

    public float metalCapacity()
    {
        return MetalCapacity;
    }

    public float crystalCapacity()
    {
        return CrystalCapacity;
    }

    public float uraniumCapacity()
    {
        return UraniumCapacity;
    }

    public float blackFuelIncome()
    {
        return BlackFuel;
    }

    public float blueFuelIncome()
    {
        return BlueFuel;
    }

    public float greenFuelIncome()
    {
        return GreenFuel;
    }

    public float blackFuelCapacity()
    {
        return BlackFuelCapacity;
    }

    public float blueFuelCapacity()
    {
        return BlueFuelCapacity;
    }

    public float greenFuelCapacity()
    {
        return GreenFuelCapacity;
    }

    public float kChingIncome()
    {
        return kChing;
    }

    public float solarDollarIncome()
    {
        return SolarDollar;
    }

    public float kChingCapacity()
    {
        return KChingCapacity;
    }

    public float solarDollarCapacity()
    {
        return SolarDollarCapacity;
    }

    






}