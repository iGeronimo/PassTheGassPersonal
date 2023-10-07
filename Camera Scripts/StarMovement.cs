using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMovement : MonoBehaviour
{
    private SpriteRenderer backGround;
    public GameObject star;
    private float boundX;
    private float boundY;

    private const int starSize = 1106 / 100;

    private bool inBounds = true;

    // Start is called before the first frame update
    void Start()
    {
        backGround = this.GetComponent<SpriteRenderer>();
        createChildren();
    }

    

    // Update is called once per frame
    void LateUpdate()
    {
        
    }

    void createChildren()
    {
        boundX = backGround.bounds.size.x;
        boundY = backGround.bounds.size.y;
        Debug.Log(boundX);
        int rows = (int)Mathf.CeilToInt(boundX / starSize);
        int colums = (int)Mathf.CeilToInt(boundY / starSize);
        GameObject clone = Instantiate(star) as GameObject;

        for(int i = 0; i < colums + 1; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                GameObject c = Instantiate(clone) as GameObject;
                c.transform.SetParent(backGround.transform);
                c.transform.position = new Vector3((-boundX / 2) + (starSize * j), (boundY /2) - (starSize * i), 0);
                c.name = "starPanel" + i + "-" + j;
            }
        }
        Destroy(clone);
    }
}
