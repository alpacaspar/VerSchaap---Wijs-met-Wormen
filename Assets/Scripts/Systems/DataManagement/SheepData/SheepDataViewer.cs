using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class SheepDataViewer : DataViewer
{
    [Header("Prefabs")]
    public GameObject sheepButtonPrefab;

    [Header("UI Panels")]
    //public RectTransform sheepButtonContainer;
    public GameObject overviewPanel;
    public GameObject detailsPanel;
    
    [Header("Sheep variable fields")]
    public TMP_InputField inputTag;
    public TMP_Dropdown inputSex;
    public TMP_Dropdown inputSheepType;   
    public TMP_Dropdown inputPairCollection;   
    public Button inputTSBorn;
    public UICalendarWidget calendarWidget;
    public Window_Graph graph;
    public Image sheepImg;

    [Header("Element Options")]
    public Button btnCancel;
    public Button btnSave;
    public Button btnAddSheep;

    [HideInInspector]
    public SheepObject selectedSheep;
    
    // dirty var to fix the calendar
    [HideInInspector]
    public DateTimeOffset tmpTime = new DateTimeOffset(DateTime.UtcNow);
    [HideInInspector]
    public SheepDataReader sheepDataReader;
    [Header("Other")]
    public ScrollRect scrollRect;
    public Dictionary<string, Sprite> sheepImages = new Dictionary<string, Sprite>();

    public override GameObject CreateNewButton(ObjectUUID objToAdd)
    {
        SheepObject s = objToAdd as SheepObject;
        var buttonGameObject = Instantiate(sheepButtonPrefab, buttonContainer);
        var but = buttonGameObject.GetComponentInChildren<SheepButton>();
        but.SetInfo(s, this);
        but.buttonMode = SheepButtonMode.ClickToEditOrRemove;
        return buttonGameObject;
    }

    
    public override void RemoveButton(ObjectUUID objToRemove)
    {
        SheepObject sheep = objToRemove as SheepObject;
        if (sheep == null) return;

        for (int i = 0; i < buttonContainer.childCount; i++)
        {
            var butObj = buttonContainer.GetChild(i).gameObject;
            var but = butObj.GetComponentInChildren<SheepButton>();

            if (but.sheep.UUID == sheep.UUID)
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

    /// <summary>
    /// Updates the image of the sheep
    /// </summary>
    /// <param name="sheepSpecies"></param>
    public void UpdateSheepImage(string sheepSpecies)
    {
        Sprite spr = null;
        sheepImages.TryGetValue(sheepSpecies, out spr);
        sheepImg.sprite = spr;
    }

    private void Start()
    {
        calendarWidget.sheepDataReader = this;
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

        btnAddSheep.onClick.AddListener(delegate
        {
            panelMode = DetailsPanelMode.CreatingElement;
            selectedSheep = new SheepObject
            {
                tsBorn = tmpTime.ToUnixTimeSeconds()
            };
            tmpTime = DateTimeOffset.UtcNow;
            ShowDetails(selectedSheep);
        });

        btnSave.onClick.AddListener(delegate
        {
            Enum.TryParse<Sex>(inputSex.GetComponentInChildren<TextMeshProUGUI>().text, out Sex sex);
            Enum.TryParse<SheepType>(inputSheepType.GetComponentInChildren<TextMeshProUGUI>().text, out SheepType sheepType);
            var pairCollectionID = sheepDataReader.GetPairCollectionUUIDByName(inputPairCollection.GetComponentInChildren<TextMeshProUGUI>().text);

            SheepObject tmpSheep = new SheepObject
            {
                UUID = selectedSheep != null ? selectedSheep.UUID : Helpers.GenerateUUID(),
                sheepTag = inputTag.text,
                sex = sex,
                sheepType = sheepType,
                tsBorn = calendarWidget.timeStamp.ToUnixTimeSeconds(),
                weight = graph.sheepWeights,
                pairCollectionID = pairCollectionID
            };
            
            StartCoroutine(sheepDataReader.UpdateSheepData(tmpSheep));
            SetPanelVisibilty(false);
        });
    }

    public void CreateSheepButtonsFromDB(List<SheepObject> sheepDatabase)
    {
        foreach (SheepObject s in sheepDatabase)
        {
            if (s.isDeleted == 1) continue;
            CreateNewButton(s);
        }
    }

    /// <summary>
    /// Moves the scrollview to a specific element
    /// </summary>
    /// <param name="target">element to scroll to</param>
    public void MoveScrollViewToElement(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        buttonContainer.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(buttonContainer.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }
    
    /// <summary>
    /// Updates the text of the birthdate field
    /// </summary>
    /// <param name="time">birthdate</param>
    public void UpdateTSButton(DateTimeOffset time)
    {
        string tsBornString = time.Day + "-" + time.Month + "-" + time.Year;
        inputTSBorn.GetComponentInChildren<TextMeshProUGUI>().SetText(tsBornString);
    }

    public void SetupDetailsPanel()
    {
        Sex[] valsSex = (Sex[])Enum.GetValues(typeof(Sex));
        SheepType[] valsSheepType = (SheepType[])Enum.GetValues(typeof(SheepType));
        List<TMP_Dropdown.OptionData> options = valsSex.Select(val => new TMP_Dropdown.OptionData(val.ToString())).ToList();
        inputSex.AddOptions(options);
        options = new List<TMP_Dropdown.OptionData>();

        foreach (var val in valsSheepType)
        {
            options.Add(new TMP_Dropdown.OptionData(val.ToString()));
        }

        inputSheepType.AddOptions(options);

        inputSheepType.onValueChanged.AddListener(delegate { UpdateSheepImage(inputSheepType.captionText.text); });

        // Show and hide the calendar when the input button is clicked
        inputTSBorn.onClick.AddListener(delegate
        {
            calendarWidget.gameObject.SetActive(!calendarWidget.gameObject.activeSelf);
            calendarWidget.SetDate(tmpTime.ToUnixTimeSeconds());
        });
    }

    /// <summary>
    /// Sets the visibilty of the overview and details panels
    /// </summary>
    /// <param name="showDetails">show the details panel</param>
    public void SetPanelVisibilty(bool showDetails)
    {
        overviewPanel.SetActive(!showDetails);
        detailsPanel.SetActive(showDetails);   
    }

    public void UpdatePairCollectionDropDown()
    {
        inputPairCollection.options = new List<TMP_Dropdown.OptionData>();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (var val in Database.GetDatabase().pairCollection)
        {
            options.Add(new TMP_Dropdown.OptionData(val.pairCollectionName));
        }

        inputPairCollection.AddOptions(options);
    }

    public void ShowDetails(SheepObject sheep)
    {
        selectedSheep = sheep;
        SetDetailsPanelTitle("Schaap");
        SetPanelVisibilty(true);
        UpdatePairCollectionDropDown();
        UpdateSheepImage(selectedSheep.sheepType.ToString());
        calendarWidget.SetDate(sheep.tsBorn);
        inputTag.SetTextWithoutNotify(sheep.sheepTag);

        // Set the sex input dropdown to the correct value
        for (int i = 0; i < inputSex.options.Count; i++)
        {
            if (!string.Equals(inputSex.options[i].text, sheep.sex.ToString(), StringComparison.CurrentCultureIgnoreCase)) continue;
            inputSex.value = i;
            break;
        }

        // Set the sheep type input dropdown to the correct value
        for (int i = 0; i < inputSheepType.options.Count; i++)
        {
            if (!string.Equals(inputSheepType.options[i].text, sheep.sheepType.ToString(), StringComparison.CurrentCultureIgnoreCase)) continue;
            inputSheepType.value = i;
            break;
        }

        // Set the pair input dropdown to the correct value
        for (int i = 0; i < inputPairCollection.options.Count; i++)
        {
            // TODO convert from uuid to readible pair name
            if (!string.Equals(inputPairCollection.options[i].text, sheepDataReader.GetPairCollectionNameByUUID(sheep.pairCollectionID), StringComparison.CurrentCultureIgnoreCase)) continue;
            inputPairCollection.value = i;
            break;
        }

        DateTimeOffset tsBornTime = DateTimeOffset.FromUnixTimeSeconds(sheep.tsBorn);
        tmpTime = tsBornTime;
        UpdateTSButton(tsBornTime);

        // Graph logic
        graph.ShowSheepWeightGraph(sheep.weight, (int _i) => "Day " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + "kg");
    }
}
