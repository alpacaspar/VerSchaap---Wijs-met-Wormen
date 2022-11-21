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

        if (type == MethodType.Put)
        {
            switch (data)
            {
                case TemporalDatabaseData newDatabase:
                    if (tempDatabase.farmerUUID == newDatabase.farmerUUID)
                    {
                        tempDatabase.farmerName = (tempDatabase.farmerName != newDatabase.farmerName) ? newDatabase.farmerName : tempDatabase.farmerName;

                        // TODO iterate through the following to catch each individual change as an update
                        tempDatabase.weides = (tempDatabase.weides != newDatabase.weides) ? newDatabase.weides : tempDatabase.weides;
                        tempDatabase.sheeps = (tempDatabase.sheeps != newDatabase.sheeps) ? newDatabase.sheeps : tempDatabase.sheeps;
                        tempDatabase.worms = (tempDatabase.worms != newDatabase.worms) ? newDatabase.worms : tempDatabase.worms;
                    }
                    break;

                case WeideObject newWeideObject:
                    bool handledData = false;

                    if (tempDatabase.weides.Count > 0)
                    {
                        foreach (WeideObject obj in tempDatabase.weides)
                        {
                            if (obj.weideUUID == newWeideObject.weideUUID)
                            {
                                obj.surfaceSqrMtr = (newWeideObject.surfaceSqrMtr != obj.surfaceSqrMtr) ? newWeideObject.surfaceSqrMtr : obj.surfaceSqrMtr;
                                obj.surfaceQuality = (newWeideObject.surfaceQuality != obj.surfaceQuality) ? newWeideObject.surfaceQuality : obj.surfaceQuality;
                                obj.grassTypes = (newWeideObject.grassTypes != obj.grassTypes) ? newWeideObject.grassTypes : obj.grassTypes;
                                obj.currentSheeps = (newWeideObject.currentSheeps != obj.currentSheeps) ? newWeideObject.currentSheeps : obj.currentSheeps;
                                obj.extraRemarks = (newWeideObject.extraRemarks != obj.extraRemarks) ? newWeideObject.extraRemarks : obj.extraRemarks;

                                handledData = true;
                                newData[0] = Status.Success2.ToString();
                            }
                        }
                    }

                    if (!handledData)
                    {
                        // entry does not exist so add new one
                        newData = ProgressData(MethodType.Post, data);
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

            WriteDatabase(tempDatabase);
        }
        else if (type == MethodType.Post)
        {

            WriteDatabase(tempDatabase);
        } 
        else if (type == MethodType.Get)
        {

        }
        else
        {
            newData[0] = Status.Failure0.ToString();
        }

        return newData;
    }

    /// <summary>
    ///     Used to save the new values
    /// </summary>
    /// <param name="data">
    ///     The updated version of the database
    /// </param>
    private static void WriteDatabase(TemporalDatabaseData data)
    {
        // save new values
    }
}

public interface ObjectInterface { }

[Serializable]
public class TemporalDatabaseData : ObjectInterface
{
    public string farmerName;
    public string farmerUUID;
    public List<WeideObject> weides;
    public List<SheepObject> sheeps;
    public List<WormObject> worms;
}

[Serializable]
public class WeideObject : ObjectInterface
{
    public string weideUUID;
    public int surfaceSqrMtr;
    public float surfaceQuality;
    public List<GrassType[]> grassTypes;
    public List<SheepType[]> currentSheeps;
    public List<string> extraRemarks;
}

[Serializable]
public class SheepObject : ObjectInterface
{
    public string sheepUUID;
    public long tsBorn; // time stamp date of birth
    public List<SheepWeight> weight;
    public List<SheepDiseases> diseases;
    public Sex sex;
    public SheepType sheepType;
    public List<string> extraRemarks;
}

[Serializable]
public class WormObject : ObjectInterface
{
    public string wormUUID;
    public WormType wormType; 
    public List<Medicine> effectiveMedicines;
    public List<Medicine> resistences;
    public List<Symptom> symptoms;
    public List<Condition> faveConditions;
    public List<string> extraRemarks;
}

[Serializable]
public struct SheepWeight
{
    public float weight;
    public long timestamp;
}

[Serializable]
public struct SheepDiseases
{
    public Disease[] diseases;
    public long timestamp;
}
