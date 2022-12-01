using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClientTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LookForHttpConnection());
        StartCoroutine(Post());
    }

    private IEnumerator LookForHttpConnection()
    {
        string port = "9876";
        var www = new UnityWebRequest($"http://{NetworkTest.GetLocalIP()}:{port}/")
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
    
    private IEnumerator Post()
    {
        string port = "9876";
        using (UnityWebRequest www = UnityWebRequest.Post($"http://{NetworkTest.GetLocalIP()}:{port}/", "Hello to you too"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Successfully posted a message");
            }
        }
    }
}
