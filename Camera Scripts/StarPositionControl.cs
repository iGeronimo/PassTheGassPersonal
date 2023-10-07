using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPositionControl : MonoBehaviour
{
    public Sprite[] stars;
    private new SpriteRenderer renderer;

    private GameObject backGround;
    private SpriteRenderer backGroundRenderer;
    private float boundX;
    private float boundY;

    public float starSpeed = 2;

    private const int starSize = 1106 / 100;
    private Vector3 lastCameraPosition;
    private void Awake()
    {
        lastCameraPosition = Camera.main.transform.position;
    }
    void Start()
    {
        backGround = GameObject.Find("StarBackground");
        backGroundRenderer = backGround.GetComponent<SpriteRenderer>();
        setStarSprite();
        setBounds();
    }

    void setStarSprite()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = stars[Random.Range(0, stars.Length)];
    }

    void setBounds()
    {
        boundX = backGroundRenderer.bounds.size.x;
        boundY = backGroundRenderer.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        adjustPosition();
    }

    private void FixedUpdate()
    {
        changePosition();
    }

    void adjustPosition()
    {
        //Adjust x
        if(transform.position.x + (starSize / 2) < -boundX /2)
        {
            transform.position = new Vector3((boundX /2) + (starSize /2), transform.position.y, transform.position.z);
        }
        if(transform.position.x - (starSize / 2) > boundX /2)
        {
            transform.position = new Vector3((-boundX / 2) - (starSize / 2), transform.position.y, transform.position.z);
        }
        //Adjust y
        if(transform.position.y + starSize/ 2 < -boundY/2) 
        {
            transform.position = new Vector3(transform.position.x, (boundY / 2) + (starSize / 2), transform.position.z);
        }
        if(transform.position.y - starSize/2 > boundY/2)
        {
            transform.position = new Vector3(transform.position.x, (-boundY / 2) - (starSize / 2), transform.position.z);
        }
    }
    
    void changePosition()
    {
        this.transform.position -= (Camera.main.transform.position - lastCameraPosition) * starSpeed;
        lastCameraPosition = Camera.main.transform.position;
    }
}
