using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeeklyForecast : MonoBehaviour
{
    [SerializeField] private GameObject dailyForecastPrefab;
    [SerializeField] private WeatherInfo weatherInfo;

    [SerializeField] private WeatherIcon[] icons;

    private Dictionary<WeatherType, Sprite> WeatherIcons
    {
        get => icons.ToDictionary(x => x.weatherType, x => x.icon);
    }

    private void OnEnable()
    {
        EventSystem<WeatherInfo>.AddListener(EventType.weatherDataReceived, OnWeatherDataReceived);
    }

    private void OnDisable()
    {
        EventSystem<WeatherInfo>.RemoveListener(EventType.weatherDataReceived, OnWeatherDataReceived);
    }

    public int selectedDayIndex;

    private void OnWeatherDataReceived(WeatherInfo info)
    {
        weatherInfo = info;

        var day = 0;
        for (var i = 0; i < weatherInfo.hourly.time.Length; i++)
        {
            if (i % 24 == 0)
            {
                var forecastDay = ((int)DateTime.Today.DayOfWeek + day) % 7;
                var dayAbb = GetDayAbb((DayOfWeek)forecastDay);
                
                var instance = Instantiate(dailyForecastPrefab, transform);
                instance.GetComponent<DailyForecast>().SetDailyForecastData(dayAbb, null, GetAvgTemp(info, i, 5, 18), GetAvgTemp(info, i, 19, 24), this, day);
                day++;
            }
        }
    }
    
    private int GetAvgTemp(WeatherInfo info, int startIndex, int startTime, int endTime)
    {
        var temp = 0f;

        for (var i = startIndex + startTime; i < startIndex + endTime; i++)
        {
            temp += info.hourly.temperature_2m[i];
        }

        return Mathf.RoundToInt(temp / (endTime - startTime));
    }

    /// <summary>
    /// Converts DayOfWeek enum to dutch abbreviation.
    /// </summary>
    /// <param name="dayOfWeek">DayOfTheWeek enum.</param>
    /// <returns>Returns dutch abbreviation of the DayOfWeek input as string.</returns>
    public static string GetDayAbb(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => "Ma",
            DayOfWeek.Tuesday => "Di",
            DayOfWeek.Wednesday => "Wo",
            DayOfWeek.Thursday => "Do",
            DayOfWeek.Friday => "Vr",
            DayOfWeek.Saturday => "Za",
            DayOfWeek.Sunday => "Zo",
            _ => null
        };
    }
    
    /// <summary>
    /// Converts DayOfWeek enum to string
    /// </summary>
    /// <param name="dayOfWeek">DayOfWeek enum</param>
    /// <returns>Returns name of the day of the week in dutch.</returns>
    public static string GetDay(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => "Maandag",
            DayOfWeek.Tuesday => "Dinsdag",
            DayOfWeek.Wednesday => "Woensdag",
            DayOfWeek.Thursday => "Donderdag",
            DayOfWeek.Friday => "Vrijdag",
            DayOfWeek.Saturday => "Zaterdag",
            DayOfWeek.Sunday => "Zondag",
            _ => null
        };
    }

    [Serializable]
    private class WeatherIcon
    {
        public WeatherType weatherType;
        public Sprite icon;
    }
}
