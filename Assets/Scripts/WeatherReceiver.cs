using System;
using System.Linq;
using UnityEngine;

public class WeatherReceiver : MonoBehaviour
{
    [SerializeField] private WeatherInfo weatherInfo;

    private void OnEnable()
    {
        EventSystem<WeatherInfo>.AddListener(EventType.weatherDataReceived, OnWeatherDataReceived);
    }

    private void OnDisable()
    {
        EventSystem<WeatherInfo>.RemoveListener(EventType.weatherDataReceived, OnWeatherDataReceived);
    }

    private void OnWeatherDataReceived(WeatherInfo info)
    {
        weatherInfo = info;
    }

    /// <summary>
    /// Returns the predicted temperature at the specified date and time provided.
    /// </summary>
    /// <param name="dateTime"></param>
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
    
    /// <summary>
    /// Returns the average predicted temperature of the next 7 days.
    /// </summary>
    public float GetAverageTemperatureWeekly()
    {
        return weatherInfo.hourly.temperature_2m.Average();
    }
    
    /// <summary>
    /// Returns the average predicted precipitation of the next 7 days.
    /// </summary>
    public int GetAveragePrecipitationWeekly()
    {
        return weatherInfo.hourly.precipitation.Sum() / weatherInfo.hourly.precipitation.Length;
    }
    
    /// <summary>
    /// Returns the average predicted relative humidity of the next 7 days.
    /// </summary>
    public int GetRelativeHumidityWeekly()
    {
        return weatherInfo.hourly.relativehumidity_2m.Sum() / weatherInfo.hourly.relativehumidity_2m.Length;
    }
    
    /// <summary>
    /// Returns the average predicted cloud cover of the next 7 days.
    /// </summary>
    public int GetAverageCloudCoverWeekly()
    {
        return weatherInfo.hourly.cloudcover.Sum() / weatherInfo.hourly.cloudcover.Length;
    }
}
