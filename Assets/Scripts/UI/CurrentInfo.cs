using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentTimeTextComponent;
    [SerializeField] private TextMeshProUGUI currentDateTextComponent;

    [SerializeField] private Image weatherIconImageComponent;
    [SerializeField] private WeatherIconsObject weatherIconsObject;
    [SerializeField] private TextMeshProUGUI temperatureTextComponent;

    private WeatherInfo weatherInfo;

    private CurrentInfo()
    {
        EventSystem<WeatherInfo>.AddListener(EventType.weatherDataReceived, OnWeatherDataReceived);
    }

    private void OnDestroy()
    {
        EventSystem<WeatherInfo>.RemoveListener(EventType.weatherDataReceived, OnWeatherDataReceived);
    }

    private void OnWeatherDataReceived(WeatherInfo info)
    {
        weatherInfo = info;
        VerweidAdvisor advisor = FindObjectOfType<VerweidAdvisor>();
        if (advisor != null) advisor.Weather = info;
    }

    private void FixedUpdate()
    {
        SetCurrentTime();
        SetCurrentDate();
        SetCurrentWeatherIcon();
        SetCurrentTemperature();
    }

    private void SetCurrentTime()
    {
        if (!currentTimeTextComponent) return;

        DateTime now = DateTime.Now;
        currentTimeTextComponent.text = $"{now.Hour}:{MakeDateTimeDisplayReady(now.Minute)}";
    }

    private void SetCurrentDate()
    {
        if (!currentDateTextComponent) return;

        DateTime now = DateTime.Now;
        CultureInfo info = new CultureInfo("nl-NL");
        string monthName = info.DateTimeFormat.GetMonthName(now.Month);
        string week = info.DateTimeFormat.AbbreviatedDayNames[(int)now.DayOfWeek];

        currentDateTextComponent.text = $"{week} {now.Day} {monthName}";
    }
    
    private void SetCurrentTemperature()
    {
        if (!temperatureTextComponent) return;
        if (weatherInfo == null) return;
        
        int temp = Mathf.RoundToInt(weatherInfo.hourly.temperature_2m[GetWeatherInfoIndex()]);
        temperatureTextComponent.text = $"{temp}°";
    }

    private void SetCurrentWeatherIcon()
    {
        if (!weatherIconImageComponent) return;
        if (weatherInfo == null) return;

        weatherIconImageComponent.sprite = weatherIconsObject.WeatherIcons[weatherInfo.hourly.weathercode[GetWeatherInfoIndex()]];
    }

    private int GetWeatherInfoIndex()
    {
        return Array.IndexOf(weatherInfo.hourly.time, ConvertDateTimeToWeatherTime());
    }

    private string ConvertDateTimeToWeatherTime()
    {
        DateTime now = DateTime.Now;

        return $"{now.Year}-{now.Month}-{MakeDateTimeDisplayReady(now.Day)}T{MakeDateTimeDisplayReady(now.Hour)}:00";
    }

    private string MakeDateTimeDisplayReady(int input)
    {
        return  (input < 10 ? "0" : "") + input;
    }
}
