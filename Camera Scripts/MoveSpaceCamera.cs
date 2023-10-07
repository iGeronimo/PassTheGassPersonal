using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveSpaceCamera
{
    public static Vector2 cameraPosition;
    public static float orthographicSize;
    public static bool cameraMoving;
    public static bool cameraZooming;
    public static void SetCameraPositionAndSize(Vector2 position, float newOrthographicSize)
    {
        SetCameraSize(newOrthographicSize);
        SetCameraPosition(position);
    }

    public static void SetCameraPosition(Vector2 position)
    {
       if((Vector2)Camera.main.transform.position != position)
        {
            cameraPosition = position;
            cameraMoving = true;
        }
    }

    public static void SetCameraSize(float size)
    {
        if(Camera.main.orthographicSize != size)
        {
            orthographicSize = size;
            cameraZooming = true;
        }
    }
}
