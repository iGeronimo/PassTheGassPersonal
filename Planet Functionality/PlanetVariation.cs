using UnityEngine;
using System;

public class PlanetVariation : MonoBehaviour
{
    SpriteRenderer planetSprite;
    Color planetColor;

    public Sprite[] planets;

    [Header("Base level recourses")]
    public Vector2 metalRange;
    public Vector2 crystalRange;
    public Vector2 uraniumRange;

    [Header("Distance multiplication factor")]
    public float distanceMagnitude;

    [Header("Danger Level Multiplier")]
    public float mediumMultiplier;
    public float highMultiplier;

    [Header("Planets current Resources")]
    public int metalResource;
    public int crystalResource;
    public int uraniumResource;

    private PlanetScript planetScript;

    public int spriteNumber;
    float scaleOffset;

    private void Awake()
    {
        planetScript = GetComponent<PlanetScript>();
    }

    void Start()
    {

        scaleOffset = UnityEngine.Random.Range(this.transform.localScale.x * -0.2f, this.transform.localScale.x * 0.2f);
        planetSprite = this.GetComponent<SpriteRenderer>();
        setVariation();
        setSprite();
    }

    public void setVariation()
    {
        Debug.Log("Load available: " + StateManager.saveAvailable);
        if (StateManager.saveAvailable) 
        {
            
        }
        else
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            spriteNumber = UnityEngine.Random.Range(0, 1000) % planets.Length;
            setNewName();
            setResources();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    string GenerateName()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var stringChars = new char[2];
        var nums = "1234567890";
        var numChars = new char[2];
        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
        }
        for (int i = 0; i < numChars.Length; i++)
        {
            numChars[i] = nums[UnityEngine.Random.Range(0, nums.Length)];
        }

        return new string(stringChars) + "-" + new string(numChars);
    }

    void setNewName()
    {
        bool nameExists = false;
        string potentialName = GenerateName();
        foreach (string name in UniversalLists.PlanetNames)
        {
            if (name == potentialName)
            {
                nameExists = true;
            }
        }
        if (!nameExists)
        {
            name = potentialName;
            Debug.Log("Set new name as " + name + " at " + DateTime.Now);
            UniversalLists.PlanetNames.Add(potentialName);
        }
        else
        {
            setNewName();
        }
    }

    void setSprite()
    {
        planetSprite.sprite = planets[spriteNumber];
        this.transform.localScale = new Vector3(this.transform.localScale.x - scaleOffset, this.transform.localScale.y - scaleOffset, this.transform.localScale.z - scaleOffset);
    }

    private void setResources()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        metalResource = (int)(UnityEngine.Random.Range(metalRange.x, metalRange.y) * (Vector2.Distance(this.transform.position, new(0, 0)) * distanceMagnitude));
        crystalResource = (int)(UnityEngine.Random.Range(crystalRange.x, crystalRange.y) * (Vector2.Distance(this.transform.position, new(0, 0)) * distanceMagnitude));
        uraniumResource = (int)(UnityEngine.Random.Range(uraniumRange.x, uraniumRange.y) * (Vector2.Distance(this.transform.position, new(0, 0)) * distanceMagnitude));
        if (planetScript.TravelRisk < 25)
        {

        }
        else if (planetScript.TravelRisk < 59)
        {
            metalResource = (int)(metalResource * mediumMultiplier);
            crystalResource = (int)(crystalResource * mediumMultiplier);
            uraniumResource = (int)(uraniumResource * mediumMultiplier);
        }
        else
        {
            metalResource = (int)(metalResource * highMultiplier);
            crystalResource = (int)(crystalResource * highMultiplier);
            uraniumResource = (int)(uraniumResource * highMultiplier);
        }
    }


}
