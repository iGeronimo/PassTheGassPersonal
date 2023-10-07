using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarrySpaceShipCoordinates : MonoBehaviour
{
    [Header("Danger Level Damages")]
    public float level1Damage;
    public float level2Damage;
    public float level3Damage;
    public float level4Damage;

    [Header("Movement Info")]
    public float speed;

    [Header("Capacity")]
    public int maxCapacity;
    public float maxFuel;
    public float maxHealth;

    [Header("Impact on Travel")]
    public float FuelConsumeRate = 1;


    public bool isTutorial = false;

    public bool arrived = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += deliveryReady;
    }

    private void Start()
    {
        Debug.Log("start");


    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= deliveryReady;
    }

    private void deliveryReady(Scene s, LoadSceneMode l)
    {
        if (arrived)
        {
            Debug.Log("delivery moved to " + SceneManager.GetActiveScene().name);
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            deliverExpeditionLoot();
        }
    }


    IEnumerator pickUpDelivery()
    {
        yield return new WaitForSeconds(1);

    }

    public void deliverExpeditionLoot()
    {
        TutorialSpace.current.isTutorial = isTutorial;
        Destroy(gameObject);
    }

    public void LoadUpDelivery()
    {
        Debug.Log("load up delivery");
        DontDestroyOnLoad(this.gameObject);
        arrived = true;
    }
}
