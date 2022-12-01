using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClientTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LookForHttpConnection());

        EventSystem<WeatherInfo>.AddListener(EventType.weatherDataReceived, OnWeatherDataReceived);
    }

    private void OnWeatherDataReceived(WeatherInfo info)
    {
        StartCoroutine(Post(JsonUtility.ToJson(info)));
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
    
    private IEnumerator Post(string data)
    {
        string port = "9876";
        using (UnityWebRequest www = UnityWebRequest.Post($"http://{NetworkTest.GetLocalIP()}:{port}/", data))
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
