using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class WeideDataViewer : MonoBehaviour
{
    public RectTransform ButtonListContainer;
    public GameObject UIButtonPrefab;

    public GameObject overviewPanel;
    public GameObject detailsPanel;

    public TMP_InputField inputUUID;
    public TMP_InputField inputPerceelName;

    public Button btnCancel;
    public Button btnSave;
    public Button btnAddElement;
    
    public WeideObject selectedElement;
    public bool bAddingElement = false;
    public SheepDataReader sheepDataReader;

    public Image sheepImg;
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
                UUID = inputUUID.text,
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
        var sheepPanelGameObject = Instantiate(UIButtonPrefab, ButtonListContainer);
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
        inputUUID.SetTextWithoutNotify(element.UUID);
        inputPerceelName.SetTextWithoutNotify(element.perceelName);
    }
}
