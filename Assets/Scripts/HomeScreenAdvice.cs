using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HomeScreenAdvice : MonoBehaviour
{
    [SerializeField] private AdviceDisplay topAdviceDisplay;
    [SerializeField] private TextMeshProUGUI topAdviceTextComponent;
    
    [SerializeField] private QuickAdviceUIData[] quickAdviceDisplays;
    
    private WeatherInfo weatherInfo;

    private HomeScreenAdvice()
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
        UpdateHomeScreenAdvice();
    }

    public void UpdateHomeScreenAdvice()
    {
        if (weatherInfo == null) return;
        
        List<LotObject> lots = Database.GetLotCollection(); // TODO: Use updated database request method.

        LotObject[] sorted = lots.OrderByDescending(lot => VerweidAdvisor.CalcValue(weatherInfo, lot.surfaceSqrMtr, lot.currentSheeps.Count)).ToArray();

        if (sorted.Length == 0) return;

        float topLotValue = (float)VerweidAdvisor.CalcValue(weatherInfo, sorted[0].surfaceSqrMtr, sorted[0].currentSheeps.Count);
        topAdviceDisplay.fill = Mathf.InverseLerp(-50, 200, topLotValue);
        topAdviceTextComponent.text = sorted[0].perceelName;

        for (int i = 0; i < quickAdviceDisplays.Length; i++)
        {
            if (i + 1 > sorted.Length - 1) break;
            float lotValue = (float)VerweidAdvisor.CalcValue(weatherInfo, sorted[i + 1].surfaceSqrMtr, sorted[i + 1].currentSheeps.Count);
            quickAdviceDisplays[i].quickAdviceDisplay.UpdateDots(lotValue);
            quickAdviceDisplays[i].lotNameTextComponent.text = sorted[i + 1].perceelName;
        }
    }

    [Serializable]
    private struct QuickAdviceUIData
    {
        public QuickAdviceDisplay quickAdviceDisplay;
        public TextMeshProUGUI lotNameTextComponent;
    }
}
