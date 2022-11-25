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
    public Button btnAddSheep;

    // dummy var to test in the editor
    public bool writeToFile;
    public SheepObject selectedSheep;

    bool bAddingSheep = false;

    // dirty var to fix the calendar
    public DateTimeOffset tmpTime = new DateTimeOffset(DateTime.UtcNow);

    private void CreateNewSheepButton(SheepObject s)
    {
        var sheepPanelGameObject = Instantiate(SheepUIObject, SheepUIPanel);
        sheepPanelGameObject.GetComponentInChildren<SheepButton>().SetInfo(s, this);
    }

    private void DBTest()
    {
        // boot up the database
        Database.InitializeDatabase();

        // prepare new data
        WeideObject weideObject = new WeideObject();
        weideObject.surfaceQuality = 69;
        string[] response = WurmAPI.MethodHandler(MethodType.Put, weideObject);
        // prints the received code
        Debug.Log(Helpers.HttpMessage[(Status)int.Parse(response[0])]);
    }

    private void Start()
    {
        DBTest();
        calendarWidget.sheepDataReader = this;

        SetupDetailsPanel();
        LoadSheepData(sheepDataFile);

        foreach (var s in SheepDatabase)
        {
            CreateNewSheepButton(s);
        }

        btnCancel.onClick.AddListener(delegate
        {
            overviewPanel.SetActive(true);
            detailsPanel.SetActive(false);
            bAddingSheep = false;
        });

        btnAddSheep.onClick.AddListener(delegate
        {
            bAddingSheep = true;
            selectedSheep = new SheepObject();
            ShowDetails(selectedSheep);
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

        //TODO do something different if adding a new sheep
        // editing existing sheep
        if (!bAddingSheep)
        {
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
                var obj = SheepUIPanel.GetChild(i).gameObject.GetComponentInChildren<SheepButton>();
                if (obj.sheep.UUID == selectedSheep.UUID)
                {
                    obj.SetInfo(sheep, this);
                    break;
                }
            }
        }

        // TODO check if UUID doesnt already exist
        else
        {
            SheepDatabase.Add(sheep);
            CreateNewSheepButton(sheep);
        }

        bAddingSheep = false;
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
    }

    public void DeleteSheep(SheepObject sheep)
    {
        int index = -1;
        // the actual data
        for (int i = 0; i < SheepDatabase.Count; i++)
        {
            var shp = SheepDatabase[i];
            if (shp.UUID.Trim() == sheep.UUID.Trim())
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            Destroy(SheepUIPanel.GetChild(index).gameObject);
            SheepDatabase.RemoveAt(index);
        }
    }

    private void Update()
    {
        if (writeToFile)
        {
            writeToFile = false;
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
