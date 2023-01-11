using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HourlyForecast : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI locationTextComponent;
    [SerializeField] private TextMeshProUGUI timeTextComponent;
    [SerializeField] private TextMeshProUGUI temperatureTextComponent;
    [SerializeField] private TextMeshProUGUI precipitationTextComponent;
    [SerializeField] private TextMeshProUGUI humidityTextComponent;
    [SerializeField] private Image weatherIconComponent;
    [SerializeField] private WeatherIconsObject weatherIconsObject;

    [SerializeField] private WeeklyForecast weeklyForecastComponent;

    [SerializeField] private Slider timeSlider;

    private LocationInfo locationInfo;
    private WeatherInfo weatherInfo;

    private int dayOfWeek;

    private HourlyForecast()
    {
        EventSystem<WeatherInfo>.AddListener(EventType.performWeatherUpdate, OnWeatherDataReceived);
        EventSystem<LocationInfo>.AddListener(EventType.locationDataReceived, OnLocationDataReceived);
    }

    private void OnDestroy()
    {
        EventSystem<WeatherInfo>.RemoveListener(EventType.performWeatherUpdate, OnWeatherDataReceived);
        EventSystem<LocationInfo>.RemoveListener(EventType.locationDataReceived, OnLocationDataReceived);
    }

    private void OnWeatherDataReceived(WeatherInfo info)
    {
        weatherInfo = info;
    }

    private void OnLocationDataReceived(LocationInfo info)
    {
        locationInfo = info;
    }

    private void FixedUpdate()
    {
        if (weatherInfo == null) return;
        int index = weeklyForecastComponent.selectedDayIndex * 24 + (int)timeSlider.value;
        string[] time = weatherInfo.hourly.time[index].Split("T");
        int forecastDay = ((int)DateTime.Today.DayOfWeek + weeklyForecastComponent.selectedDayIndex) % 7;

        CultureInfo cultureInfo = new CultureInfo("nl-NL");
        string dateTime = $"{cultureInfo.DateTimeFormat.GetDayName((DayOfWeek)forecastDay)} {time[1]}"; 
        Sprite weatherIcon = weatherIconsObject.WeatherIcons[weatherInfo.hourly.weathercode[index]];
        UpdateInfo(locationInfo == null ? "Nederland" : locationInfo.city, dateTime, weatherInfo.hourly.temperature_2m[index], weatherInfo.hourly.precipitation[index], weatherInfo.hourly.relativehumidity_2m[index], weatherIcon);
    }

    private void UpdateInfo(string location, string time, float temp, int precipitation, int humidity, Sprite weatherIcon)
    {
        locationTextComponent.text = location;
        timeTextComponent.text = time;
        temperatureTextComponent.text = $"temperatuur: {temp}°";
        precipitationTextComponent.text = $"neerslag: {precipitation}%";
        humidityTextComponent.text = $"vochtigheid: {humidity}%";
        weatherIconComponent.sprite = weatherIcon;
    }
}
