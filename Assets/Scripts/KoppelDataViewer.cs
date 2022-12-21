using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class KoppelDataViewer : MonoBehaviour
{
    // The "content" object inside the scroll view
    public RectTransform buttonListContainer;
    public GameObject UIButtonPrefab;

    public GameObject overviewPanel;
    public GameObject detailsPanel;

    public Button btnCancel;
    public Button btnSave;
    public Button btnAddSheep;

    public SheepKoppel selectedElement;
    public bool bAddingElement = false;
    public SheepDataReader dataReader;

    public ScrollRect scrollRect;

    public void GetKoppels()
    {
        //sheepDataReader.testDatabase.sheepKoppels
        foreach (var kop in dataReader.testDatabase.sheepKoppels)
        {
            /*
            foreach (var shp in sheepDataReader.testDatabase.sheeps.Where(shp => shp.sheepKoppelID == kop.UUID))
            {

            }
            */
        }
    }

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
        var panelGameObject = Instantiate(UIButtonPrefab, buttonListContainer);
        panelGameObject.GetComponentInChildren<KoppelButton>().SetInfo(s, this);
        return panelGameObject;
    }

    public void MoveScrollViewToElement(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        buttonListContainer.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(buttonListContainer.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
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
        SetPanelVisibilty(true);
    }
}
