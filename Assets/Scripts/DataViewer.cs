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

    public RectTransform buttonContainer;

    [HideInInspector]
    public DetailsPanelMode panelMode = DetailsPanelMode.None;

    public virtual GameObject CreateNewButton(ObjectUUID objToAdd)
    {
        return null;
    }

    public virtual void RemoveButton(ObjectUUID objToRemove)
    {
        //SheepObject sheep = objToRemove as SheepObject;
        if (objToRemove == null) return;

        for (int i = 0; i < buttonContainer.childCount; i++)
        {
            var butObj = buttonContainer.GetChild(i).gameObject;
            var but = butObj.GetComponentInChildren<ObjectUUIDButton>();

            if (but.element.UUID == objToRemove.UUID)
            {
                Destroy(butObj);
                break;
            }
        }
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

    public void RemoveAllButtons()
    {
        for (int i = buttonContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(buttonContainer.GetChild(i).gameObject);
        }
    }
}
