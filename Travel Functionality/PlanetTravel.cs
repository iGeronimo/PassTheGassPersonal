using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlanetTravel : MonoBehaviour
{
    [Header("Movement Info")]
    public float currentSpeed;

    public Vector3 targetPosition;

    public float speedToLevel1;
    public float speedToLevel2;
    public float speedToLevel3;
    public float speedToLevel4;

    public int EngineLevel = 0;
    public int targetLevel = 0;

    public float snapToPlanetDistance = 0.01f;

    public Vector2 currentVelocity;

    [Header("Travel Time")]
    public bool timeStarted = false;
    public bool newArrival = true;
    private bool linesSet = false;
    private bool planetSet = false;
    public bool damageCheck = false;
    private bool transitionFollow = false;

    public float secondsLeft;

    [Header("Danger Level Damages")]
    public float level1Damage;
    public float level2Damage;
    public float level3Damage;
    public float level4Damage;

    public GameObject shipDamage;
    Gradient toRed = new Gradient();
    Gradient toNormal = new Gradient();

    public float damagePoint = 0;

    private bool startColorChange = false;
    private bool turnedRed = false;
    private bool turnedNormal = false;

    private TravelRestrictions travelRestriction;
    private SpaceShipResources shipResources;

    private void Awake()
    {
        targetPosition = this.transform.position;
    }

    private void Start()
    {
        travelRestriction = GetComponent<TravelRestrictions>();
        shipResources = GetComponent<SpaceShipResources>();
        setGradients();
    }

    void setGradients()
    {
        GradientColorKey[] colorKeyToRed = new GradientColorKey[2];
        colorKeyToRed[0].color = Color.white;
        colorKeyToRed[1].color = Color.red;
        colorKeyToRed[0].time = 0.0f;
        colorKeyToRed[1].time = 1.0f;

        GradientAlphaKey[] alphaKeyToRed = new GradientAlphaKey[2];
        alphaKeyToRed[0].alpha = 1.0f;
        alphaKeyToRed[1].alpha = 1.0f;
        alphaKeyToRed[0].time = 0.0f;
        alphaKeyToRed[1].time = 1.0f;

        toRed.SetKeys(colorKeyToRed, alphaKeyToRed);

        GradientColorKey[] colorKeyToNorm = new GradientColorKey[2];
        colorKeyToNorm[0].color = Color.red;
        colorKeyToNorm[1].color = Color.white;
        colorKeyToNorm[0].time = 0.0f;
        colorKeyToNorm[1].time = 1.0f;

        GradientAlphaKey[] alphaKeyToNorm = new GradientAlphaKey[2];
        alphaKeyToNorm[0].alpha = 1.0f;
        alphaKeyToNorm[1].alpha = 1.0f;
        alphaKeyToNorm[0].time = 0.0f;
        alphaKeyToNorm[1].time = 1.0f;

        toNormal.SetKeys(colorKeyToNorm, alphaKeyToNorm);
    }

    private void Update()
    {
        if (!linesSet)
        {
            linesReady();
        }
        if (!StateManager.shipNoFuel)
        {
            TravelTimeTicking();
        }
        onArrival();
        shipDamageColor();
    }
    private void FixedUpdate()
    {
        if (!StateManager.shipNoFuel)
        {
            Travel();
            TimeLeftInTravel();
        }
        activeFollow();
    }

    void activeFollow()
    {
        if (FollowShip.followShip)
        {
            if (transitionFollow)
            {
                MoveSpaceCamera.SetCameraPosition(this.transform.position);
                if (Vector2.Distance(Camera.main.transform.position, this.transform.position) < 0.01f)
                {
                    transitionFollow = false;
                }
            }
            else
            {
                Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
                MoveSpaceCamera.cameraPosition = Camera.main.transform.position;
            }
        }
        else
        {
            transitionFollow = true;
        }
    }

    private void TimeLeftInTravel()
    {
        if (this.transform.position != targetPosition)
        {
            if (timeStarted != true)
            {
                if (targetLevel < EngineLevel)
                {
                    secondsLeft = 0;
                }
                else
                {
                    secondsLeft = Vector2.Distance(this.transform.position, targetPosition) / Vector3.Distance(new Vector2(0, 0), Vector3.Normalize(targetPosition - this.transform.position) * currentSpeed);
                    secondsLeft /= 50;
                }
                timeStarted = true;
            }
        }
    }

    private void TravelTimeTicking()
    {
        if (timeStarted == true)
        {
            secondsLeft -= Time.deltaTime;
            if (secondsLeft < 0)
            {
                secondsLeft = 0;
                timeStarted = false;
            }
        }
    }

    private void Travel()
    {
        if (PlanetManager.planetManager.loadFinished == true)
        {
            if (this.transform.position != targetPosition)
            {
                if (targetLevel < EngineLevel)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, 0.09f);
                    currentVelocity = Vector3.Normalize(targetPosition - this.transform.position);
                }
                else
                {
                    this.transform.position += Vector3.Normalize(targetPosition - this.transform.position) * currentSpeed;
                    currentVelocity = Vector3.Normalize(targetPosition - this.transform.position);
                }


                if (Vector2.Distance(this.transform.position, targetPosition) < snapToPlanetDistance)
                {
                    this.transform.position = targetPosition;
                    newArrival = true;
                    planetSet = false;
                    damageCheck = false;
                }
            }
        }
    }

    private void linesReady()
    {
        bool allTrue = true;
        if (PlanetManager.planetManager.linesGenerated)
        {
            foreach (GameObject planet in PlanetManager.planetManager.planets)
            {
                if (planet != null)
                {
                    if (planet.GetComponent<DrawPlanetLines>().lineSet == true) { }
                    else
                    {
                        allTrue = false;
                    }
                }
            }
            if (allTrue)
            {
                linesSet = true;
            }
        }
    }

    private void onArrival()
    {
        if (this.transform.position == targetPosition)
        {
            if (newArrival)
            {
                if (PlanetManager.planetManager.planets != null && linesSet)
                {
                    travelRestriction.canTravel(transform.position);
                    newArrival = false;
                    FollowShip.setFollowMode(false);
                }
                if (PlanetManager.planetManager.planets != null && planetSet == false)
                {
                    foreach (GameObject planet in PlanetManager.planetManager.planets)
                    {
                        if (planet != null)
                        {
                            if (planet.transform.position == this.transform.position)
                            {
                                PlanetManager.planetManager.currentPlanet = planet;
                                planetSet = true;
                                MoveSpaceCamera.SetCameraPosition(planet.transform.position);
                                SpaceUIManager.spaceUIManager.setPlanetInspectPanel();
                                SpaceUIManager.spaceUIManager.PlanetInspectUI.SetActive(true);
                            }
                        }
                    }
                }
                if (damageCheck == false)
                {
                    takeDamage();
                    shipResources.setHealthBar();
                }

            }
        }

    }

    void shipDamageColor()
    {
        if (startColorChange)
        {
            if (!turnedRed)
            {
                damagePoint += Time.deltaTime;
                this.GetComponentInChildren<SpriteRenderer>().color = toRed.Evaluate(damagePoint);
                if (damagePoint > 1)
                {
                    this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                    turnedRed = true;
                    damagePoint = 0;
                }
                return;
            }
            if (!turnedNormal)
            {
                damagePoint += Time.deltaTime;
                this.GetComponentInChildren<SpriteRenderer>().color = toNormal.Evaluate(damagePoint);
                if (damagePoint > 1)
                {
                    this.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    turnedNormal = true;
                    damagePoint = 0;
                }
            }
            if (turnedRed && turnedNormal)
            {
                startColorChange = false;
                turnedRed = false;
                turnedNormal = false;
            }
        }
    }

    private void takeDamage()
    {
        if (PlanetManager.planetManager.planets != null)
        {
            foreach (GameObject planet in PlanetManager.planetManager.planets)
            {
                if (planet != null)
                {
                    if (planet.transform.position == this.transform.position)
                    {
                        int dangerLevel = planet.GetComponent<PlanetBootup>().ringIndex;
                        int riskPercentage = planet.GetComponent<PlanetScript>().TravelRisk;

                        Random.InitState((int)Time.time);
                        int roll = Random.Range(0, 101);

                        bool tookDamage = false;

                        if (dangerLevel == 0)
                        {
                            if (riskPercentage > roll)
                            {
                                shipResources.currentHealth -= level1Damage;
                                tookDamage = true;
                            }
                        }
                        else if (dangerLevel == 1)
                        {
                            if (riskPercentage > roll)
                            {
                                shipResources.currentHealth -= level2Damage;
                                tookDamage = true;
                            }
                        }
                        else if (dangerLevel == 2)
                        {
                            if (riskPercentage > roll)
                            {
                                shipResources.currentHealth -= level3Damage;
                                tookDamage = true;
                            }
                        }
                        else
                        {
                            if (riskPercentage > roll)
                            {
                                shipResources.currentHealth -= level4Damage;
                                tookDamage = true;
                            }
                        }
                        if (tookDamage)
                        {
                            startColorChange = true;
                            Instantiate(shipDamage, GetComponentInChildren<SpriteRenderer>().transform);
                        }
                        damageCheck = true;
                    }
                }
            }
        }
    }


    public void SetTargetPosition(Vector3 newTarget, int ringIndex)
    {
        targetPosition = newTarget;
        targetLevel = ringIndex;
        if (targetLevel == 0)
        {
            currentSpeed = speedToLevel1;
        }
        else if (targetLevel == 1)
        {
            currentSpeed = speedToLevel2;
        }
        else if (targetLevel == 2)
        {
            currentSpeed = speedToLevel3;
        }
        else if (targetLevel == 3)
        {
            currentSpeed = speedToLevel4;
        }
        //STMaintainData.Instance.targetPosition = targetPosition;
    }

    public void doubleSpeedAd()
    {
        Advertisements.Instance.ShowRewardedVideo(onDoubleSpeedComplete);
    }

    public void onDoubleSpeedComplete(bool complete)
    {
        if(complete)
        {
            currentSpeed *= 2;

            Firebase.Analytics.FirebaseAnalytics.LogEvent("watch_ad_to_double_speed_in_spacex_successful", "on_ad_button_pressed_success", "1");
        }
        Firebase.Analytics.FirebaseAnalytics.LogEvent("watch_ad_to_double_speed_in_spacex", "on_ad_button_pressed_success", "1");
    }
    
}

