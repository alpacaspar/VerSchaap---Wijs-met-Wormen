using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClientTest : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem<WeatherInfo>.AddListener(EventType.weatherDataReceived, OnWeatherDataReceived);
    }

    private void OnDisable()
    {
        EventSystem<WeatherInfo>.RemoveListener(EventType.weatherDataReceived, OnWeatherDataReceived);
    }
    
    private void Start()
    {
        Connect($"http://{NetworkTest.GetLocalIP()}:{8052}/");   
    }
    
    /// <summary>
    /// Connect http client to specified uri.
    /// </summary>
    /// <param name="uri">uri as string.</param>
    public void Connect(string uri)
    {
        StartCoroutine(LookForHttpConnection(uri));
    }

    /// <summary>
    /// Post data string to the specified URI.
    /// </summary>
    /// <param name="uri">URI Address string.</param>
    /// <param name="data">Data string like JSON.</param>
    public void Post(string uri, string data)
    {
        StartCoroutine(PostToUri(uri, data));
    }

    private void OnWeatherDataReceived(WeatherInfo info)
    {
        Post($"http://{NetworkTest.GetLocalIP()}:{8052}/", JsonUtility.ToJson(info));
    }

    private IEnumerator LookForHttpConnection(string uri)
    {
        var www = new UnityWebRequest(uri)
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
    
    private IEnumerator PostToUri(string uri, string data)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(uri, data))
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
