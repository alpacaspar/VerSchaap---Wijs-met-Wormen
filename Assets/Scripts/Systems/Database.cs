using System;
using System.Collections.Generic;
using UnityEngine;

public static class Database
{
    private static TemporalDatabaseData tempDatabase = null;

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
    public static string[] ProgressData<IObect>(MethodType type, IObect data)
    {
        if (tempDatabase == null) InitializeDatabase();

        string[] newData = new string[] { Status.Error1.ToString(), "" };
        bool handledData = false;

        if (type == MethodType.Put)
        {
            switch (data)
            {
                case TemporalDatabaseData:
                        // we are not going to do this method type
                        newData[0] = Status.Failure5.ToString();
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
                                newData[0] = Status.Success2.ToString();
                            }
                        }
                    }

                    if (!handledData)
                    {
                        // entry does not exist
                        newData[0] = Status.Error4.ToString();
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
                                // add new weight
                                for (int i = 0; i < newSheepObject.weight.Count; i++)
                                {
                                    obj.weight.Add(newSheepObject.weight[i]);
                                }
                                // add new diseases collection
                                for (int i = 0; i < newSheepObject.diseases.Count; i++)
                                {
                                    obj.diseases.Add(newSheepObject.diseases[i]);
                                }

                                obj.tsBorn = (newSheepObject.tsBorn != obj.tsBorn && newSheepObject.tsBorn != 0) ? newSheepObject.tsBorn : obj.tsBorn;
                                obj.extraRemarks = (newSheepObject.extraRemarks != obj.extraRemarks) ? newSheepObject.extraRemarks : obj.extraRemarks;

                                handledData = true;
                                newData[0] = Status.Success2.ToString();
                            }
                        }
                    }

                    if (!handledData)
                    {
                        // entry does not exist
                        newData[0] = Status.Error4.ToString();
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
                                newData[0] = Status.Success2.ToString();
                            }
                        }
                    }

                    if (!handledData)
                    {
                        // entry does not exist
                        newData[0] = Status.Error4.ToString();
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
                    newData[0] = Status.Failure5.ToString();
                    break;

                case WeideObject newWeideObject:
                    newData = AddEntry(newWeideObject, Helpers.WeideToUUID(tempDatabase.weides));
                    break;

                case SheepObject newSheepObject:
                    newData = AddEntry(newSheepObject, Helpers.SheepToUUID(tempDatabase.sheeps));
                    break;

                case WormObject newWormObject:
                    newData = AddEntry(newWormObject, Helpers.WormToUUID(tempDatabase.worms));
                    break;
            }

            WriteDatabase(tempDatabase);
        }
        else if (type == MethodType.Get)
        {
            switch (data)
            {
                case TemporalDatabaseData newDatabase:
                    newData[0] = Status.Success1.ToString();
                    newData[1] = tempDatabase.ToString();
                    break;

                case WeideObject newWeideObject:
                    newData = GetEntry(newWeideObject, Helpers.WeideToUUID(tempDatabase.weides));
                    break;

                case SheepObject newSheepObject:
                    newData = GetEntry(newSheepObject, Helpers.SheepToUUID(tempDatabase.sheeps));
                    break;

                case WormObject newWormObject:
                    newData = GetEntry(newWormObject, Helpers.WormToUUID(tempDatabase.worms));
                    break;
            }
        }
        else
        {
            newData[0] = Status.Failure0.ToString();
        }

        return newData;
    }

    private static void WriteDatabase(TemporalDatabaseData data)
    {
        // save new values
    }

    private static string[] AddEntry(ObjectUUID newObject, List<ObjectUUID> oldObjects)
    {
        string[] newData = new string[] { Status.Error1.ToString(), "" };
        bool handledData = false;

        if (tempDatabase.weides.Count > 0)
        {
            foreach (ObjectUUID obj in oldObjects)
            {
                if (obj.UUID == newObject.UUID)
                {
                    handledData = true;
                    newData[0] = Status.Failure3.ToString();
                    break;
                }
            }
        }

        if (!handledData)
        {
            oldObjects.Add(newObject);
            newData[0] = Status.Success1.ToString();
        }

        return newData;
    }

    private static string[] GetEntry(ObjectUUID newObject, List<ObjectUUID> oldObjects)
    {
        string[] newData = new string[] { Status.Error1.ToString(), "" };
        bool handledData = false;

        if (tempDatabase.weides.Count > 0)
        {
            foreach (ObjectUUID obj in oldObjects)
            {
                if (obj.UUID == newObject.UUID)
                {
                    handledData = true;
                    newData[0] = Status.Success1.ToString();
                    newData[1] = obj.ToString();
                    break;
                }
            }
        }

        if (!handledData)
        {
            newData[0] = Status.Failure4.ToString();
        }

        return newData;
    }
}
