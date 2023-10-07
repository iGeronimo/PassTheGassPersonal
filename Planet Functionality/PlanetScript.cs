using UnityEngine;
using UnityEngine.EventSystems;
using System;
//using OpenCover.Framework.Model;
using System.Collections.Generic;

[System.Serializable]
public class PlanetScript : MonoBehaviour
{
    private GameObject spaceShip;

    private PlanetPanelScript PPscript;
    private PlanetVariation planetVariation;
    private PlanetBootup planetBoot;

    public float planetZoom = 2f;
    public float xAxisOffset = 1f;

    public bool canTravel = true;

    public int TravelRisk;

    public int LowRiskThreshHold;
    public int MediumRiskThreshHold;

    [Header("VFX")]
    public GameObject whiteAura;
    public GameObject yellowAura;
    public GameObject orangeAura;
    public GameObject redAura;
    public GameObject onPlanetClick;

    private CircleCollider2D coll;
    private float lastCamSize;


    private void Awake()
    {

    }
    void Start()
    {
        coll = GetComponent<CircleCollider2D>();
        lastCamSize = Camera.main.orthographicSize;
        planetBoot = GetComponent<PlanetBootup>();
        setRisk();
        setAura();
    }

    public void setRisk()
    {
        if (StateManager.saveAvailable) { }
        else
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            TravelRisk = UnityEngine.Random.Range(0, 101);
            Debug.Log("Set travel Risk at: " + TravelRisk);
        }
    }

    // Update is called once per frame
    void Update()
    {
        scaleHitBoxToCamera();
    }

    private void OnMouseUp()
    {
        if (TutorialSpace.current.tutorialPart == 3 || !TutorialSpace.current.isTutorial)
        {
            Vector3 mousez = Camera.main.WorldToScreenPoint(new Vector3(0, 0, Input.mousePosition.z));
            Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mousez.z));
            Vector3 direction = new Vector3(0, 0, Camera.main.transform.position.z);

            RaycastHit2D[] hits = Physics2D.RaycastAll(new(origin.x, origin.y), Vector2.up);

            bool hitPlanet = false;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Planet")
                {
                    hitPlanet = true;
                }
            }

            if (hitPlanet && !SpaceTravelCamera.dragging)
            {
                if (!SpaceUIManager.spaceUIManager.PlanetInspectUI.activeSelf)
                {
                    PlanetManager.planetManager.currentPlanet = this.gameObject;
                }
                GameObject vfxTransform = new GameObject();
                vfxTransform.transform.position = new Vector2(origin.x, origin.y);
                vfxTransform.transform.localScale = this.transform.localScale;
                Instantiate(onPlanetClick, vfxTransform.transform);
                MoveSpaceCamera.SetCameraPositionAndSize(new(GetComponentInParent<Transform>().position.x - xAxisOffset, GetComponentInParent<Transform>().position.y), planetZoom);
                SpaceUIManager.spaceUIManager.setPlanetInspectPanel();
                if (TutorialSpace.current.tutorialPart == 3 && canTravel)
                {
                    Debug.Log("adadasdasdasd");
                    TutorialSpace.current.tutorialPart++;
                }
            }
        }
    }

    void deactivateAura()
    {
        whiteAura.SetActive(false);
        yellowAura.SetActive(false);
        orangeAura.SetActive(false);
        redAura.SetActive(false);
    }

    void setAura()
    {
        if (TravelRisk < LowRiskThreshHold)
        {
            deactivateAura();
            yellowAura.SetActive(true);
        }
        else if (TravelRisk < MediumRiskThreshHold)
        {
            deactivateAura();
            orangeAura.SetActive(true);
        }
        else
        {
            deactivateAura();
            redAura.SetActive(true);
        }
    }

    public string getRiskLevel()
    {
        if (TravelRisk < LowRiskThreshHold)
        {
            SpaceUIManager.spaceUIManager.TMPRiskLevelText.color = Color.yellow;
            return "LOW RISK";
        }
        else if (TravelRisk < MediumRiskThreshHold)
        {
            SpaceUIManager.spaceUIManager.TMPRiskLevelText.color = new Color(255, 120, 0); //orange
            return "MEDIUM RISK";
        }
        else
        {
            SpaceUIManager.spaceUIManager.TMPRiskLevelText.color = Color.red;
            return "HIGH RISK";
        }
    }

    public string getDangerLevel()
    {
        if (planetBoot != null)
        {
            if (planetBoot.ringIndex == 0)
            {
                SpaceUIManager.spaceUIManager.TMPDangerLevelText.color = Color.white;
                return "LOW DANGER";
            }
            else if (planetBoot.ringIndex == 1)
            {
                SpaceUIManager.spaceUIManager.TMPDangerLevelText.color = Color.yellow;
                return "MEDIUM DANGER";
            }
            else if (planetBoot.ringIndex == 2)
            {
                SpaceUIManager.spaceUIManager.TMPDangerLevelText.color = new Color(255, 120, 0); //orange
                return "HIGH DANGER";
            }
            else
            {
                SpaceUIManager.spaceUIManager.TMPDangerLevelText.color = Color.red;
                return "BINGUS DANGER";
            }
        }
        else
        {
            return "";
        }
    }

    private void scaleHitBoxToCamera()
    {
        if (lastCamSize != Camera.main.orthographicSize)
        {
            coll.radius = 5 + (Camera.main.orthographicSize * 0.3f);
            lastCamSize = Camera.main.orthographicSize;
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