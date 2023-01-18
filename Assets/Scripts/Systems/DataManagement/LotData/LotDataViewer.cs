using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class LotDataViewer : DataViewer
{
    [Header("Prefabs")]
    public GameObject LotButtonPrefab;
    
    [Header("UI Panels")]
    public RectTransform LotButtonContainer;
    public GameObject overviewPanel;
    public GameObject detailsPanel;

    [Header("Lot variable fields")]
    public TMP_InputField inputPerceelName;
    public TMP_InputField inputSurfaceArea;
    public TMP_InputField inputSurfaceQuality;
    public Image sheepImg;

    [Header("Element Options")]
    public Button btnCancel;
    public Button btnSave;
    public Button btnAddElement;
    
    [HideInInspector]
    public LotObject selectedElement;
    [HideInInspector]
    public SheepDataReader sheepDataReader;
    
    public Dictionary<string, Sprite> sheepImages = new Dictionary<string, Sprite>();

    public override GameObject CreateNewButton(ObjectUUID objToAdd)
    {
        LotObject Lot = objToAdd as LotObject;
        var buttonGameObject = Instantiate(LotButtonPrefab, LotButtonContainer);
        buttonGameObject.GetComponentInChildren<LotButton>().SetInfo(Lot, this);
        return buttonGameObject;
    }

    public override void RemoveButton(ObjectUUID objToRemove)
    {
        LotObject Lot = objToRemove as LotObject;
        if (Lot == null) return;

        for (int i = 0; i < LotButtonContainer.childCount; i++)
        {
            var butObj = LotButtonContainer.GetChild(i).gameObject;
            var but = butObj.GetComponentInChildren<LotButton>();

            if (but.Lot.UUID == Lot.UUID)
            {
                Destroy(butObj);
                break;
            }
        }
    }

    private void LoadSheepImages()
    {
        var textures = Resources.LoadAll("SheepImages", typeof(Sprite));

        foreach (var t in textures)
        {
            sheepImages.Add(t.name, (Sprite)t);
        }
    }

    public void UpdateSheepImage(string sheepName)
    {
        Sprite spr = null;
        sheepImages.TryGetValue(sheepName, out spr);
        sheepImg.sprite = spr;
    }

    private void Start()
    {
        SetupDetailsPanel();
        SetupButtons();
        LoadSheepImages();
    }

    private void SetupButtons()
    {
        btnCancel.onClick.AddListener(delegate
        {
            SetPanelVisibilty(false);
            panelMode = DetailsPanelMode.None;
        });

        btnAddElement.onClick.AddListener(delegate
        {
            panelMode = DetailsPanelMode.CreatingElement;
            selectedElement = new LotObject
            {
                UUID = Helpers.GenerateUUID()
            };

            ShowDetails(selectedElement);
        });

        btnSave.onClick.AddListener(delegate
        {

            LotObject tmpLot = new LotObject
            {
                UUID = selectedElement != null ? selectedElement.UUID : Helpers.GenerateUUID(),
                perceelName = inputPerceelName.text,
                surfaceSqrMtr = int.Parse(inputSurfaceArea.text),
                surfaceQuality = float.Parse(inputSurfaceQuality.text),
                //grassTypes = new List<GrassType[]>(),
                //currentSheeps = new List<SheepType[]>(),
                //extraRemarks = new List<string>()
            };

            sheepDataReader.UpdateLotData(tmpLot);
            SetPanelVisibilty(false);
        });
    }

    public void CreateSheepButtonsFromDB(List<LotObject> database)
    {
        foreach (LotObject s in database)
        {
            CreateNewButton(s);
        }
    }

    public void SetupDetailsPanel()
    {
        
    }

    public void SetPanelVisibilty(bool showDetails)
    {
        overviewPanel.SetActive(!showDetails);
        detailsPanel.SetActive(showDetails);   
    }

    public void ShowDetails(LotObject element)
    {
        selectedElement = element;
        SetDetailsPanelTitle("Perceel");
        SetPanelVisibilty(true);
        inputPerceelName.SetTextWithoutNotify(element.perceelName);
        inputSurfaceArea.SetTextWithoutNotify(element.surfaceSqrMtr.ToString());
        inputSurfaceQuality.SetTextWithoutNotify(element.surfaceQuality.ToString());
    }
}
