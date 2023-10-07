using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceTravelCamera : MonoBehaviour
{
    //public Camera camera;

    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 lastDifference;

    public float boundCameraX = 20;
    public float boundCameraY = 10;

    public float _boundCameraX;
    public float _boundCameraY;

    public float maxOrthoSize = 30;
    public float minOrthoSize = 1;

    public float zoomTouchSpeed = 0.003f;
    public float zoomSpeed = 1f;

    public float lerpStepSpeed = 0.1f;

    public float minimumDragLength = 0.01f;

    [SerializeField]
    private bool _drag = false;

    public static bool dragging = false;

    void Start()
    {
        MoveSpaceCamera.cameraPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        dragging = _drag;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        setBounds();
        CameraZoomScrollWheel();
        CameraZoomTouch();
        CameraDrag();
        KeepCameraInBounds();
        lerpMovement();
    }

    void lerpMovement()
    {
        if (MoveSpaceCamera.cameraMoving)
        {
            if ((Vector2)Camera.main.transform.position != MoveSpaceCamera.cameraPosition)
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(MoveSpaceCamera.cameraPosition.x, MoveSpaceCamera.cameraPosition.y, Camera.main.transform.position.z), lerpStepSpeed);
            }
            if (Vector3.Distance((Vector2)Camera.main.transform.position, MoveSpaceCamera.cameraPosition) < 0.001)
            {
                MoveSpaceCamera.cameraMoving = false;
            }
        }
        if (MoveSpaceCamera.cameraZooming)
        {
            if (Camera.main.orthographicSize != MoveSpaceCamera.orthographicSize)
            {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, MoveSpaceCamera.orthographicSize, lerpStepSpeed);
            }
            if (Mathf.Abs(MoveSpaceCamera.orthographicSize - Camera.main.orthographicSize) < 0.01)
            {
                MoveSpaceCamera.cameraZooming = false;
            }
        }
    }

    void setBounds()
    {
        _boundCameraX = boundCameraX - (Camera.main.orthographicSize * (Screen.width / Screen.height));
        _boundCameraY = boundCameraY - (Camera.main.orthographicSize);
    }


    void CameraDrag()
    {
        //Alex
        if (TutorialSpace.current.tutorialPart == 3 || !TutorialSpace.current.isTutorial)
        {
            if (!MoveSpaceCamera.cameraMoving || FollowShip.followShip)
            {
                if (Input.GetMouseButton(0) && Input.touchCount < 2)
                {
                    if (true)
                    {
                        Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
                        if (Vector3.Distance(Difference, lastDifference) > minimumDragLength * (Camera.main.orthographicSize * 2))
                        {
                            if (_drag == false)
                            {
                                _drag = true;
                                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                if (FollowShip.followShip)
                                {
                                    FollowShip.switchFollowMode();
                                }
                            }
                        }
                    }
                }
                else
                {
                    lastDifference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
                    _drag = false;
                }
                if (_drag == true)
                {
                    Camera.main.transform.position = Origin - Difference;

                }
            }
        }
    }

    void KeepCameraInBounds()
    {
        if (Camera.main.transform.position.x < _boundCameraX * -1)
        {
            Camera.main.transform.position = new Vector3(_boundCameraX * -1, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        if (Camera.main.transform.position.x > _boundCameraX)
        {
            Camera.main.transform.position = new Vector3(_boundCameraX, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        if (Camera.main.transform.position.y < _boundCameraY * -1)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, _boundCameraY * -1, Camera.main.transform.position.z);
        }
        if (Camera.main.transform.position.y > _boundCameraY)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, _boundCameraY, Camera.main.transform.position.z);
        }
    }

    void CameraZoomScrollWheel()
    {
        if (!MoveSpaceCamera.cameraZooming)
        {
            Camera.main.orthographicSize -= (Input.mouseScrollDelta.y * zoomSpeed);
        }
        if (Camera.main.orthographicSize > maxOrthoSize)
        {
            Camera.main.orthographicSize = maxOrthoSize;
        }

        if (Camera.main.orthographicSize < minOrthoSize)
        {
            Camera.main.orthographicSize = minOrthoSize;
        }
    }

    void CameraZoomTouch()
    {
        if (!MoveSpaceCamera.cameraZooming)
        {
            if(Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Camera.main.orthographicSize -= (difference * zoomTouchSpeed); 
            }
        }
        if (Camera.main.orthographicSize > maxOrthoSize)
        {
            Camera.main.orthographicSize = maxOrthoSize;
        }

        if (Camera.main.orthographicSize < minOrthoSize)
        {
            Camera.main.orthographicSize = minOrthoSize;
        }
    }

    //Alex
    private bool isPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
