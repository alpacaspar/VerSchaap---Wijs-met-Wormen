using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Weather Icon Data", menuName = "ScriptableObjects/WeatherIconsScriptableObject", order = 1)]
public class WeatherIconsObject : ScriptableObject
{
    [SerializeField] private WeatherIcon[] icons;

    public Dictionary<WeatherType, Sprite> WeatherIcons => icons.ToDictionary(x => x.weatherType, x => x.icon);

    [Serializable]
    private class WeatherIcon
    {
        public WeatherType weatherType;
        public Sprite icon;
    }
}