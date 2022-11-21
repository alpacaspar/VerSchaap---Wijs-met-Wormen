using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;


public class SheepDataReader : MonoBehaviour
{
    public TextAsset sheepDataFile;
    public SheepArray allSheep;

    public List<SheepObject> SheepDatabase = new List<SheepObject>();

    // dummy var to test in the editor
    public bool writeToFile;

    private void Start()
    {
        LoadSheepData(sheepDataFile);
        UpdateDatabase();
        //Sheep DummySheep = new Sheep();
        //var output = WurmAPI.MethodHandler<Sheep>(MethodType.Get, DummySheep);
        //Debug.Log(output);
    }

    private void UpdateDatabase()
    {
        SheepDatabase.Clear();
        foreach (var s in allSheep.sheep)
        {
            SheepObject databaseSheep = ConvertSheepClassToSheepObject(s);
            SheepDatabase.Add(databaseSheep);
        }
    }

    /// <summary>
    /// Converts a 'Sheep' object to a 'SheepObject' object
    /// </summary>
    /// <param name="inputSheep"></param>
    /// <returns></returns>
    private SheepObject ConvertSheepClassToSheepObject(Sheep inputSheep)
    {
        SheepObject sheep = new SheepObject();
        sheep.sheepUUID = inputSheep.uuid;
        sheep.tsBorn = inputSheep.tsborn;

        // Timestamp should be a long for this to work
        //int currentTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        int currentTime = 0;

        // Sheep weigth
        SheepWeight weigth = new SheepWeight();
        weigth.timestamp = currentTime;
        weigth.weight = inputSheep.weight;
        sheep.weight = new List<SheepWeight>();
        sheep.weight.Add(weigth);

        sheep.sex = inputSheep.gender;
        sheep.sheepType = inputSheep.species;

        SheepDiseases diseases = new SheepDiseases();
        diseases.timestamp = currentTime;
        diseases.diseases = inputSheep.diseases;
        sheep.diseases = new List<SheepDiseases>();
        sheep.diseases.Add(diseases);
        
        //sheep.extraRemarks
        return sheep;
    }

    private void Update()
    {
        if (writeToFile)
        {
            writeToFile = false;
            UpdateDatabase();
            WurmFileHandler.WriteDataToCsvFile("TESTSHEEPDATABASE", allSheep.sheep, false);
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
        allSheep = JsonUtility.FromJson<SheepArray>(inputFile.text);
    }

    /// <summary>
    /// Loads sheep data from a CSV file into the database.
    /// </summary>
    /// <param name="inputFile"></param>
    private void LoadSheepDataFromCsvFile(TextAsset inputFile)
    {
        List<Sheep> sheepList = WurmFileHandler.GetDataFromCsvFile<Sheep>(inputFile);
        allSheep.sheep = sheepList.ToArray();
    }
}
