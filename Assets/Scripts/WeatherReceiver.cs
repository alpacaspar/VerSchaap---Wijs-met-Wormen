using System;
using UnityEngine;

public class WeatherReceiver : MonoBehaviour
{
    [SerializeField] private WeatherInfo weatherInfo;
    
    private void OnEnable()
    {
        EventSystem<WeatherInfo>.AddListener(EventType.weatherDataReceived,OnWeatherDataReceived);
    }

    private void OnDisable()
    {
        EventSystem<WeatherInfo>.RemoveListener(EventType.weatherDataReceived,OnWeatherDataReceived);
    }

    private void OnWeatherDataReceived(WeatherInfo info)
    {
        weatherInfo = info;
    }

    /// <summary>
    /// Returns the predicted temperature at the specified date and time provided.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>Returns temperature float.</returns>
    public float GetTemperatureAtDateTime(string dateTime)
    {
        var index = Array.IndexOf(weatherInfo.hourly.time, dateTime);
        return weatherInfo.hourly.temperature_2m[index];
    }
    
    /// <summary>
    /// Returns the predicted precipitation at the specified date and time provided.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>Returns precipitationW in percentages. </returns>
    public int GetPrecipitationAtDateTime(string dateTime)
    {
        var index = Array.IndexOf(weatherInfo.hourly.time, dateTime);
        return weatherInfo.hourly.precipitation[index];
    }
    
    /// <summary>
    /// Returns the predicted relative humidity at the specified date and time provided.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>Returns relative humidity in percentages. </returns>
    public int GetRelativeHumidityAtDateTime(string dateTime)
    {
        var index = Array.IndexOf(weatherInfo.hourly.time, dateTime);
        return weatherInfo.hourly.relativehumidity_2m[index];
    }
    
    /// <summary>
    /// Returns the predicted cloud cover at the specified date and time provided.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>Returns cloud cover in percentages. </returns>
    public int GetCloudCoverAtDateTime(string dateTime)
    {
        var index = Array.IndexOf(weatherInfo.hourly.time, dateTime);
        return weatherInfo.hourly.cloudcover[index];
    }
}
