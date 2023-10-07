using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowShipButton : MonoBehaviour
{
    private TMP_Text buttonText;
    private bool _followShip;
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowShipText();
    }

    void FollowShipText()
    {
        if (_followShip != FollowShip.followShip)
        {
            _followShip = FollowShip.followShip;
            if (_followShip == true)
            {
                buttonText.text = "Unfollow Ship";
            }
            else
            {
                buttonText.text = "Follow Ship";
            }
        }
    }

    public void followButtonPress()
    {
        if(!SpaceTravelCamera.dragging)
        {
            FollowShip.switchFollowMode();
        }
    }


}
