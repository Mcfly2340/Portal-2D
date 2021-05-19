using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPanel : MonoBehaviour
{
    public GameObject controlPanelUI;
    public bool panelEnabled = true;
    private void Update()
    {
        Panel();
    }
    void startPanelEnabled()
    {
        controlPanelUI.SetActive(false);
    }
    public void Panel()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            if (panelEnabled == true)
            {
                controlPanelUI.SetActive(false);
                panelEnabled = false;
            }
            else
            {
                controlPanelUI.SetActive(true);
                panelEnabled = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Menu.startOverPressed)
        {
            controlPanelUI.SetActive(false);
            panelEnabled = false;
        }
        else if (panelEnabled)
        {
            Invoke(nameof(startPanelEnabled), 5);
            panelEnabled = false;
        }
    }
}
