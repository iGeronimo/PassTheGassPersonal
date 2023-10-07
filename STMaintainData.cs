using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STMaintainData : MonoBehaviour
{
    public static STMaintainData Instance;

    public Vector3 targetPosition;

    public Vector3 SpaceShipPosition;

    public System.DateTime lastTime;




    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
