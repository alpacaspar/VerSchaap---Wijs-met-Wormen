using System;
using System.Collections.Generic;
using UnityEngine;

public static class Database
{
    private static TemporalDatabaseData tempDatabase;

    /// <summary>
    ///     Load database
    /// </summary>
    public static void InitializeDatabase()
    {
        // load temp database upon startup

        tempDatabase = new TemporalDatabaseData();
    }

    /// <summary>
    ///     Get request
    /// </summary>
    /// <param name="data">
    ///     Generic type that contains keys and info
    /// </param>
    /// <returns>
    ///     Http status type and data content
    /// </returns>
    public static string[] GetData<T>(T data)
    {
        string[] newData = new string[] { Status.Error1.ToString(), "" };

        // TODO, iterate somehow through generic in order to find the corresponding type and return the value assigned to it
        newData[0] = UpdateDatabase(new TemporalDatabaseData()).ToString();

        return newData;
    }

    /// <summary>
    ///     Post request
    /// </summary>
    /// <param name="data">
    ///     Generic type that contains keys and info
    /// </param>
    /// <returns>
    ///     Http status type and data content
    /// </returns>
    public static string[] PostData<T>(T data)
    {
        string[] newData = new string[] { Status.Error1.ToString(), "" };

        newData[0] = UpdateDatabase(new TemporalDatabaseData()).ToString();

        return newData;
    }

    /// <summary>
    ///     Put request
    /// </summary>
    /// <param name="data">
    ///     Generic type that contains keys and info
    /// </param>
    /// <returns>
    ///     Http status type and data content
    /// </returns>
    public static string[] PutData<T>(T data)
    {
        string[] newData = new string[] { Status.Error1.ToString(), "" };

        // TODO, check if entry exists by UUID otherwise convert to post request
        newData[0] = UpdateDatabase(new TemporalDatabaseData()).ToString();

        return newData;
    }

    /// <summary>
    ///     Used to update the database values
    /// </summary>
    /// <param name="data">
    ///     The new values
    /// </param>
    /// <returns>
    ///     Http status
    /// </returns>
    private static Status UpdateDatabase(TemporalDatabaseData data)
    {
        Status code = Status.Success0;

        // save new values

        return code;
    }
}

[Serializable]
public class TemporalDatabaseData
{
    public string farmerName;
    public string farmerUUID;
    public WeideObject[] weides;
    public SheepObject[] sheeps;
    public WormObject[] worms;
}

[Serializable]
public struct WeideObject
{
    public string weideUUID;
    public int surfaceSqrMtr;
    public float surfaceQuality;
    public GrassType[] grassTypes;
    public SheepType[] currentSheeps;
    public string[] extraRemarks;
}

[Serializable]
public struct SheepObject
{
    public string sheepUUID;
    public int tsBorn; // time stamp date of birth
    public List<SheepWeight> weight;
    public List<SheepDiseases> diseases;
    public Sex sex;
    public SheepType sheepType;
    public string[] extraRemarks;
}

[Serializable]
public struct WormObject
{
    public string wormUUID;
    public WormType wormType; 
    public Medicine[] effectiveMedicines;
    public Medicine[] resistences;
    public Symptom[] symptoms;
    public Condition[] faveConditions;
    public string[] extraRemarks;
}


[Serializable]
public struct SheepWeight
{
    public float weight;
    public int timestamp;
}

[Serializable]
public struct SheepDiseases
{
    public Disease[] diseases;
    public int timestamp;
}
