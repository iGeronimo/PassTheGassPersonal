using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    LineRenderer lr;
    LaserBeamController laserControl;

    public Material possibleLine;
    public Material blockedLine;

    private bool setController = false;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        setLines();
    }

    void setLines()
    {
        possibleLine.color = Color.green;
        blockedLine.color = Color.red;
    }

    private void Update()
    {
        //setLaserController();
    }

    void setLaserController()
    {
        if (!setController)
        {
            if (PlanetManager.planetManager.linesGenerated)
            {
                setController = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpaceShip"))
        {
            Debug.Log("SpaceShip Entered Line");
            lr.material = possibleLine;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpaceShip"))
        {
            Debug.Log("SpaceShip Exited Line");
            lr.material = blockedLine;
        }
    }
}
