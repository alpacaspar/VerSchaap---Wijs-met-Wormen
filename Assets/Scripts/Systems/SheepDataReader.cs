using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;


public class SheepDataReader : MonoBehaviour
{
    public TextAsset sheepDataFile;

    public List<SheepObject> SheepDatabase = new List<SheepObject>();
    public RectTransform SheepUIPanel;
    public GameObject SheepUIObject;

    public GameObject overviewPanel;
    public GameObject detailsPanel;

    public TMP_InputField inputUUID;
    public TMP_Dropdown inputSex;
    public TMP_Dropdown inputSheepType;
    
    public Button inputTSBorn;
    public UICalendarWidget calendarWidget;

    public Button btnCancel;
    public Button btnSave;

    // dummy var to test in the editor
    public bool writeToFile;
    public SheepObject selectedSheep;

    public DateTimeOffset tmpTime = new DateTimeOffset(DateTime.UtcNow);

    private void Start()
    {
        calendarWidget.sheepDataReader = this;

        SetupDetailsPanel();
        LoadSheepData(sheepDataFile);
        UpdateDatabase();

        foreach (var s in SheepDatabase)
        {
            var sheepPanelGameObject = Instantiate(SheepUIObject, SheepUIPanel);
            sheepPanelGameObject.GetComponent<SheepButton>().SetInfo(s, this);
        }

        btnCancel.onClick.AddListener(delegate
        {
            overviewPanel.SetActive(true);
            detailsPanel.SetActive(false);
        });

        btnSave.onClick.AddListener(delegate
        {
            SheepObject tmpSheep = new SheepObject();
            tmpSheep.UUID = inputUUID.text;

            Sex sex;
            Enum.TryParse<Sex>(inputSex.GetComponentInChildren<TextMeshProUGUI>().text, out sex);
            tmpSheep.sex = sex;

            SheepType sheepType;
            Enum.TryParse<SheepType>(inputSheepType.GetComponentInChildren<TextMeshProUGUI>().text, out sheepType);
            tmpSheep.sheepType = sheepType;

            tmpSheep.tsBorn = calendarWidget.timeStamp.ToUnixTimeSeconds();

            UpdateSheepData(tmpSheep);

            overviewPanel.SetActive(true);
            detailsPanel.SetActive(false);
        });
    }

    public void UpdateTSButton(DateTimeOffset time)
    {
        string tsBornString = time.Day + "-" + time.Month + "-" + time.Year;
        inputTSBorn.GetComponentInChildren<TextMeshProUGUI>().SetText(tsBornString);
    }

    public void UpdateSheepData(SheepObject sheep)
    {
        int nChilds = SheepUIPanel.childCount;

        // the actual data
        for (int i = 0; i < SheepDatabase.Count; i++)
        {
            var shp = SheepDatabase[i];
            if (shp.UUID == selectedSheep.UUID)
            {
                shp.UUID = sheep.UUID;
                shp.sex = sheep.sex;
                shp.sheepType = sheep.sheepType;
            }
        }

        // visual
        for (int i = 0; i < nChilds; i++)
        {
            var obj = SheepUIPanel.GetChild(i).gameObject.GetComponent<SheepButton>();
            if (obj.sheep.UUID == selectedSheep.UUID)
            {
                obj.SetInfo(sheep, this);
                break;
            }
        }
    }

    public void SetupDetailsPanel()
    {
        Sex[] valsSex = (Sex[])Enum.GetValues(typeof(Sex));
        SheepType[] valsSheepType = (SheepType[])Enum.GetValues(typeof(SheepType));

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (var val in valsSex)
        {
            options.Add(new TMP_Dropdown.OptionData(val.ToString()));
        }

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
            AnotherFuckingFunc();
            //calendarWidget.SetDate(tmpTime.ToUnixTimeSeconds());
            //calendarWidget.SetDate(tmpTime.ToUnixTimeSeconds());
            //UpdateCalendar();
        });
    }

    private void AnotherFuckingFunc()
    {
        calendarWidget.SetDate(tmpTime.ToUnixTimeSeconds());
    }

    public void ShowDetails(SheepObject sheep)
    {
        selectedSheep = sheep;
        Debug.Log("showdetails");
        overviewPanel.SetActive(false);
        detailsPanel.SetActive(true);
        inputUUID.SetTextWithoutNotify(sheep.UUID);
        tmpTime = DateTimeOffset.FromUnixTimeSeconds(sheep.tsBorn);

        for (int i = 0; i < inputSex.options.Count; i++)
        {
            if (inputSex.options[i].text.ToLower() == sheep.sex.ToString().ToLower())
            {
                inputSex.value = i;
                break;
            }
        }

        for (int i = 0; i < inputSheepType.options.Count; i++)
        {
            if (inputSheepType.options[i].text.ToLower() == sheep.sheepType.ToString().ToLower())
            {
                inputSheepType.value = i;
                break;
            }
        }

        // this can also be done with tsborntime.tostring() but it will be much longer
        DateTimeOffset tsBornTime = DateTimeOffset.FromUnixTimeSeconds(sheep.tsBorn);
        UpdateTSButton(tsBornTime);
        //string tsBornString = tsBornTime.Day + "-" + tsBornTime.Month + "-" + tsBornTime.Year;
        //inputTSBorn.GetComponentInChildren<TextMeshProUGUI>().SetText(tsBornString);
    }

    private void UpdateDatabase()
    {

    }

    private void Update()
    {
        if (writeToFile)
        {
            writeToFile = false;
            UpdateDatabase();
            var sheepDB = SheepDatabase.ToArray();
            Debug.Log("sheepdblength = " + sheepDB.Length);
            WurmFileHandler.WriteDataToCsvFile("TESTSHEEPDATABASE", sheepDB, false);
        }
    }

    /// <summary>
    /// Loads data from a file into the database. The extension must be capitalized and contained in the filename if it is not a JSON file.
    /// </summary>
    /// /// <param name="inputFile"></param>
    private void LoadSheepData(TextAsset inputFile)
    {
        if (inputFile == null)
        {
            Debug.LogError("Sheep input data file missing!");
            return;
        }

        if (inputFile.name.Contains("CSV"))
        {
            LoadSheepDataFromCsvFile(inputFile);
        }

        // Assume it is a JSON file if no capitalized extension is present in the filename
        else
        {
            LoadSheepDataFromJsonFile(inputFile);
        }
    }

    /// <summary>
    /// Loads sheep data from a JSON file into the database.
    /// </summary>
    /// <param name="inputFile"></param>
    private void LoadSheepDataFromJsonFile(TextAsset inputFile)
    {
        //allSheep = JsonUtility.FromJson<SheepArray>(inputFile.text);
    }

    /// <summary>
    /// Loads sheep data from a CSV file into the database.
    /// </summary>
    /// <param name="inputFile"></param>
    private void LoadSheepDataFromCsvFile(TextAsset inputFile)
    {
        SheepDatabase = WurmFileHandler.GetDataFromCsvFile<SheepObject>(inputFile);
    }
}
