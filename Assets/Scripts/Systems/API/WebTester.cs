using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SheepObject newSheep = new SheepObject();
        newSheep.sheepTag = "NL-123456-1-12345";
        newSheep.UUID = Helpers.GenerateUUID();
        newSheep.sex = Sex.Female;
        string[] result = Database.ProgressData(MethodType.Post, newSheep);
        Debug.Log(Helpers.CodeToMessage(result));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
