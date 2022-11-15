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

public class TemporalDatabaseData
{
    public WeideObject[] weides;
    public SheepObject[] sheeps;
    public WormObject[] worms;
}

public struct WeideObject
{
    public string weideID;
    public int surfaceSqrMtr;
    public float surfaceQuality;
    public GrassType[] grassTypes;
    public SheepType[] currentSheeps;
    public string[] extraRemarks;
}

public struct SheepObject
{
    public string sheepID;
    public int tsBorn; // time stamp date of birth
    public float weight;
    public Sex sex;
    public SheepType sheepType;
    public Decease[] deceases;
    public string[] extraRemarks;
}

public struct WormObject
{
    public WormType wormType; 
    public Medicine[] effectiveMedicines;
    public Medicine[] resistences;
    public Symptom[] symptoms;
    public Conditions[] faveConditions;
    public string[] extraRemarks;
}
