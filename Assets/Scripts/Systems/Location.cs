using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Location : MonoBehaviour
{
    public static float longitude = 5.2913f;
    public static float latitude = 52.1326f;
    private string ipAddress;

    private bool isRunning;
    
    public void GetLocation()
    {
        if (isRunning) return;
        StartCoroutine(GetIPAddress());
    }

    private IEnumerator GetIPAddress()
    {
        isRunning = true;
        UnityWebRequest www = UnityWebRequest.Get("http://checkip.dyndns.org");
        yield return www.SendWebRequest();

        if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            yield break;
        }

        string result = www.downloadHandler.text;

        string[] a = result.Split(':');
        string a2 = a[1][1..];
        string[] a3 = a2.Split('<');
        string a4 = a3[0];

        ipAddress = a4;
        
        yield return StartCoroutine(GetCoordinates());

        isRunning = false;
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
        longitude = info.lon;
        longitude = info.lon;

        EventSystem<LocationInfo>.InvokeEvent(EventType.locationDataReceived, info);
    }

    public void SetLongitude(string longitude)
    {
        float value;
        try
        {
            value = float.Parse(longitude);
        }
        catch (Exception e)
        {
            Debug.Log("Warning: Could not parse longitude, using default value instead.");
            value = 5.2913f;
        }

        Location.longitude = Mathf.Clamp(value, -180f, 180f);
    }
    
    public void SetLatitude(string latitude)
    {
        float value;
        try
        {
            value = float.Parse(latitude);
        }
        catch (Exception e)
        {
            Debug.Log("Warning: Could not parse latitude, using default value instead.");
            value = 52.1326f;
        }
        Location.latitude = Mathf.Clamp(value, -90f, 90f);
    }

    public void OnToggleValueChanged(bool value)
    {
        if (value)
        {
            GetLocation();
        }
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
