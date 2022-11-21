using System.Collections;
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
        Debug.Log(Helpers.HttpMessage[(Status)int.Parse(response[0])]);
    }
}
