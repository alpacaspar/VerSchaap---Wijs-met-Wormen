using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class KoppelDataViewer : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject KoppelButtonPrefab;
    public GameObject SheepButtonPrefab;

    [Header("UI Panels")]
    public RectTransform koppelButtonContainer;
    public RectTransform koppelSheepListButtonContainer;
    public GameObject overviewPanel;
    public GameObject detailsPanel;

    [Header("Koppel variable fields")]
    public TMP_InputField inputName;

    [Header("Element Options")]
    public Button btnCancel;
    public Button btnSave;
    public Button btnAddSheep;

    [HideInInspector]
    public SheepKoppel selectedElement;
    [HideInInspector]
    public bool bAddingElement = false;
    [HideInInspector]
    public SheepDataReader dataReader;

    [Header("Other")]
    public ScrollRect scrollRect;

    private void Start()
    {
        SetupDetailsPanel();
        SetupButtons();
    }

    private void SetupButtons()
    {
        /*
        btnCancel.onClick.AddListener(delegate
        {
            SetPanelVisibilty(false);
            bAddingElement = false;
        });

        btnAddSheep.onClick.AddListener(delegate
        {
            bAddingElement = true;
            selectedElement = new SheepKoppel
            {

            };

            ShowDetails(selectedElement);
        });

        btnSave.onClick.AddListener(delegate
        {
            
            sheepDataReader.UpdateSheepData(tmpSheep);
            
            SetPanelVisibilty(false);
        });
        */
    }

    public void CreateButtonsFromDB(List<SheepKoppel> elementList)
    {
        foreach (SheepKoppel s in elementList)
        {
            CreateNewButton(s);
        }
    }

    public GameObject CreateNewButton(SheepKoppel s)
    {
        var panelGameObject = Instantiate(KoppelButtonPrefab, koppelButtonContainer);
        panelGameObject.GetComponentInChildren<KoppelButton>().SetInfo(s, this);
        return panelGameObject;
    }

    // delete old sheep buttons
    public void RemoveExistingSheepButtons()
    {
        for (int i = koppelSheepListButtonContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(koppelSheepListButtonContainer.GetChild(i).gameObject);
        }
    }

    public GameObject CreateNewSheepButtons()
    {
        RemoveExistingSheepButtons();

        foreach (var sheepuuid in selectedElement.allSheep)
        {
            var foundSheep = dataReader.GetSheepObjectByUUID(sheepuuid);

            // sheep with this uuid was found in the database
            if (foundSheep != null)
            {
                var panelGameObject = Instantiate(SheepButtonPrefab, koppelSheepListButtonContainer);
                panelGameObject.GetComponentInChildren<SheepButton>().SetInfo(foundSheep, FindObjectOfType<SheepDataViewer>());
            }
        }
        // get sheep bij UUID
        return null;
    }

    public void MoveScrollViewToElement(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        koppelButtonContainer.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(koppelButtonContainer.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    public void SetupDetailsPanel()
    {

    }

    public void SetPanelVisibilty(bool showDetails)
    {
        if (overviewPanel != null) overviewPanel.SetActive(!showDetails);
        if (detailsPanel != null) detailsPanel.SetActive(showDetails);
    }

    public void ShowDetails(SheepKoppel element)
    {
        selectedElement = element;
        inputName.SetTextWithoutNotify(selectedElement.koppelName);
        SetPanelVisibilty(true);
        CreateNewSheepButtons();
    }
}
