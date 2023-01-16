using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTester : MonoBehaviour
{
    public List<string> receivedAnswers = new List<string>();

    private void OnEnable()
    {
        EventSystem<string>.AddListener(EventType.checkDatabaseResponse, CheckResponse);
    }

    private void OnDisable()
    {
        EventSystem<string>.RemoveListener(EventType.checkDatabaseResponse, CheckResponse);
    }

    private void Start()
    {
        StartCoroutine(TestMethod());
    }

    IEnumerator TestMethod()
    {
        SheepObject newSheep = new SheepObject();
        newSheep.UUID = "4dea7913-f4d7-42b3-be55-47f97b8576b2";
        Debug.Log("Asking for sheep label");
        string[] result = Database.ProgressData(MethodType.Get, newSheep);
        Debug.Log(Helpers.CodeToMessage(result[0]));
        yield return new WaitForEndOfFrame();

        while (!receivedAnswers.Contains(result[1]))
        {
            yield return new WaitForSeconds(1);
        }

        SheepCollectionJson sheepObj = JsonUtility.FromJson<SheepCollectionJson>("{\"Sheeps\":" + DBST.Instance.dataPackages[result[1]] + "}");
        DBST.Instance.dataPackages.Remove(result[1]);
        receivedAnswers.Remove(result[1]);

        Debug.Log("Label recevied: " + sheepObj.Sheeps[0].Sheep_Label);
    }

    public void CheckResponse(string methodUUID)
    {
        if (Database.requests.Contains(methodUUID) && DBST.Instance.dataPackages.ContainsKey(methodUUID))
        {
            Debug.Log("Received UUID: " + methodUUID);
            Database.requests.Remove(methodUUID);
            receivedAnswers.Add(methodUUID);
        }
        else
        {
            Debug.LogError("Method UUID not found: " + methodUUID);
        }
    }
}
