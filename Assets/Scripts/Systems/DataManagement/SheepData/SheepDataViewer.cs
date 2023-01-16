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
    public RectTransform sheepButtonContainer;
    public GameObject overviewPanel;
    public GameObject detailsPanel;
    
    [Header("Sheep variable fields")]
    public TMP_InputField inputTag;
    public TMP_Dropdown inputSex;
    public TMP_Dropdown inputSheepType;   
    public TMP_Dropdown inputKoppel;   
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
    //[HideInInspector]
    //public bool bAddingSheep = false;
    [HideInInspector]
    public SheepDataReader sheepDataReader;
    [Header("Other")]
    public ScrollRect scrollRect;
    public Dictionary<string, Sprite> sheepImages = new Dictionary<string, Sprite>();

    public override GameObject CreateNewButton(ObjectUUID objToAdd)
    {
        SheepObject s = objToAdd as SheepObject;
        var buttonGameObject = Instantiate(sheepButtonPrefab, sheepButtonContainer);
        var but = buttonGameObject.GetComponentInChildren<SheepButton>();
        but.SetInfo(s, this);
        but.buttonMode = SheepButtonMode.ClickToEditOrRemove;
        return buttonGameObject;
    }

    public override void RemoveButton(ObjectUUID objToRemove)
    {
        SheepObject sheep = objToRemove as SheepObject;
        if (sheep == null) return;

        for (int i = 0; i < sheepButtonContainer.childCount; i++)
        {
            var butObj = sheepButtonContainer.GetChild(i).gameObject;
            var but = butObj.GetComponentInChildren<SheepButton>();

            if (but.sheep.UUID == sheep.UUID)
            {
                Destroy(butObj);
                break;
            }
        }
    }

    private void RemoveAllButtons()
    {
        for (int i = sheepButtonContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(sheepButtonContainer.GetChild(i).gameObject);
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
            //bAddingSheep = false;
        });

        btnAddSheep.onClick.AddListener(delegate
        {
            panelMode = DetailsPanelMode.CreatingElement;
            //bAddingSheep = true;
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
            var koppelID = sheepDataReader.GetKoppelUUIDByName(inputKoppel.GetComponentInChildren<TextMeshProUGUI>().text);

            SheepObject tmpSheep = new SheepObject
            {
                UUID = selectedSheep != null ? selectedSheep.UUID : Helpers.GenerateUUID(),
                sheepTag = inputTag.text,
                sex = sex,
                sheepType = sheepType,
                tsBorn = calendarWidget.timeStamp.ToUnixTimeSeconds(),
                weight = graph.sheepWeights,
                sheepKoppelID = koppelID
            };
            
            sheepDataReader.UpdateSheepData(tmpSheep);
            SetPanelVisibilty(false);
        });
    }

    public void CreateSheepButtonsFromDB(List<SheepObject> sheepDatabase)
    {
        foreach (SheepObject s in sheepDatabase)
        {
            CreateNewButton(s);
        }
    }

    public void MoveScrollViewToElement(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        sheepButtonContainer.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(sheepButtonContainer.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }
    
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

    public void SetPanelVisibilty(bool showDetails)
    {
        overviewPanel.SetActive(!showDetails);
        detailsPanel.SetActive(showDetails);   
    }

    public void UpdateKoppelDropDown()
    {
        inputKoppel.options = new List<TMP_Dropdown.OptionData>();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (var val in sheepDataReader.testDatabase.sheepKoppels)
        {
            options.Add(new TMP_Dropdown.OptionData(val.koppelName));
        }

        inputKoppel.AddOptions(options);
    }

    public void ShowDetails(SheepObject sheep)
    {
        selectedSheep = sheep;
        SetDetailsPanelTitle("Schaap");
        SetPanelVisibilty(true);
        UpdateKoppelDropDown();
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

        // Set the koppel input dropdown to the correct value
        for (int i = 0; i < inputKoppel.options.Count; i++)
        {
            // TODO convert from uuid to readible koppel name
            if (!string.Equals(inputKoppel.options[i].text, sheepDataReader.GetKoppelNameByUUID(sheep.sheepKoppelID), StringComparison.CurrentCultureIgnoreCase)) continue;
            inputKoppel.value = i;
            break;
        }

        DateTimeOffset tsBornTime = DateTimeOffset.FromUnixTimeSeconds(sheep.tsBorn);
        tmpTime = tsBornTime;
        UpdateTSButton(tsBornTime);

        // Graph logic
        graph.ShowSheepWeightGraph(sheep.weight, (int _i) => "Day " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + "kg");
    }
}
