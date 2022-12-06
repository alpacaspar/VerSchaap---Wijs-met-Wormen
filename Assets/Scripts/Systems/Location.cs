using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Location : MonoBehaviour
{
    private string ipAddress;

    private void Start()
    {
        StartCoroutine(GetIPAddress());
    }

    private IEnumerator GetIPAddress()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://checkip.dyndns.org");
        yield return www.SendWebRequest();

        if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            yield break;
        }

        string result = www.downloadHandler.text;

        string[] a = result.Split(':');
        string a2 = a[1].Substring(1);
        string[] a3 = a2.Split('<');
        string a4 = a3[0];

        ipAddress = a4;
        
        yield return StartCoroutine(GetCoordinates());
    }

    private IEnumerator GetCoordinates()
    {
        UnityWebRequest www = new UnityWebRequest("http://ip-api.com/json/" + ipAddress)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();
        
        if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            yield break;
        }

        LocationInfo info = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);

        EventSystem<LocationInfo>.InvokeEvent(EventType.locationDataReceived, info);
    }
}

[Serializable]
public class LocationInfo
{
    public string query;
    public string status;
    public string continent;
    public string continentCode;
    public string country;
    public string countryCode;
    public string region;
    public string regionName;
    public string city;
    public string district;
    public string zip;
    public float lat;
    public float lon;
    public string timezone;
    public string offset;
    public string currency;
    public string isp;
    public string org;
    public string @as;
    public string asname;
    public bool mobile;
    public bool proxy;
    public bool hosting;
}
