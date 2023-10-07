using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    void Update()
    {
        lastTime();
    }

    void lastTime()
    {
        STMaintainData.Instance.lastTime = System.DateTime.Now;
    }
}
