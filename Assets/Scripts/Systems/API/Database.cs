using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Database
{
    private static TemporalDatabaseData tempDatabase = null;

    private static string filePath = Application.persistentDataPath + "/database.xml";

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

                    if (tempDatabase.weides.Count > 0)
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
                                newData = UpdateEntry(obj, Helpers.SheepToUUID(tempDatabase.sheeps), new WeideObject().GetType());

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

                    if (tempDatabase.weides.Count > 0)
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
                    DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Post, "AddSheep");
                    newData[0] = (int)Status.Success1 + ""; // TODO WAITING ON REQUEST RESPONSE
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
                    break;
                case "SheepObject":
                    SheepObject sheepObject = (SheepObject)newObject;
                    tempDatabase.sheeps.Add(sheepObject);
                    string[] fieldCollection = { "Sheep_UUID", "Sheep_Label", "Sheep_Female", "Farmer_UUID" };
                    string[] dataCollection = { sheepObject.UUID, sheepObject.sheepTag, "" + (int)sheepObject.sex, tempDatabase.farmerUUID };
                    DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Post, "UpdateSheep");
                    newData[0] = (int)Status.Success1 + ""; // TODO WAITING ON REQUEST RESPONSE
                    break;
                case "WormObject":
                    break;
                case "SheepKoppel":
                    break;
            }
        }

        return newData;
    }

    // If entry is found return json string
    private static string[] GetEntry(ObjectUUID newObject, List<ObjectUUID> collection, System.Type type)
    {
        string[] newData = new string[] { (int)Status.Error1 + "", "" };
        bool handledData = false;

        switch (type.ToString())
        {
            case "WeideObject":
                break;
            case "SheepObject":
                SheepObject sheepObject = (SheepObject)newObject;
                string[] fieldCollection = { "Sheep_UUID", "Farmer_UUID" };
                string[] dataCollection = { sheepObject.UUID, tempDatabase.farmerUUID };
                DBST.Instance.FireURI(fieldCollection, dataCollection, MethodType.Get, "GetSheep");
                newData[0] = (int)Status.Success1 + ""; // TODO WAITING ON REQUEST RESPONSE
                handledData = true;
                break;
            case "WormObject":
                break;
            case "SheepKoppel":
                break;
        }

        if (!handledData)
        {
            newData[0] = (int)Status.Failure4 + "";
        }

        return newData;
    }
}
