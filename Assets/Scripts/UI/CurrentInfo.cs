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
        EventSystem<WeatherInfo>.AddListener(EventType.performWeatherUpdate, OnWeatherDataReceived);
    }

    private void OnDestroy()
    {
        EventSystem<WeatherInfo>.RemoveListener(EventType.performWeatherUpdate, OnWeatherDataReceived);
    }

    private void OnWeatherDataReceived(WeatherInfo info)
    {
        weatherInfo = info;
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
        string dayOfWeek = info.DateTimeFormat.AbbreviatedDayNames[(int)now.DayOfWeek];

        currentDateTextComponent.text = $"{dayOfWeek} {now.Day} {monthName}";
    }
    
    private void SetCurrentTemperature()
    {
        if (!temperatureTextComponent) return;
        if (weatherInfo == null) return;
        
        int temp = Mathf.RoundToInt(weatherInfo.hourly.temperature_2m[GetCurrentWeatherInfoIndex()]);
        temperatureTextComponent.text = $"{temp}°";
    }

    private void SetCurrentWeatherIcon()
    {
        if (!weatherIconImageComponent) return;
        if (weatherInfo == null) return;

        weatherIconImageComponent.sprite = weatherIconsObject.WeatherIcons[weatherInfo.hourly.weathercode[GetCurrentWeatherInfoIndex()]];
    }

    private int GetCurrentWeatherInfoIndex()
    {
        return DateTime.Now.Hour;
    }

    private string MakeDateTimeDisplayReady(int input)
    {
        return  (input < 10 ? "0" : "") + input;
    }
}
