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
        ProgressData(MethodType.Put, data);
        //newData[0] = UpdateDatabase(ProgressData(MethodType.Put, data));

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

        tempDatabase = data;
        // save new values

        return code;
    }

    /// <summary>
    ///     Used to correctly digest the values
    /// </summary>
    /// <param name="data">
    ///     The new values
    /// </param>
    /// <returns>
    ///     Http status
    /// </returns>
    public static string[] ProgressData<ObjectInterface>(MethodType type, ObjectInterface data)
    {
        string[] newData = new string[] { Status.Error1.ToString(), "" };

        switch (data)
        {
            case TemporalDatabaseData newDatabase:
                // TODO handle proper request
                break;

            case WeideObject newWeideObject:
                if (tempDatabase.weides.Length <= 0) break; // TODO catch empty obj and fire post request?

                foreach(WeideObject obj in tempDatabase.weides)
                {
                    if (obj.weideUUID == newWeideObject.weideUUID)
                    {
                        obj.surfaceSqrMtr = (newWeideObject.surfaceSqrMtr != obj.surfaceSqrMtr) ? newWeideObject.surfaceSqrMtr : obj.surfaceSqrMtr;
                        obj.surfaceQuality = (newWeideObject.surfaceQuality != obj.surfaceQuality) ? newWeideObject.surfaceQuality : obj.surfaceQuality;
                        obj.grassTypes = (newWeideObject.grassTypes != obj.grassTypes) ? newWeideObject.grassTypes : obj.grassTypes;
                        obj.currentSheeps = (newWeideObject.currentSheeps != obj.currentSheeps) ? newWeideObject.currentSheeps : obj.currentSheeps;
                        obj.extraRemarks = (newWeideObject.extraRemarks != obj.extraRemarks) ? newWeideObject.extraRemarks : obj.extraRemarks;
                    }
                }
                // TODO fire request and receive http code
                break;

            case SheepObject newSheepObject:
                // TODO handle proper request
                break;

            case WormObject newWormObject:
                // TODO handle proper request
                break;
        }

        switch (type)
        {
            case MethodType.Put:

                break;
        }

        // TODO, check if entry exists by UUID otherwise convert to post request
        newData[0] = UpdateDatabase(new TemporalDatabaseData()).ToString();

        return newData;
    }
}

public interface ObjectInterface
{

}

[Serializable]
public class TemporalDatabaseData : ObjectInterface
{
    public string farmerName;
    public string farmerUUID;
    public WeideObject[] weides;
    public SheepObject[] sheeps;
    public WormObject[] worms;
}

[Serializable]
public class WeideObject : ObjectInterface
{
    public string weideUUID;
    public int surfaceSqrMtr;
    public float surfaceQuality;
    public GrassType[] grassTypes;
    public SheepType[] currentSheeps;
    public string[] extraRemarks;
}

[Serializable]
public class SheepObject : ObjectInterface
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
public class WormObject : ObjectInterface
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
