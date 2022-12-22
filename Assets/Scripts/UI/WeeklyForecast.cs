using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class WeeklyForecast : MonoBehaviour
{
    [SerializeField] private GameObject dailyForecastPrefab;
    [SerializeField] private Transform dailyForecastParent;

    [SerializeField] private WeatherInfo weatherInfo;

    [SerializeField] private WeatherIconsObject weatherIconsObject;
    
    public int selectedDayIndex;

    private WeeklyForecast()
    {
        EventSystem<WeatherInfo>.AddListener(EventType.performWeatherUpdate, OnWeatherDataReceived);
    }

    private void OnDestroy()
    {
        EventSystem<WeatherInfo>.RemoveListener(EventType.performWeatherUpdate, OnWeatherDataReceived);
    }

    private void OnWeatherDataReceived(WeatherInfo info)
    {
        weatherInfo = info;

        for (int i = 0; i < dailyForecastParent.childCount; i++)
        {
            Destroy(dailyForecastParent.GetChild(i).gameObject);
        }

        int day = 0;
        for (int i = 0; i < weatherInfo.hourly.time.Length; i++)
        {
            if (i % 24 == 0)
            {
                CultureInfo cultureInfo = new CultureInfo("nl-NL");
                
                int forecastDay = ((int)DateTime.Today.DayOfWeek + day) % 7;
                string dayAbbreviation = cultureInfo.DateTimeFormat.AbbreviatedDayNames[forecastDay];
                Sprite sprite = weatherIconsObject.WeatherIcons[GetAverageWeatherCode(i)];
                
                GameObject instance = Instantiate(dailyForecastPrefab, dailyForecastParent);
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
}
