using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class SheepDataReader : MonoBehaviour
{
    public TextAsset sheepDataFile;
    private SheepDataViewer sheepDataViewer;
    private WormDataViewer wormDataViewer;
    //public ObjectDetailsEditor detailsEditor;
    public TemporalDatabaseData testDatabase;

    // dummy var to `write from the editor
    public bool writeToFile;

    public void OpenFileExplorer()
    {
        var paths = SFB.StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
    }

    private void Start()
    {
        sheepDataViewer = GetComponent<SheepDataViewer>();
        wormDataViewer = GetComponent<WormDataViewer>();
        
        if (wormDataViewer != null)
        {
            wormDataViewer.dataReader = this;
            wormDataViewer.CreateWormButtonsFromDB(testDatabase.worms);
        }
        
        sheepDataViewer.sheepDataReader = this;
        LoadSheepData(sheepDataFile);
        sheepDataViewer.CreateSheepButtonsFromDB(testDatabase.sheeps);

    }

    public void UpdateSheepData(SheepObject sheep)
    {
        int nChilds = sheepDataViewer.sheepUIPanel.childCount;

        // editing existing sheep
        if (!sheepDataViewer.bAddingSheep)
        {
            // update the actual data
            // magic, ignore casing and check if names are the same
            foreach (var shp in testDatabase.sheeps.Where(shp => string.Equals(shp.UUID, sheepDataViewer.selectedSheep.UUID, StringComparison.CurrentCultureIgnoreCase)))
            {
                shp.UUID = sheep.UUID;
                shp.sex = sheep.sex;
                shp.sheepType = sheep.sheepType;
                shp.tsBorn = sheep.tsBorn;
            }

            // update the visuals representing the data
            for (int i = 0; i < nChilds; i++)
            {
                var obj = sheepDataViewer.sheepUIPanel.GetChild(i).gameObject.GetComponentInChildren<SheepButton>();
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
            sheepDataViewer.CreateNewSheepButton(sheep);
        }

        sheepDataViewer.bAddingSheep = false;
    }

    public void UpdateWormData(WormObject worm)
    {
        int nChilds = wormDataViewer.wormUIPanel.childCount;

        // editing existing sheep
        if (!wormDataViewer.bAddingSheep)
        {
            // update the actual data
            // magic, ignore casing and check if names are the same
            foreach (var wrm in testDatabase.worms.Where(wrm => string.Equals(wrm.UUID, wormDataViewer.selectedWorm.UUID, StringComparison.CurrentCultureIgnoreCase)))
            {
                wrm.UUID = worm.UUID;
                wrm.wormType = worm.wormType;
            }

            // update the visuals representing the data
            for (int i = 0; i < nChilds; i++)
            {
                var obj = wormDataViewer.wormUIPanel.GetChild(i).gameObject.GetComponentInChildren<WormButton>();
                if (obj.worm.UUID != wormDataViewer.selectedWorm.UUID) continue;
                obj.SetInfo(worm, wormDataViewer);
                break;
            }
        }

        // Adding a new worm
        // TODO check if UUID doesnt already exist
        else
        {
            testDatabase.worms.Add(worm);
            wormDataViewer.CreateNewWormButton(worm);
        }

        sheepDataViewer.bAddingSheep = false;
    }

    public void DeleteSheep(SheepObject sheep)
    {
        int index = -1;
        
        for (int i = 0; i < testDatabase.sheeps.Count; i++)
        {
            var shp = testDatabase.sheeps[i];
            if (shp.UUID.Trim() != sheep.UUID.Trim()) continue;
            index = i;
            break;
        }

        if (index == -1) return;
        Destroy(sheepDataViewer.sheepUIPanel.GetChild(index).gameObject);
        testDatabase.sheeps.RemoveAt(index);
    }

    public void DeleteWorm(WormObject worm)
    {
        int index = -1;

        for (int i = 0; i < testDatabase.worms.Count; i++)
        {
            var wrm = testDatabase.worms[i];
            if (wrm.UUID.Trim() != worm.UUID.Trim()) continue;
            index = i;
            break;
        }

        if (index == -1) return;
        Destroy(wormDataViewer.wormUIPanel.GetChild(index).gameObject);
        testDatabase.worms.RemoveAt(index);
    }

    private void Update()
    {
        if (writeToFile)
        {
            writeToFile = false;
            var sheepDB = testDatabase.sheeps.ToArray();
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
        testDatabase = JsonUtility.FromJson<TemporalDatabaseData>(inputFile.text);
        
        // TODO fix the timestamp calculation, because this assumes the input is in nano seconds and converts it to seconds
        foreach (var s in testDatabase.sheeps)
        {
            s.tsBorn /= 1000000000;
        }
        //allSheep = JsonUtility.FromJson<SheepArray>(inputFile.text);
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
