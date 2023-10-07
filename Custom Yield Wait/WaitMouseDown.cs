using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitMouseDown : CustomYieldInstruction
{
    // Start is called before the first frame update
    public override bool keepWaiting
    {
        get
        {
            return !Input.GetMouseButtonDown(0);
        }
    }

    public WaitMouseDown()
    {

    }
}
