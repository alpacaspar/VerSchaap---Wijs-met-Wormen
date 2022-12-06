using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class Weather : MonoBehaviour
{
    // documentation for open-meteo api: https://open-meteo.com/en/docs

    private void OnEnable()
    {
        EventSystem<LocationInfo>.AddListener(EventType.locationDataReceived,OnLocationDataReceived);
    }

    private void OnDisable()
    {
        EventSystem<LocationInfo>.RemoveListener(EventType.locationDataReceived,OnLocationDataReceived);
    }

    private void OnLocationDataReceived(LocationInfo info)
    {
        UpdateWeather(info);
    }
    
    /// <summary>
    /// Update weather information based on the most recently updated location information.
    /// </summary>
    public void UpdateWeather(LocationInfo info)
    {
        StartCoroutine(GetWeather(info.lat, info.lon, info.timezone));
    }
    
    private IEnumerator GetWeather(float latitude, float longitude, string timezone)
    {
        string latitudeString = latitude.ToString(CultureInfo.InvariantCulture);
        string longitudeString = longitude.ToString(CultureInfo.InvariantCulture);
        var www = new UnityWebRequest($"https://api.open-meteo.com/v1/forecast?latitude={latitudeString}&longitude={longitudeString}&hourly=temperature_2m,relativehumidity_2m,precipitation,cloudcover,weathercode&timezone={timezone}")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();
        
        if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            yield break;
        }

        var info = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);
        
        EventSystem<WeatherInfo>.InvokeEvent(EventType.weatherDataReceived, info);
    }
}

// Code Conventions according to the Open-Meteo Weather API
[Serializable]
public class WeatherInfo
{
    public float latitude;
    public float longitude;
    public float generationtime_ms;
    public int utc_offset_seconds;
    public string timezone;
    public string timezone_abbreviation;
    public int elevation;
    public HourlyUnits hourly_units;
    public Hourly hourly;
}

// Code Conventions according to the Open-Meteo Weather API
[Serializable]
public class Hourly
{
    public string[] time;
    public float[] temperature_2m;
    public int[] relativehumidity_2m;
    public int[] precipitation;
    public int[] cloudcover;
    public WeatherType[] weathercode;
}

// Code Conventions according to the Open-Meteo Weather API
[Serializable]
public class HourlyUnits
{
    public string time;
    public string temperature_2m;
    public string relativehumidity_2m;
    public string precipitation;
    public string cloudcover;
}