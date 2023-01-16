using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DetailsPanelMode
{
    None,
    CreatingElement,
    EditingElement
}

public class DataViewer : MonoBehaviour
{
    public TextMeshProUGUI detailPanelTitle;
    [HideInInspector]
    public DetailsPanelMode panelMode = DetailsPanelMode.None;

    public virtual GameObject CreateNewButton(ObjectUUID objToAdd)
    {
        return null;
    }

    public virtual void RemoveButton(ObjectUUID objToRemove)
    {

    }

    public void SetDetailsPanelTitle(string txt)
    {
        string tmpTxt = txt;

        switch (panelMode)
        {
            default:
                break;
            case DetailsPanelMode.CreatingElement:
                tmpTxt += " toevoegen";
                break;
            case DetailsPanelMode.EditingElement:
                tmpTxt += " bewerken";
                break;
        }

        detailPanelTitle.text = tmpTxt;
    }
}
