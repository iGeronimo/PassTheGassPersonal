using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class FollowShip : MonoBehaviour
{
    GameObject spaceShip;
    public static bool followShip;
    public bool followShipCopy;

    void Start()
    {
        spaceShip = GameObject.FindGameObjectWithTag("SpaceShip");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //activeFollow();
        followShipCopy = followShip;
    }

    
    void activeFollow()
    {
        if (followShip)
        {
            MoveSpaceCamera.SetCameraPosition(spaceShip.transform.position);
        }
    }

    public static void switchFollowMode()
    {
        followShip = !followShip;
    }

    public static void setFollowMode(bool b)
    {
        followShip = b;
    }
}
