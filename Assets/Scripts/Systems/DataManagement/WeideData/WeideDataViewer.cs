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
    public TextMeshProUGUI detailPanelTitle;

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

    public void RemoveWeideButton(WeideObject weide)
    {
        for (int i = 0; i < WeideButtonContainer.childCount; i++)
        {
            var butObj = WeideButtonContainer.GetChild(i).gameObject;
            var but = butObj.GetComponentInChildren<WeideButton>();

            if (but.weide.UUID == weide.UUID)
            {
                Destroy(butObj);
                break;
            }
        }
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
                UUID = Helpers.GenerateUUID()
            };

            ShowDetails(selectedElement);
        });

        btnSave.onClick.AddListener(delegate
        {

            WeideObject tmpWeide = new WeideObject
            {
                UUID = selectedElement != null ? selectedElement.UUID : Helpers.GenerateUUID(),
                perceelName = inputPerceelName.text,
                surfaceSqrMtr = int.Parse(inputSurfaceArea.text),
                surfaceQuality = float.Parse(inputSurfaceQuality.text),
                //grassTypes = new List<GrassType[]>(),
                //currentSheeps = new List<SheepType[]>(),
                //extraRemarks = new List<string>()
            };

            sheepDataReader.UpdateWeideData(tmpWeide);
            SetPanelVisibilty(false);
        });
    }

    public void CreateSheepButtonsFromDB(List<WeideObject> database)
    {
        foreach (WeideObject s in database)
        {
            CreateNewWeideButton(s);
        }
    }
    
    public void CreateNewWeideButton(WeideObject s)
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
        detailPanelTitle.text = bAddingElement ? "Perceel toevoegen" : "Perceel bewerken";
        SetPanelVisibilty(true);
        inputPerceelName.SetTextWithoutNotify(element.perceelName);
        inputSurfaceArea.SetTextWithoutNotify(element.surfaceSqrMtr.ToString());
        inputSurfaceQuality.SetTextWithoutNotify(element.surfaceQuality.ToString());
    }
}
