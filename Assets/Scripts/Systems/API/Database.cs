using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Database
{
    private static TemporalDatabaseData tempDatabase = null;

    private static string filePath = Application.persistentDataPath + "/database.xml";

    public static List<string> requests = new List<string>();

    /// <summary>
    ///     Load database
    /// </summary>
    public static void InitializeDatabase()
    {
        // load temp database upon startup
        tempDatabase = LoadDatabase();
    }

    public static TemporalDatabaseData GetDatabase()
    {
        if (tempDatabase == null) InitializeDatabase();

        return tempDatabase;
    }

    public static List<WormObject> GetWormCollection()
    {
        if (tempDatabase == null) InitializeDatabase();

        return tempDatabase.worms;
    }

    public static List<WeideObject> GetWeideCollection()
    {
        if (tempDatabase == null) InitializeDatabase();

        return tempDatabase.weides;
    }

    public static List<SheepObject> GetSheepCollection()
    {
        if (tempDatabase == null) InitializeDatabase();

        return tempDatabase.sheeps;
    }

    public static List<SheepKoppel> GetSheepKoppelCollection()
    {
        if (tempDatabase == null) InitializeDatabase();

        return tempDatabase.sheepKoppels;
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
    public static string[] ProgressData<IObject>(MethodType type, IObject data)
    {
        if (tempDatabase == null) InitializeDatabase();

        string[] newData = new string[] { (int)Status.Error1 + "", "" };
        bool handledData = false;

        if (type == MethodType.Put)
        {
            switch (data)
            {
                case TemporalDatabaseData:
                        // we are not going to do this method type
                        newData[0] = (int)Status.Failure5 + "";
                    break;

                case WeideObject newWeideObject:
                    handledData = false;

                    if (tempDatabase.weides.Count > 0)
                    {
                        foreach (WeideObject obj in tempDatabase.weides)
                        {
                            if (obj.UUID == newWeideObject.UUID)
                            {
                                obj.surfaceSqrMtr = (newWeideObject.surfaceSqrMtr != obj.surfaceSqrMtr) ? newWeideObject.surfaceSqrMtr : obj.surfaceSqrMtr;
                                obj.surfaceQuality = (newWeideObject.surfaceQuality != obj.surfaceQuality) ? newWeideObject.surfaceQuality : obj.surfaceQuality;
                                obj.grassTypes = (newWeideObject.grassTypes != obj.grassTypes) ? newWeideObject.grassTypes : obj.grassTypes;
                                obj.currentSheeps = (newWeideObject.currentSheeps != obj.currentSheeps) ? newWeideObject.currentSheeps : obj.currentSheeps;
                                obj.extraRemarks = (newWeideObject.extraRemarks != obj.extraRemarks) ? newWeideObject.extraRemarks : obj.extraRemarks;

                                handledData = true;
                                newData[0] = (int)Status.Success2 + "";
                            }
                        }
                    }

                    if (!handledData)
                    {
                        // entry does not exist
                        newData[0] = (int)Status.Failure4 + "";
                    }
                    break;

                case SheepObject newSheepObject:
                    handledData = false;

                    if (tempDatabase.sheeps.Count > 0)
                    {
                        foreach (SheepObject obj in tempDatabase.sheeps)
                        {
                            if (obj.UUID == newSheepObject.UUID)
                            {
                                // add new weight -- ALL WEIGHT DATA MUST BE NEW DATA
                                for (int i = 0; i < newSheepObject.weight.Count; i++)
                                {
                                    obj.weight.Add(newSheepObject.weight[i]);
                                    // TODO query per weight
                                }
                                // add new diseases collection -- ALL DISEASES MUST BE NEW DISEASES
                                for (int i = 0; i < newSheepObject.diseases.Count; i++)
                                {
                                    obj.diseases.Add(newSheepObject.diseases[i]);
                                    // TODO query per disease
                                }

                                obj.tsBorn = (newSheepObject.tsBorn != obj.tsBorn && newSheepObject.tsBorn != 0) ? newSheepObject.tsBorn : obj.tsBorn;
                                obj.extraRemarks = (newSheepObject.extraRemarks != obj.extraRemarks) ? newSheepObject.extraRemarks : obj.extraRemarks;

                                handledData = true;
                                newData = UpdateEntry(obj, Helpers.SheepToUUID(tempDatabase.sheeps), new SheepObject().GetType());

                                newData[0] = (int)Status.Success2 + ""; // TODO set status waiting
                            }
                        }
                    }

                    if (!handledData)
                    {
                        // entry does not exist
                        newData[0] = (int)Status.Failure4 + "";
                    }
                    break;

                case WormObject newWormObject:
                    handledData = false;

                    if (tempDatabase.worms.Count > 0)
                    {
                        foreach (WormObject obj in tempDatabase.worms)
                        {
                            if (obj.UUID == newWormObject.UUID)
                            {
                                // add medicines
                                for (int i = 0; i < newWormObject.effectiveMedicines.Count; i++)
                                {
                                    obj.effectiveMedicines.Add(newWormObject.effectiveMedicines[i]);
                                }
                                // add resistences
                                for (int i = 0; i < newWormObject.resistences.Count; i++)
                                {
                                    obj.resistences.Add(newWormObject.resistences[i]);
                                }
                                // add symptoms
                                for (int i = 0; i < newWormObject.symptoms.Count; i++)
                                {
                                    obj.symptoms.Add(newWormObject.symptoms[i]);
                                }
                                // add favourite conditions
                                for (int i = 0; i < newWormObject.faveConditions.Count; i++)
                                {
                                    obj.faveConditions.Add(newWormObject.faveConditions[i]);
                                }

                                obj.extraRemarks = (newWormObject.extraRemarks != obj.extraRemarks) ? newWormObject.extraRemarks : obj.extraRemarks;

                                handledData = true;
                                newData[0] = (int)Status.Success2 + "";
                            }
                        }
                    }

                    if (!handledData)
                    {
                        // entry does not exist
                        newData[0] = (int)Status.Failure4 + "";
                    }
                    break;
            }

            WriteDatabase(tempDatabase);
        }
        else if (type == MethodType.Post)
        {
            switch (data)
            {
                case TemporalDatabaseData:
                    // we are not going to do this method type
                    newData[0] = (int)Status.Failure5 + "";
                    break;

                case WeideObject newWeideObject:
                    newData = AddEntry(newWeideObject, Helpers.WeideToUUID(tempDatabase.weides), new WeideObject().GetType());
                    break;

                case SheepObject newSheepObject:
                    newData = AddEntry(newSheepObject, Helpers.SheepToUUID(tempDatabase.sheeps), new SheepObject().GetType());
                    break;

                case WormObject newWormObject:
                    newData = AddEntry(newWormObject, Helpers.WormToUUID(tempDatabase.worms), new WormObject().GetType());
                    break;

                case SheepKoppel newSheepKoppelObject:
                    newData = AddEntry(newSheepKoppelObject, Helpers.SheepKoppelToUUID(tempDatabase.sheepKoppels), new SheepKoppel().GetType());
                    break;
            }

            WriteDatabase(tempDatabase);
        }
        else if (type == MethodType.Get)
        {
            switch (data)
            {
                case TemporalDatabaseData newDatabase:
                    newData[0] = (int)Status.Success1 + "";
                    newData[1] = tempDatabase.ToString();
                    break;

                case WeideObject newWeideObject:
                    newData = GetEntry(newWeideObject, Helpers.WeideToUUID(tempDatabase.weides), new WeideObject().GetType());
                    break;

                case SheepObject newSheepObject:
                    newData = GetEntry(newSheepObject, Helpers.SheepToUUID(tempDatabase.sheeps), new SheepObject().GetType());
                    Debug.Log("Requested: " + newData[1]);
                    requests.Add(newData[1]);
                    break;

                case WormObject newWormObject:
                    newData = GetEntry(newWormObject, Helpers.WormToUUID(tempDatabase.worms), new WormObject().GetType());
                    break;
            }
        }
        else
        {
            newData[0] = (int)Status.Failure0 + "";
        }

        return newData;
    }

    // Save database
    private static void WriteDatabase(TemporalDatabaseData data)
    {
        // write to a clean file
        File.Delete(filePath);

        // jsonfy the data
        string jsonData = JsonUtility.ToJson(data);

        // save new values
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.Write(jsonData);
        writer.Close();
    }

    // Load existing database
    private static TemporalDatabaseData LoadDatabase()
    {
        // check if there is a file to load
        if (!File.Exists(filePath)) CreateEmptyFile();

        // load the values
        StreamReader reader = new StreamReader(filePath);
        TemporalDatabaseData newDatabase = JsonUtility.FromJson<TemporalDatabaseData>(reader.ReadToEnd());
        reader.Close();

        return newDatabase;
    }

    // Create a new empty database
    private static void CreateEmptyFile()
    {
        tempDatabase = new TemporalDatabaseData();
        tempDatabase.farmerName = "Ploopploop";
        tempDatabase.farmerUUID = Helpers.GenerateUUID();
        tempDatabase.weides = new List<WeideObject>();
        tempDatabase.sheeps = new List<SheepObject>();
        tempDatabase.sheepKoppels = new List<SheepKoppel>();
        tempDatabase.worms = new List<WormObject>();

        WriteDatabase(tempDatabase);
    }

    private static string[] AddEntry(ObjectUUID newObject, List<ObjectUUID> collection, System.Type type)
    {
        string[] newData = new string[] { (int)Status.Error1 + "", "" };
        bool handledData = false;

        if (collection.Count > 0)
        {
            foreach (ObjectUUID obj in collection)
            {
                if (obj.UUID == newObject.UUID)
                {
                    handledData = true;
                    newData[0] = (int)Status.Failure3 + "";
                    break;
                }
            }
        }

        if (!handledData)
        {
            switch (type.ToString())
            {
                case "WeideObject":
                    tempDatabase.weides.Add((WeideObject)newObject);
                    newData[0] = (int)Status.Success1 + "";
                    break;
                case "SheepObject":
                    SheepObject sheepObject = (SheepObject)newObject;
                    tempDatabase.sheeps.Add(sheepObject);
                    string[] fieldCollection = { "Sheep_UUID", "Sheep_Label", "Sheep_Female", "Farmer_UUID" };
                    string[] dataCollection = { sheepObject.UUID, sheepObject.sheepTag, "" + (int)sheepObject.sex, tempDatabase.farmerUUID };
                    newData[0] = (int)Status.Success5 + "";
                    newData[1] = Helpers.GenerateUUID();
                    DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Post, "AddSheep", newData[1]);
                    break;
                case "WormObject":
                    tempDatabase.worms.Add((WormObject)newObject);
                    newData[0] = (int)Status.Success1 + "";
                    break;
                case "SheepKoppel":
                    tempDatabase.sheepKoppels.Add((SheepKoppel)newObject);
                    newData[0] = (int)Status.Success1 + "";
                    break;
            }
        }

        return newData;
    }

    private static string[] UpdateEntry(ObjectUUID newObject, List<ObjectUUID> collection, System.Type type)
    {
        string[] newData = new string[] { (int)Status.Error1 + "", "" };
        string[] fieldCollection;
        string[] dataCollection;
        bool handledData = false;

        if (collection.Count > 0)
        {
            foreach (ObjectUUID obj in collection)
            {
                if (obj.UUID == newObject.UUID)
                {
                    handledData = true;
                    newData[0] = (int)Status.Failure3 + "";
                    break;
                }
            }
        }

        if (handledData)
        {
            switch (type.ToString())
            {
                case "WeideObject":
                    WeideObject weideObject = (WeideObject)newObject;
                    fieldCollection = new[] { "Lot_UUID", "Lot_Name", "Lot_Surface", "Lot_Quality", "Lot_Mowed_TS", "Lot_State_ID", "Farmer_UUID" };
                    dataCollection = new[] { weideObject.UUID, weideObject.perceelName, weideObject.surfaceSqrMtr + "", weideObject.surfaceQuality + "", weideObject.lastMowedTs + "", weideObject.state, tempDatabase.farmerUUID };
                    newData[0] = (int)Status.Success5 + "";
                    newData[1] = Helpers.GenerateUUID();
                    DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Put, "UpdateLot", newData[1]);
                    break;
                case "SheepObject":
                    SheepObject sheepObject = (SheepObject)newObject;
                    fieldCollection = new[] { "Sheep_UUID", "Sheep_Label", "Sheep_Female", "Farmer_UUID" };
                    dataCollection = new[] { sheepObject.UUID, sheepObject.sheepTag, "" + (int)sheepObject.sex, tempDatabase.farmerUUID };
                    newData[0] = (int)Status.Success5 + "";
                    newData[1] = Helpers.GenerateUUID();
                    DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Put, "UpdateSheep", newData[1]);
                    break;
                case "WormObject":
                    WormObject wormObject = (WormObject)newObject;
                    fieldCollection = new[] { "Worm_UUID", "Worm_Latin_Name", "Worm_Normal_Name", "Worm_EPG_Danger", "Worm_Egg_Description" };
                    dataCollection = new[] { wormObject.UUID, wormObject.scientificName, wormObject.nonScienceName + "", wormObject.EPGDanger + "", wormObject.eggDescription + "" };
                    newData[0] = (int)Status.Success5 + "";
                    newData[1] = Helpers.GenerateUUID();
                    DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Put, "UpdateWorm", newData[1]);
                    break;
                case "SheepKoppel":
                    SheepKoppel sheepPairObject = (SheepKoppel)newObject;
                    fieldCollection = new[] { "Pair_UUID", "Pair_Name", "TS_Formed", "TS_Removed", "Farmer_UUID", "Last_Modified" };
                    dataCollection = new[] { sheepPairObject.UUID, sheepPairObject.koppelName, sheepPairObject.tsFormed + "", sheepPairObject.tsRemoved + "", tempDatabase.farmerUUID, sheepPairObject.lastModified + "" };
                    newData[0] = (int)Status.Success5 + "";
                    newData[1] = Helpers.GenerateUUID();
                    DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Put, "UpdateSheepPair", newData[1]);
                    break;
            }
        }

        return newData;
    }

    private static string[] GetEntry(ObjectUUID newObject, List<ObjectUUID> collection, System.Type type)
    {
        string[] newData = new string[] { (int)Status.Error1 + "", "" };
        bool handledData = false;
        string[] fieldCollection;
        string[] dataCollection;

        switch (type.ToString())
        {
            case "WeideObject":
                WeideObject weideObject = (WeideObject)newObject;
                fieldCollection = new string[] { "Lot_UUID", "Farmer_UUID" };
                dataCollection = new string[] { weideObject.UUID, tempDatabase.farmerUUID };
                newData[0] = (int)Status.Success5 + "";
                newData[1] = Helpers.GenerateUUID();
                DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Get, GetRequest.GetLot.ToString(), newData[1]);
                handledData = true;
                break;
            case "SheepObject":
                SheepObject sheepObject = (SheepObject)newObject;
                fieldCollection = new string[] { "Sheep_UUID", "Farmer_UUID" };
                dataCollection = new string[] { sheepObject.UUID, tempDatabase.farmerUUID };
                newData[0] = (int)Status.Success5 + "";
                newData[1] = Helpers.GenerateUUID();
                DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Get, GetRequest.GetSheep.ToString(), newData[1]);
                handledData = true;
                break;
            case "WormObject":
                WormObject wormObject = (WormObject)newObject;
                fieldCollection = new string[] { "Worm_UUID" };
                dataCollection = new string[] { wormObject.UUID };
                newData[0] = (int)Status.Success5 + "";
                newData[1] = Helpers.GenerateUUID();
                DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Get, GetRequest.GetWorm.ToString(), newData[1]);
                handledData = true;
                break;
            case "SheepKoppel":
                SheepKoppel koppelObject = (SheepKoppel)newObject;
                fieldCollection = new string[] { "Pair_UUID" };
                dataCollection = new string[] { koppelObject.UUID };
                newData[0] = (int)Status.Success5 + "";
                newData[1] = Helpers.GenerateUUID();
                DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Get, GetRequest.GetPair.ToString(), newData[1]);
                handledData = true;
                break;
        }

        if (!handledData)
        {
            newData[0] = (int)Status.Failure4 + "";
        }

        return newData;
    }
}
