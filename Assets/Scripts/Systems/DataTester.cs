using System.Collections.Generic;
using UnityEngine;

public class DataTester : MonoBehaviour
{
    void Start()
    {
        // boot up the database
        Database.InitializeDatabase();

        // prepare new data
        WeideObject weideObject = new WeideObject();
        weideObject.surfaceQuality = 69;
        string[] response = WurmAPI.MethodHandler(MethodType.Put, weideObject);
        // prints the received code
        Debug.Log(response[0] + ": " + Helpers.HttpMessage[(Status)int.Parse(response[0])]);
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
