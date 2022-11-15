using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Location : MonoBehaviour
{
    private string ipAddress;
    public LocationInfo info;
    private float latitude;
    private float longitude;
    private string timezone;
    
    public WeatherInfo weatherInfo;
    
    private void Start()
    {
        StartCoroutine(GetIPAddress());
    }

    /// <summary>
    /// Returns the current location information.
    /// </summary>
    /// <returns>LocationInfo class.</returns>
    public LocationInfo GetLocation()
    {
        return info;
    }
    
    private IEnumerator GetIPAddress()
    {
        var www = UnityWebRequest.Get("http://checkip.dyndns.org");
        yield return www.SendWebRequest();

        if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            yield break;
        }

        var result = www.downloadHandler.text;

        var a = result.Split(':');
        var a2 = a[1].Substring(1);
        var a3 = a2.Split('<');
        var a4 = a3[0];

        ipAddress = a4;
        
        yield return StartCoroutine(GetCoordinates());
    }

    private IEnumerator GetCoordinates()
    {
        var www = new UnityWebRequest("http://ip-api.com/json/" + ipAddress)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();
        
        if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            yield break;
        }

        info = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);
        latitude = info.lat;
        longitude = info.lon;
        timezone = info.timezone;
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
