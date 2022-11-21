using UnityEngine;

public class WeeklyForecast : MonoBehaviour
{
    [SerializeField] private GameObject dailyForecastPrefab;
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

        for (var day = 0; day < 7; day++)
        {
            var instance = Instantiate(dailyForecastPrefab, transform);
            instance.GetComponent<DailyForecast>().SetDailyForecastData("", null, 2, 2);
        }
    }
}
