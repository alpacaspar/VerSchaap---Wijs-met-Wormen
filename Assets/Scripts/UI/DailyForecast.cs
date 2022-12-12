using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyForecast : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayTextComponent;
    [SerializeField] private Image weatherIconComponent;
    [SerializeField] private TextMeshProUGUI dayTemperatureTextComponent;
    [SerializeField] private TextMeshProUGUI nightTemperatureTextComponent;

    private WeeklyForecast weeklyForecast;
    private int dayIndex;

    /// <summary>
    /// Set the UI elements of the daily forecast container.
    /// </summary>
    /// <param name="dayAbb">Abbreviation of the specified day.</param>
    /// <param name="weatherIcon">Relevant sprite of the forecasted weather.</param>
    /// <param name="dayTemp">Temperature during daylight. (dawn to dusk)</param>
    /// <param name="nightTemp">Temperature during the night. (dusk to dawn)</param>
    public void SetDailyForecastData(string dayAbb, Sprite weatherIcon, float dayTemp, float nightTemp, WeeklyForecast weeklyForecast, int dayIndex)
    {
        if (dayTextComponent != null)
        {
            dayTextComponent.text = dayAbb;
        }

        if (weatherIconComponent != null)
        {
            weatherIconComponent.sprite = weatherIcon;
        }

        if (dayTemperatureTextComponent != null)
        {
            dayTemperatureTextComponent.text = $"{dayTemp}°";
        }

        if (nightTemperatureTextComponent != null)
        {
            nightTemperatureTextComponent.text = $"{nightTemp}°";
        }

        this.weeklyForecast = weeklyForecast;
        this.dayIndex = dayIndex;
    }

    public void OnButtonPressed()
    {
        weeklyForecast.selectedDayIndex = dayIndex;
    }
}
