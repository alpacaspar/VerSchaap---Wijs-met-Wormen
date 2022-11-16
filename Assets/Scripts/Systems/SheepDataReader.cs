using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;


public class SheepDataReader : MonoBehaviour
{
    public TextAsset sheepDataFile;
    public SheepArray allSheep;

    public bool writeToFile;

    private void Start()
    {
        LoadSheepData(sheepDataFile);
        //WurmFileHandler.WriteDataToCsvFile("TESTSHEEPDATABASE", allSheep.sheep, false);
        //PrintDatabase();
    }

    private void Update()
    {
        if (writeToFile)
        {
            writeToFile = false;
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

        else
        {
            LoadSheepDataFromJsonFile(inputFile);
        }
    }

    /// <summary>
    /// Outputs the database to the console.
    /// </summary>
    private void PrintDatabase()
    {
        foreach (Sheep sheep in allSheep.sheep)
        {
            sheep.Print();
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
