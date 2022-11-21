using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTester : MonoBehaviour
{
    public TemporalDatabaseData TemporalDatabaseData = new TemporalDatabaseData();

    void Start()
    {
        WeideObject weideObject = new WeideObject();
        weideObject.weideUUID = "updated UUID";
        WurmAPI.MethodHandler(MethodType.Put, weideObject);
    }

    void Update()
    {
        
    }
}
