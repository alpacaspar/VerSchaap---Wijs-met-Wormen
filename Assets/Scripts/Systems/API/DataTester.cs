using System.Collections.Generic;
using UnityEngine;

public class DataTester : MonoBehaviour
{
    void Start()
    {
        // populate file
        PopulateSheeps(250);
        PopulateWorms(5);
        PopulateSurfaces(20);
    }

    /// <summary>
    ///     Generate dummy data for sheep
    /// </summary>
    /// <param name="quantity">
    ///     How many sheeps do we need?
    /// </param>
    public void PopulateSheeps(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            SheepObject newSheep = new SheepObject();
            newSheep.UUID = Helpers.GenerateUUID();
            newSheep.tsBorn = Helpers.GetCurrentTimestamp() - Random.Range(15000, 20000);
            newSheep.sex = Sex.Female;
            newSheep.sheepType = SheepType.WiltshireHorn;

            for (int j = 0; j < Random.Range(5, 10); j++)
            {
                SheepWeight weight = new SheepWeight();
                weight.weight = Random.Range(10, 100);
                weight.timestamp = Helpers.GetCurrentTimestamp() - Random.Range(100, 10000);
                newSheep.weight.Add(weight);
            }

            string[] response = WurmAPI.MethodHandler(MethodType.Post, newSheep);
            Debug.Log(Helpers.CodeToMessage(response));
        }
    }

    /// <summary>
    ///     Generate dummy data for worms
    /// </summary>
    /// <param name="quantity">
    ///     How many sheeps do we need?
    /// </param>
    public void PopulateWorms(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            WormObject newWorm = new WormObject();
            newWorm.UUID = Helpers.GenerateUUID();
            newWorm.wormType = WormType.Leverbot;

            string[] response = WurmAPI.MethodHandler(MethodType.Post, newWorm);
            Debug.Log(Helpers.CodeToMessage(response));
        }
    }

    /// <summary>
    ///     Generate dummy data for weides
    /// </summary>
    /// <param name="quantity">
    ///     How many sheeps do we need?
    /// </param>
    public void PopulateSurfaces(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            WeideObject newWeide = new WeideObject();
            newWeide.UUID = Helpers.GenerateUUID();
            newWeide.surfaceSqrMtr = Random.Range(50, 500);
            newWeide.surfaceQuality = Random.Range(10, 95);

            string[] response = WurmAPI.MethodHandler(MethodType.Post, newWeide);
            Debug.Log(Helpers.CodeToMessage(response));
        }
    }

    /// <summary>
    ///     Test sample of doing a request for an entire database
    /// </summary>
    /// <param name="type">
    ///     Requires the method type such as put for example
    /// </param>
    /// <param name="tempDatabase">
    ///     Requires the database in order to function properly
    /// </param>
    /// <returns>
    ///     Returns a dictionary with the http messages (unmapped) and the returned data as a complete collection, results have to be handled seperately
    /// </returns>
    public Dictionary<List<string>,List<string>> ProgressTestData(MethodType type, TemporalDatabaseData tempDatabase)
    {
        List<string> httpsMessages = new List<string>();
        List<string> data = new List<string>();

        // iterate through all weides from database
        foreach (WeideObject newObj in tempDatabase.weides)
        {
            string[] response = WurmAPI.MethodHandler(type, newObj);
            httpsMessages.Add(response[0]);
            data.Add(response[1]);
        }

        // iterate through all sheeps from database
        foreach (SheepObject newObj in tempDatabase.sheeps)
        {
            string[] response = WurmAPI.MethodHandler(type, newObj);
            httpsMessages.Add(response[0]);
            data.Add(response[1]);
        }

        // iterate through all worms from database
        foreach (WormObject newObj in tempDatabase.worms)
        {
            string[] response = WurmAPI.MethodHandler(type, newObj);
            httpsMessages.Add(response[0]);
            data.Add(response[1]);
        }

        Dictionary<List<string>, List<string>> results = new Dictionary<List<string>, List<string>>();
        results.Add(httpsMessages, data);

        return results;
    }
}
