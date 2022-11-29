using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClientTest : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(LookForHttpConnection());
    }

    private IEnumerator LookForHttpConnection()
    {
        string port = "9876";
        var www = new UnityWebRequest($"http://10.3.27.232:{port}/")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();
        
        if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            yield break;
        }

        Debug.Log(www.downloadHandler.text);
    }
}
