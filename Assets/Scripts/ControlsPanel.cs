using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPanel : MonoBehaviour
{
    public GameObject controlPanelUI;
    public bool panelEnabled = false;

    private void Update()
    {
        Panel();       
    }
    public void Panel()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            if (panelEnabled == false)
            {
                panelEnabled = true;
                controlPanelUI.SetActive(true);
            }
            else if (panelEnabled == true)
            {
                panelEnabled = false;
                controlPanelUI.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Menu.startOverPressed)
        {
            controlPanelUI.SetActive(false);
            panelEnabled = false;
        }
    }
}
