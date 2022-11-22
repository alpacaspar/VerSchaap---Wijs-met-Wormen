using System;
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

    [SerializeField] private WeeklyForecast weeklyForecastComponent;

    [SerializeField] private Slider timeSlider;

    private LocationInfo locationInfo;
    private WeatherInfo weatherInfo;

    private int dayOfWeek;

    private void OnEnable()
    {
        EventSystem<WeatherInfo>.AddListener(EventType.weatherDataReceived, OnWeatherDataReceived);
        EventSystem<LocationInfo>.AddListener(EventType.locationDataReceived, OnLocationDataReceived);
    }

    private void OnDisable()
    {
        EventSystem<WeatherInfo>.RemoveListener(EventType.weatherDataReceived, OnWeatherDataReceived);
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
        var index = weeklyForecastComponent.selectedDayIndex * 24 + (int)timeSlider.value;
        var time = weatherInfo.hourly.time[index].Split("T");
        var forecastDay = ((int)DateTime.Today.DayOfWeek + weeklyForecastComponent.selectedDayIndex) % 7;

        var dateTime = $"{WeeklyForecast.GetDay((DayOfWeek)forecastDay)} {time[1]}";
        UpdateInfo(locationInfo.city, dateTime, weatherInfo.hourly.temperature_2m[index], weatherInfo.hourly.precipitation[index], weatherInfo.hourly.relativehumidity_2m[index]);
    }

    private void UpdateInfo(string location, string time, float temp, int precipitation, int humidity)
    {
        locationTextComponent.text = location;
        timeTextComponent.text = time;
        temperatureTextComponent.text = $"temperatuur: {temp}°";
        precipitationTextComponent.text = $"neerslag: {precipitation}%";
        humidityTextComponent.text = $"luchtvochtigheid: {humidity}%";
    }
}
