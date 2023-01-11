using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class DisplayPanel
{
    public Button activateButton;
    public GameObject panel;
}

public class PanelDisplayer : MonoBehaviour
{
    public List<DisplayPanel> panels = new List<DisplayPanel>();
    private List<GameObject> panelObjects = new List<GameObject>();

    private void Start()
    {
        foreach (var panel in panels)
        {
            if (!panelObjects.Contains(panel.panel)) panelObjects.Add(panel.panel);
            panel.activateButton.onClick.AddListener(delegate { ShowPanel(panel.panel.name); });
        }
    }

    public void ShowPanel(string panelName)
    {
        var panel = GetPanelByName(panelName);

        if (panel != null)
        {
            HideAllPanels();
            panel.SetActive(true);
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in panelObjects)
        {
            panel.SetActive(false);
        }
    }

    public GameObject GetPanelByName(string panelName)
    {
        foreach (var panel in panelObjects)
        {
            if (panel.name == panelName) return panel;
        }

        return null;
    }
}
