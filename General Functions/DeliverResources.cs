using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliverResources : MonoBehaviour
{
    public int toDeliverMetal = 0;
    public int toDeliverCrystal = 0;
    public int toDeliverUranium = 0;

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
            StartCoroutine(pickUpDelivery());
        }
    }


    IEnumerator pickUpDelivery()
    {
        yield return new WaitForSeconds(3);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        deliverExpeditionLoot();
    }

    public void deliverExpeditionLoot()
    {
        SpaceStation station = (SpaceStation)BuildingManager.buildingManager.GetBuildingByType(BuildingManager.BuildingType.SPACESTATION);
        station.GetResourceDelivery(toDeliverMetal, toDeliverCrystal, toDeliverUranium);
        //ResourceManager.resourceManager.GetResourceDelivery(toDeliverMetal, toDeliverCrystal, toDeliverUranium);
        Destroy(gameObject);
    }

    public void LoadUpDelivery(int metal, int crystal, int uranium)
    {
        Debug.Log("load up delivery");
        toDeliverMetal = metal;
        toDeliverCrystal = crystal;
        toDeliverUranium = uranium;
        DontDestroyOnLoad(this.gameObject);
        arrived = true;
    }
}
