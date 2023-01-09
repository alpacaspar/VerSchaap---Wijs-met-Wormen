using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class WeideDataViewer : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject WeideButtonPrefab;
    
    [Header("UI Panels")]
    public RectTransform WeideButtonContainer;
    public GameObject overviewPanel;
    public GameObject detailsPanel;

    [Header("Weide variable fields")]
    public TMP_InputField inputPerceelName;
    public TMP_InputField inputSurfaceArea;
    public TMP_InputField inputSurfaceQuality;
    public Image sheepImg;

    [Header("Element Options")]
    public Button btnCancel;
    public Button btnSave;
    public Button btnAddElement;
    
    [HideInInspector]
    public WeideObject selectedElement;
    [HideInInspector]
    public bool bAddingElement = false;
    [HideInInspector]
    public SheepDataReader sheepDataReader;
    
    public Dictionary<string, Sprite> sheepImages = new Dictionary<string, Sprite>();

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
            bAddingElement = false;
        });

        btnAddElement.onClick.AddListener(delegate
        {
            bAddingElement = true;
            selectedElement = new WeideObject
            {

            };

            ShowDetails(selectedElement);
        });

        btnSave.onClick.AddListener(delegate
        {

            WeideObject tmpWeide = new WeideObject
            {

            };
            
            SetPanelVisibilty(false);
        });
    }

    public void CreateSheepButtonsFromDB(List<WeideObject> database)
    {
        foreach (WeideObject s in database)
        {
            CreateNewElementButton(s);
        }
    }
    
    public void CreateNewElementButton(WeideObject s)
    {
        var sheepPanelGameObject = Instantiate(WeideButtonPrefab, WeideButtonContainer);
        sheepPanelGameObject.GetComponentInChildren<WeideButton>().SetInfo(s, this);
    }

    public void SetupDetailsPanel()
    {
        
    }

    public void SetPanelVisibilty(bool showDetails)
    {
        overviewPanel.SetActive(!showDetails);
        detailsPanel.SetActive(showDetails);   
    }

    public void ShowDetails(WeideObject element)
    {
        selectedElement = element;
        SetPanelVisibilty(true);
        inputPerceelName.SetTextWithoutNotify(element.perceelName);
        inputSurfaceArea.SetTextWithoutNotify(element.surfaceSqrMtr.ToString());
        inputSurfaceQuality.SetTextWithoutNotify(element.surfaceQuality.ToString());
    }
}
