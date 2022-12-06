using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeeklyForecast : MonoBehaviour
{
    [SerializeField] private GameObject dailyForecastPrefab;
    [SerializeField] private WeatherInfo weatherInfo;

    [SerializeField] private WeatherIconsObject weatherIconsObject;

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

        int day = 0;
        for (int i = 0; i < weatherInfo.hourly.time.Length; i++)
        {
            if (i % 24 == 0)
            {
                int forecastDay = ((int)DateTime.Today.DayOfWeek + day) % 7;
                string dayAbbreviation = GetDayAbbreviation((DayOfWeek)forecastDay);
                Sprite sprite = weatherIconsObject.WeatherIcons[GetAverageWeatherCode(i)];
                
                GameObject instance = Instantiate(dailyForecastPrefab, transform);
                instance.GetComponent<DailyForecast>().SetDailyForecastData(dayAbbreviation, sprite, GetAverageTemp(info, i, 5, 18), GetAverageTemp(info, i, 19, 24), this, day);
                GetAverageWeatherCode(i);
                day++;
            }
        }
    }
    
    private int GetAverageTemp(WeatherInfo info, int startIndex, int startTime, int endTime)
    {
        float temp = 0f;

        for (int i = startIndex + startTime; i < startIndex + endTime; i++)
        {
            temp += info.hourly.temperature_2m[i];
        }

        return Mathf.RoundToInt(temp / (endTime - startTime));
    }

    private WeatherType GetAverageWeatherCode(int startIndex)
    {
        List<WeatherType> currentDayWeatherCodes = new();

        for (int i = startIndex; i < startIndex + 24; i++)
        {
            currentDayWeatherCodes.Add(weatherInfo.hourly.weathercode[i]);
        }
        
        IOrderedEnumerable<IGrouping<WeatherType, WeatherType>> groups = currentDayWeatherCodes.GroupBy(x => x).OrderByDescending(x => x.Count());

        return groups.First().Key;
    }

    /// <summary>
    /// Converts DayOfWeek enum to dutch abbreviation.
    /// </summary>
    /// <param name="dayOfWeek">DayOfTheWeek enum.</param>
    /// <returns>Dutch abbreviation of the DayOfWeek input as string.</returns>
    public static string GetDayAbbreviation(DayOfWeek dayOfWeek)
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
    /// <returns>Name of the day of the week in dutch.</returns>
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
}
