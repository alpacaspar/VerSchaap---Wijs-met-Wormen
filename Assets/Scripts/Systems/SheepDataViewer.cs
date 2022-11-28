using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class SheepDataViewer : MonoBehaviour
{
    public RectTransform sheepUIPanel;
    public GameObject sheepUIObject;

    public GameObject overviewPanel;
    public GameObject detailsPanel;

    public TMP_InputField inputUUID;
    public TMP_Dropdown inputSex;
    public TMP_Dropdown inputSheepType;
    
    public Button inputTSBorn;
    public UICalendarWidget calendarWidget;

    public Button btnCancel;
    public Button btnSave;
    public Button btnAddSheep;
    
    public SheepObject selectedSheep;
    
    // dirty var to fix the calendar
    public DateTimeOffset tmpTime = new DateTimeOffset(DateTime.UtcNow);
    public bool bAddingSheep = false;
    public SheepDataReader sheepDataReader;

    private void Start()
    {
        calendarWidget.sheepDataReader = this;
        SetupDetailsPanel();
        SetupButtons();
    }

    private void SetupButtons()
    {
        btnCancel.onClick.AddListener(delegate
        {
            overviewPanel.SetActive(true);
            detailsPanel.SetActive(false);
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
                UUID = inputUUID.text,
                sex = sex,
                sheepType = sheepType,
                tsBorn = calendarWidget.timeStamp.ToUnixTimeSeconds()
            };
            
            sheepDataReader.UpdateSheepData(tmpSheep);
            overviewPanel.SetActive(true);
            detailsPanel.SetActive(false);
        });
    }

    public void CreateSheepButtonsFromDB(List<SheepObject> sheepDatabase)
    {
        foreach (SheepObject s in sheepDatabase)
        {
            CreateNewSheepButton(s);
        }
    }
    
    public void CreateNewSheepButton(SheepObject s)
    {
        var sheepPanelGameObject = Instantiate(sheepUIObject, sheepUIPanel);
        sheepPanelGameObject.GetComponentInChildren<SheepButton>().SetInfo(s, this);
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

        // Show and hide the calendar when the input button is clicked
        inputTSBorn.onClick.AddListener(delegate
        {
            calendarWidget.gameObject.SetActive(!calendarWidget.gameObject.activeSelf);
            calendarWidget.SetDate(tmpTime.ToUnixTimeSeconds());
        });
    }

    public void ShowDetails(SheepObject sheep)
    {
        selectedSheep = sheep;
        overviewPanel.SetActive(false);
        detailsPanel.SetActive(true);
        calendarWidget.SetDate(sheep.tsBorn);
        inputUUID.SetTextWithoutNotify(sheep.UUID);

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
    }
}
