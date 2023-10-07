using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipAnimationBehaviour : MonoBehaviour
{
    PlanetTravel planetTravel;
    Transform shipSprite;

    private Vector2 velocity;

    public GameObject[] particleSystems;


    private bool startTravelCheck = false;



    void Start()
    {
        planetTravel = this.GetComponent<PlanetTravel>();
        shipSprite = this.GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        setShipDirection();
    }

    void setShipDirection()
    {
        if (planetTravel.timeStarted)
        {
            if (!startTravelCheck)
            {
                Vector2 currentPosition = (Vector2)this.transform.position;
                Vector2 targetPosition = planetTravel.targetPosition;
                Debug.Log(planetTravel.targetPosition);
                float angle = (((Mathf.Atan2(targetPosition.y - currentPosition.y, targetPosition.x - currentPosition.x) * 180 / Mathf.PI) + 540) % 360);
                Debug.Log(angle);
                shipSprite.localRotation = Quaternion.Euler(0, 0, angle);
                startTravelCheck = true;
                foreach (GameObject particle in particleSystems)
                {
                    particle.SetActive(true);
                }
            }
        }
        else
        {
            foreach(GameObject particle in particleSystems)
            {
                particle.SetActive(false);
            }
            startTravelCheck = false;
        }
    }
}
