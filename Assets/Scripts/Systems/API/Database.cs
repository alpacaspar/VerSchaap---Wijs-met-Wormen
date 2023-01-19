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

    public static List<LotObject> GetLotCollection()
    {
        if (tempDatabase == null) InitializeDatabase();

        return tempDatabase.Lots;
    }

    public static List<SheepObject> GetSheepCollection()
    {
        if (tempDatabase == null) InitializeDatabase();

        return tempDatabase.sheeps;
    }

    public static List<PairCollection> GetPairCollection()
    {
        if (tempDatabase == null) InitializeDatabase();

        return tempDatabase.pairCollection;
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

                case LotObject newLotObject:
                    handledData = false;

                    if (tempDatabase.Lots.Count > 0)
                    {
                        foreach (LotObject obj in tempDatabase.Lots)
                        {
                            if (obj.UUID == newLotObject.UUID)
                            {
                                obj.surfaceSqrMtr = (newLotObject.surfaceSqrMtr != obj.surfaceSqrMtr) ? newLotObject.surfaceSqrMtr : obj.surfaceSqrMtr;
                                obj.surfaceQuality = (newLotObject.surfaceQuality != obj.surfaceQuality) ? newLotObject.surfaceQuality : obj.surfaceQuality;
                                obj.grassTypes = (newLotObject.grassTypes != obj.grassTypes) ? newLotObject.grassTypes : obj.grassTypes;
                                obj.currentSheeps = (newLotObject.currentSheeps != obj.currentSheeps) ? newLotObject.currentSheeps : obj.currentSheeps;
                                obj.extraRemarks = (newLotObject.extraRemarks != obj.extraRemarks) ? newLotObject.extraRemarks : obj.extraRemarks;

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

                                obj.sheepTag = (newSheepObject.sheepTag != obj.sheepTag && newSheepObject.sheepTag != "") ? newSheepObject.sheepTag : obj.sheepTag;
                                obj.tsBorn = (newSheepObject.tsBorn != obj.tsBorn && newSheepObject.tsBorn != 0) ? newSheepObject.tsBorn : obj.tsBorn;
                                obj.extraRemarks = (newSheepObject.extraRemarks != obj.extraRemarks) ? newSheepObject.extraRemarks : obj.extraRemarks;
                                obj.isDeleted = (newSheepObject.isDeleted != obj.isDeleted) ? newSheepObject.isDeleted : obj.isDeleted;
                                obj.sex = (newSheepObject.sex != obj.sex) ? newSheepObject.sex : obj.sex;
                                obj.sheepType = (newSheepObject.sheepType != obj.sheepType) ? newSheepObject.sheepType : obj.sheepType;

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

                case LotObject newLotObject:
                    newData = AddEntry(newLotObject, Helpers.LotToUUID(tempDatabase.Lots), new LotObject().GetType());
                    break;

                case SheepObject newSheepObject:
                    newData = AddEntry(newSheepObject, Helpers.SheepToUUID(tempDatabase.sheeps), new SheepObject().GetType());
                    break;

                case WormObject newWormObject:
                    newData = AddEntry(newWormObject, Helpers.WormToUUID(tempDatabase.worms), new WormObject().GetType());
                    break;

                case PairCollection newPair:
                    newData = AddEntry(newPair, Helpers.PairCollectionToUUID(tempDatabase.pairCollection), new PairCollection().GetType());
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

                case LotObject newLotObject:
                    newData = GetEntry(newLotObject, Helpers.LotToUUID(tempDatabase.Lots), new LotObject().GetType());
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
    public static void WriteDatabase(TemporalDatabaseData data)
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
        tempDatabase.Lots = new List<LotObject>();
        tempDatabase.sheeps = new List<SheepObject>();
        tempDatabase.pairCollection = new List<PairCollection>();
        tempDatabase.worms = new List<WormObject>();

        WriteDatabase(tempDatabase);
    }

    private static string[] AddEntry(ObjectUUID newObject, List<ObjectUUID> collection, System.Type type)
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

        if (!handledData)
        {
            switch (type.ToString())
            {
                case "LotObject":
                    LotObject LotObject = (LotObject)newObject;
                    tempDatabase.Lots.Add(LotObject); 
                    fieldCollection = new string[] { "Lot_UUID", "Lot_Name", "Lot_Surface", "Lot_Quality", "Lot_Mowed_TS", "Lot_State_ID", "Is_Deleted", "Farmer_UUID", "Last_Modified" };
                    dataCollection = new string[] { LotObject.UUID, LotObject.perceelName, "" + LotObject.surfaceSqrMtr, "" + LotObject.surfaceQuality, "" + LotObject.lastMowedTs, "" + LotObject.surfaceSqrMtr, "" + LotObject.state, "" + LotObject.isDeleted, tempDatabase.farmerUUID };
                    EntryInner(fieldCollection, dataCollection, newData, "AddLot", MethodType.Post);
                    break;
                case "SheepObject":
                    SheepObject sheepObject = (SheepObject)newObject;
                    tempDatabase.sheeps.Add(sheepObject);
                    fieldCollection = new string[] { "Sheep_UUID", "Sheep_Label", "Sheep_Female", "Sheep_Species", "Timestamp_Born", "Is_Deleted", "Farmer_UUID" };
                    dataCollection = new string[] { sheepObject.UUID, sheepObject.sheepTag, "" + (int)sheepObject.sex, "" + (int)sheepObject.sheepType, "" + sheepObject.tsBorn, "" + sheepObject.isDeleted, tempDatabase.farmerUUID };
                    EntryInner(fieldCollection, dataCollection, newData, "AddSheep", MethodType.Post);
                    break;
                case "WormObject":
                    WormObject wormObject = (WormObject)newObject;
                    tempDatabase.worms.Add(wormObject);
                    fieldCollection = new string[] { "Worm_UUID", "Worm_Latin_Name", "Worm_Normal_Name", "Worm_EPG_Danger", "Worm_Egg_Description", "Is_Deleted" };
                    dataCollection = new string[] { wormObject.UUID, wormObject.scientificName, wormObject.nonScienceName, wormObject.EPGDanger + "", wormObject.eggDescription, "" + wormObject.isDeleted };
                    EntryInner(fieldCollection, dataCollection, newData, "AddNewWorm", MethodType.Post);
                    break;
                case "PairCollection":
                    PairCollection pairObject = (PairCollection)newObject;
                    tempDatabase.pairCollection.Add(pairObject);
                    fieldCollection = new string[] { "Pair_Name", "Pair_UUID", "Is_Deleted", "Farmer_UUID" };
                    dataCollection = new string[] { pairObject.pairCollectionName, pairObject.UUID, "" + pairObject.isDeleted, tempDatabase.farmerUUID };
                    EntryInner(fieldCollection, dataCollection, newData, "AddPair", MethodType.Post);
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
                case "LotObject":
                    LotObject LotObject = (LotObject)newObject;
                    fieldCollection = new[] { "Lot_UUID", "Lot_Name", "Lot_Surface", "Lot_Quality", "Lot_Mowed_TS", "Lot_State_ID", "Is_Deleted", "Farmer_UUID" };
                    dataCollection = new[] { LotObject.UUID, LotObject.perceelName, LotObject.surfaceSqrMtr + "", LotObject.surfaceQuality + "", LotObject.lastMowedTs + "", LotObject.state, "" + LotObject.isDeleted, tempDatabase.farmerUUID };
                    EntryInner(fieldCollection, dataCollection, newData, "UpdateLot", MethodType.Put);
                    break;
                case "SheepObject":
                    SheepObject sheepObject = (SheepObject)newObject;
                    fieldCollection = new[] { "Sheep_UUID", "Sheep_Label", "Sheep_Female", "Sheep_Species", "Timestamp_Born", "Is_Deleted", "Farmer_UUID" };
                    dataCollection = new[] { sheepObject.UUID, sheepObject.sheepTag, "" + (int)sheepObject.sex, "" + (int)sheepObject.sheepType, "" + sheepObject.tsBorn, "" + sheepObject.isDeleted, tempDatabase.farmerUUID };
                    EntryInner(fieldCollection, dataCollection, newData, "UpdateSheep", MethodType.Put);
                    break;
                case "WormObject":
                    WormObject wormObject = (WormObject)newObject;
                    fieldCollection = new[] { "Worm_UUID", "Worm_Latin_Name", "Worm_Normal_Name", "Worm_EPG_Danger", "Worm_Egg_Description", "Is_Deleted" };
                    dataCollection = new[] { wormObject.UUID, wormObject.scientificName, wormObject.nonScienceName + "", wormObject.EPGDanger + "", wormObject.eggDescription + "", "" + wormObject.isDeleted };
                    EntryInner(fieldCollection, dataCollection, newData, "UpdateWorm", MethodType.Put);
                    break;
                case "PairCollection":
                    PairCollection sheepPairObject = (PairCollection)newObject;
                    fieldCollection = new[] { "Pair_UUID", "Pair_Name", "TS_Formed", "TS_Removed", "Is_Deleted", "Farmer_UUID", "Last_Modified" };
                    dataCollection = new[] { sheepPairObject.UUID, sheepPairObject.pairCollectionName, sheepPairObject.tsFormed + "", sheepPairObject.tsRemoved + "", "" + sheepPairObject.isDeleted, tempDatabase.farmerUUID, sheepPairObject.lastModified + "" };
                    EntryInner(fieldCollection, dataCollection, newData, "UpdateSheepPair", MethodType.Put);
                    break;
            }
        }

        return newData;
    }

    private static void EntryInner(string[] fieldCollection, string[] dataCollection, string[] newData, string phpMethod, MethodType methodType)
    {
        newData[0] = (int)Status.Success5 + "";
        newData[1] = Helpers.GenerateUUID();
        DBST.Instance.FireURI(fieldCollection, dataCollection, methodType, phpMethod, newData[1]);
    }

    private static string[] GetEntry(ObjectUUID newObject, List<ObjectUUID> collection, System.Type type)
    {
        string[] newData = new string[] { (int)Status.Error1 + "", "" };
        bool handledData = false;
        string[] fieldCollection;
        string[] dataCollection;

        switch (type.ToString())
        {
            case "LotObject":
                LotObject LotObject = (LotObject)newObject;
                fieldCollection = new string[] { "Lot_UUID", "Farmer_UUID" };
                dataCollection = new string[] { LotObject.UUID, tempDatabase.farmerUUID };
                EntryInner(fieldCollection, dataCollection, newData, GetRequest.GetLot.ToString(), MethodType.Get);
                //newData[0] = (int)Status.Success5 + "";
                //newData[1] = Helpers.GenerateUUID();
                //DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Get, GetRequest.GetLot.ToString(), newData[1]);
                handledData = true;
                break;
            case "SheepObject":
                SheepObject sheepObject = (SheepObject)newObject;
                fieldCollection = new string[] { "Sheep_UUID", "Farmer_UUID" };
                dataCollection = new string[] { sheepObject.UUID, tempDatabase.farmerUUID };
                EntryInner(fieldCollection, dataCollection, newData, GetRequest.GetSheep.ToString(), MethodType.Get);
                handledData = true;
                break;
            case "WormObject":
                WormObject wormObject = (WormObject)newObject;
                fieldCollection = new string[] { "Worm_UUID" };
                dataCollection = new string[] { wormObject.UUID };
                EntryInner(fieldCollection, dataCollection, newData, GetRequest.GetWorm.ToString(), MethodType.Get);
                handledData = true;
                break;
            case "PairCollection":
                PairCollection koppelObject = (PairCollection)newObject;
                fieldCollection = new string[] { "Pair_UUID" };
                dataCollection = new string[] { koppelObject.UUID };
                EntryInner(fieldCollection, dataCollection, newData, GetRequest.GetPair.ToString(), MethodType.Get);
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
