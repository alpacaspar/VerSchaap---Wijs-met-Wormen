using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FontSizeSettings : MonoBehaviour
{
    private readonly Dictionary<TextMeshProUGUI, float> textAssets = new();
    private Dictionary<TextMeshProUGUI, float> TextAssets
    {
        get
        {
            if (textAssets.Count == 0)
            {
                var assets = FindObjectsOfType<TextMeshProUGUI>();
                foreach (var asset in assets)
                {
                    textAssets.Add(asset, asset.fontSize);
                }
            }

            return textAssets;
        }
    }

    [SerializeField] private Slider fontSizeSlider;

    /// <summary>
    /// Changes the font sizes of all text elements in the scene.
    /// Subscribe this function to the OnSliderValueChanged UnityEvent on a slider component.
    /// </summary>
    public void OnSliderValueChanged()
    {
        foreach (var (textElement, size) in TextAssets)
        {
            textElement.fontSize = size + fontSizeSlider.value;
        }
    }
}
