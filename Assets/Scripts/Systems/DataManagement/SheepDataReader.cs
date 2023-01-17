using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SheepDataReader : MonoBehaviour
{
    public TextAsset sheepDataFile;
    public SheepDataViewer sheepDataViewer;
    public WormDataViewer wormDataViewer;
    public WeideDataViewer weideDataViewer;
    public KoppelDataViewer koppelDataViewer;
    public TemporalDatabaseData testDatabase;

    public GameObject pnlDelete;
    public Button btnDeleteConfirm;
    public Button btnDeleteCancel;

    public bool bConfirmToDelete;
    public Toggle bDeleteToggle;

    public void OpenFileExplorer()
    {
        var paths = SFB.StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
    }

    private void Start()
    {
        btnDeleteCancel.onClick.AddListener(delegate { pnlDelete.SetActive(false); });

        sheepDataViewer = GetComponent<SheepDataViewer>();
        wormDataViewer = GetComponent<WormDataViewer>();
        weideDataViewer = GetComponent<WeideDataViewer>();
        koppelDataViewer = GetComponent<KoppelDataViewer>();

        koppelDataViewer.dataReader = this;
        wormDataViewer.dataReader = this;
        sheepDataViewer.sheepDataReader = this;
        weideDataViewer.sheepDataReader = this;
        
        LoadSheepData(sheepDataFile);
        OnDatabaseLoaded();
    }

    /// <summary>
    /// Creates the buttons for all elements in the database when it has finished loading
    /// </summary>
    public void OnDatabaseLoaded()
    {
        koppelDataViewer.CreateButtonsFromDB(testDatabase.sheepKoppels);
        wormDataViewer.CreateWormButtonsFromDB(testDatabase.worms);
        sheepDataViewer.CreateSheepButtonsFromDB(testDatabase.sheeps);
        weideDataViewer.CreateSheepButtonsFromDB(testDatabase.weides);
    }

    private T GetElementByUUID<T>(string uuid, List<T> list) where T : ObjectUUID
    {
        foreach (var element in list)
        {
            if (element.UUID == uuid) return element;
        }

        return null;
    }

    public SheepObject GetSheepObjectByUUID(string uuid)
    {
        return GetElementByUUID(uuid, testDatabase.sheeps);
    }

    public SheepKoppel GetSheepKoppelByUUID(string uuid)
    {
        return GetElementByUUID(uuid, testDatabase.sheepKoppels);
    }

    public WeideObject GetWeideObjectByUUID(string uuid)
    {
        return GetElementByUUID(uuid, testDatabase.weides);
    }

    public string GetKoppelNameByUUID(string uuid)
    {
        SheepKoppel kop = GetSheepKoppelByUUID(uuid);
        if (kop != null) return kop.koppelName;
        return null;
    }

    public string GetKoppelUUIDByName(string name)
    {
        foreach (var koppel in testDatabase.sheepKoppels)
        {
            if (koppel.koppelName == name) return koppel.UUID;
        }

        return null;
    }

    public void UpdateSheepData(SheepObject sheep)
    {
        int nChilds = sheepDataViewer.sheepButtonContainer.childCount;

        // editing existing sheep
        if (sheepDataViewer.panelMode == DetailsPanelMode.EditingElement)
        //if (!sheepDataViewer.bAddingSheep)
        {
            // update the actual data
            var shp = GetSheepObjectByUUID(sheepDataViewer.selectedSheep.UUID);

            if (shp != null)
            {
                shp.sheepTag = sheep.sheepTag;
                shp.sex = sheep.sex;
                shp.sheepType = sheep.sheepType;
                shp.tsBorn = sheep.tsBorn;

                var oldKoppelID = shp.sheepKoppelID;
                var newKoppelID = sheep.sheepKoppelID;
                // TODO check if it is a valid koppelid

                if (oldKoppelID != newKoppelID)
                {
                    // Remove the sheep from the old koppel
                    SheepKoppel oldKop = GetSheepKoppelByUUID(oldKoppelID);
                    oldKop?.allSheep.Remove(sheep.UUID);

                    // Add the sheep to the new koppel
                    SheepKoppel newKop = GetSheepKoppelByUUID(newKoppelID);
                    newKop?.allSheep.Add(shp.UUID);
                }

                shp.sheepKoppelID = newKoppelID;
            }

            // update the visuals representing the data
            for (int i = 0; i < nChilds; i++)
            {
                var obj = sheepDataViewer.sheepButtonContainer.GetChild(i).gameObject.GetComponentInChildren<SheepButton>();
                if (obj.sheep.UUID != sheepDataViewer.selectedSheep.UUID) continue;
                obj.SetInfo(sheep, sheepDataViewer);
                break;
            }
        }

        // Adding a new sheep
        // TODO check if UUID doesnt already exist
        else
        {
            testDatabase.sheeps.Add(sheep);
            var obj = sheepDataViewer.CreateNewButton(sheep);
            sheepDataViewer.MoveScrollViewToElement(obj.GetComponent<RectTransform>());
        }

        sheepDataViewer.panelMode = DetailsPanelMode.None;
        //sheepDataViewer.bAddingSheep = false;
    }

    public void UpdateWeideData(WeideObject weide)
    {
        int nChilds = sheepDataViewer.sheepButtonContainer.childCount;

        // editing existing weide
        if (weideDataViewer.panelMode == DetailsPanelMode.EditingElement)
        //if (!weideDataViewer.bAddingElement)
        {
            // update the actual data
            WeideObject wds = GetWeideObjectByUUID(weideDataViewer.selectedElement.UUID);
            
            if (wds != null)
            {
                wds.perceelName = weide.perceelName;
                wds.surfaceQuality = weide.surfaceQuality;
                wds.surfaceSqrMtr = weide.surfaceSqrMtr;
            }

            // update the visuals representing the data
            for (int i = 0; i < nChilds; i++)
            {
                var obj = weideDataViewer.WeideButtonContainer.GetChild(i).gameObject.GetComponentInChildren<WeideButton>();
                if (obj.weide.UUID != weideDataViewer.selectedElement.UUID) continue;
                obj.SetInfo(weide, weideDataViewer);
                break;
            }
        }

        // Adding a new weide
        // TODO check if UUID doesnt already exist
        else
        {
            testDatabase.weides.Add(weide);
            weideDataViewer.CreateNewButton(weide);
        }

        weideDataViewer.panelMode = DetailsPanelMode.None;
        //weideDataViewer.bAddingElement = false;
    }

    private bool DeleteElement<T>(T element, List<T> list, DataViewer dataViewer = null) where T : ObjectUUID
    {
        int index = -1;

        for (int i = 0; i < list.Count; i++)
        {
            var tmp = list[i];
            if (tmp.UUID.Trim() != element.UUID.Trim()) continue;
            index = i;
            break;
        }

        if (index == -1) return false;
        list.RemoveAt(index);

        // Make the dataviewer remove the button associated with the object
        if (dataViewer != null) dataViewer.RemoveButton(element);
        return true;
    }

    public void DeleteSheep(SheepObject sheep)
    {
        DeleteElement(sheep, testDatabase.sheeps, sheepDataViewer);
    }

    public void DeleteKoppel(SheepKoppel koppel)
    {
        if (DeleteElement(koppel, testDatabase.sheepKoppels, koppelDataViewer))
        {
            // Set koppelID to "" for all sheeps using this koppel
            foreach (var sheep in testDatabase.sheeps.Where(sheep => string.Equals(sheep.sheepKoppelID, koppel.UUID, StringComparison.CurrentCultureIgnoreCase)))
            {
                sheep.sheepKoppelID = "";
            }
        }
    }

    public void DeleteButtonClicked(ObjectUUID obj)
    {
        bConfirmToDelete = bDeleteToggle.isOn;

        UnityEngine.Events.UnityAction sheepDeleteEvent = delegate { DeleteSheep(obj as SheepObject); };
        UnityEngine.Events.UnityAction weideDeleteEvent = delegate { DeleteWeide(obj as WeideObject); };
        UnityEngine.Events.UnityAction sheepKoppelDeleteEvent = delegate { DeleteKoppel(obj as SheepKoppel); };

        if (bConfirmToDelete)
        {
            pnlDelete.SetActive(true);
            btnDeleteConfirm.onClick.RemoveAllListeners();

            switch (obj)
            {
                case SheepObject sheepObject:
                    btnDeleteConfirm.onClick.AddListener(sheepDeleteEvent);
                    break;
                case WeideObject weideObject:
                    btnDeleteConfirm.onClick.AddListener(weideDeleteEvent);
                    break;
                case SheepKoppel sheepKoppel:
                    btnDeleteConfirm.onClick.AddListener(sheepKoppelDeleteEvent);
                    break;

            }

            btnDeleteConfirm.onClick.AddListener(delegate { pnlDelete.SetActive(false); });
        }

        else
        {
            switch (obj)
            {
                case SheepObject sheepObject:
                    sheepDeleteEvent.Invoke();
                    break;
                case WeideObject weideObject:
                    weideDeleteEvent.Invoke();
                    break;
                case SheepKoppel sheepKoppel:
                    sheepKoppelDeleteEvent.Invoke();
                    break;
            }
        }
    }

    public void DeleteWeide(WeideObject weide)
    {
        DeleteElement(weide, testDatabase.weides, weideDataViewer);
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
        testDatabase = JsonUtility.FromJson<TemporalDatabaseData>(inputFile.text);
        
        // Assumes the timestamp is in nanoseconds and converts it to seconds
        foreach (var s in testDatabase.sheeps)
        {
            s.tsBorn /= 1000000000;
        }
    }

    /// <summary>
    /// Loads sheep data from a CSV file into the database.
    /// </summary>
    /// <param name="inputFile"></param>
    private void LoadSheepDataFromCsvFile(TextAsset inputFile)
    {
        testDatabase.sheeps = WurmFileHandler.GetDataFromCsvFile<SheepObject>(inputFile);
    }
}
