using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class SheepDataViewer : MonoBehaviour
{
    // The "content" object inside the scroll view
    public RectTransform sheepUIPanel;
    public GameObject sheepUIObject;

    public GameObject navPanel;
    public GameObject overviewPanel;
    public GameObject detailsPanel;

    public TMP_InputField inputTag;
    public TMP_Dropdown inputSex;
    public TMP_Dropdown inputSheepType;
    
    public Button inputTSBorn;
    public UICalendarWidget calendarWidget;
    public Window_Graph graph;

    public Button btnCancel;
    public Button btnSave;
    public Button btnAddSheep;
    
    public SheepObject selectedSheep;
    
    // dirty var to fix the calendar
    public DateTimeOffset tmpTime = new DateTimeOffset(DateTime.UtcNow);
    public bool bAddingSheep = false;
    public SheepDataReader sheepDataReader;

    public Image sheepImg;
    public Dictionary<string, Sprite> sheepImages = new Dictionary<string, Sprite>();

    public ScrollRect scrollRect;

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
            bAddingSheep = false;
        });

        btnAddSheep.onClick.AddListener(delegate
        {
            bAddingSheep = true;
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

            SheepObject tmpSheep = new SheepObject
            {
                UUID = selectedSheep != null ? selectedSheep.UUID : Helpers.GenerateUUID(),
                sheepTag = inputTag.text,
                sex = sex,
                sheepType = sheepType,
                tsBorn = calendarWidget.timeStamp.ToUnixTimeSeconds(),
                weight = graph.sheepWeights
            };
            
            sheepDataReader.UpdateSheepData(tmpSheep);
            SetPanelVisibilty(false);
        });
    }

    public void CreateSheepButtonsFromDB(List<SheepObject> sheepDatabase)
    {
        foreach (SheepObject s in sheepDatabase)
        {
            CreateNewSheepButton(s);
        }
    }
    
    public GameObject CreateNewSheepButton(SheepObject s)
    {
        var sheepPanelGameObject = Instantiate(sheepUIObject, sheepUIPanel);
        sheepPanelGameObject.GetComponentInChildren<SheepButton>().SetInfo(s, this);
        return sheepPanelGameObject;
    }

    public void MoveScrollViewToElement(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        sheepUIPanel.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(sheepUIPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
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
        if (navPanel != null) navPanel.SetActive(!showDetails);
        overviewPanel.SetActive(!showDetails);
        detailsPanel.SetActive(showDetails);   
    }

    public void ShowDetails(SheepObject sheep)
    {
        selectedSheep = sheep;
        SetPanelVisibilty(true);
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

        DateTimeOffset tsBornTime = DateTimeOffset.FromUnixTimeSeconds(sheep.tsBorn);
        tmpTime = tsBornTime;
        UpdateTSButton(tsBornTime);

        // Graph logic
        graph.ShowSheepWeightGraph(sheep.weight, (int _i) => "Day " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + "kg");
    }
}
