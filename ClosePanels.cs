using UnityEngine;

[ExecuteInEditMode]
public class ClosePanels : MonoBehaviour
{
    [SerializeField]
    GameObject[] ClosingPanels;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void closePanelsInList()
    {
        foreach(GameObject panel in ClosingPanels)
        {
            panel.SetActive(false);
        }
    }
}
