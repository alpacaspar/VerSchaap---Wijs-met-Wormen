using UnityEngine;
using UnityEngine.UI;

public class QuickAdviceDisplay : MonoBehaviour
{
    [SerializeField] private Gradient colorGradient;
    
    private Image[] dots;
    private Image[] Dots => dots ??= GetComponentsInChildren<Image>();

    /// <summary>
    /// Test function. Uses the surface quality instead of the value from the verweid formula
    /// </summary>
    public void UpdateDotsBasedOnQuality(float surfaceQuality)
    {
        for (int i = 0; i < Dots.Length; i++)
        {
            Dots[i].color = 100.0f / Dots.Length * i > 100 - surfaceQuality ? Color.gray : colorGradient.Evaluate(1 - surfaceQuality / 100f);
        }
    }

    // TODO: Change "Update" function to event function that fires with every clock update.
    private void Update()
    {
        return;
        // TODO: calculate "value" variable based on formula.
        float value = 0f;

        value = Mathf.InverseLerp(-50f, 200f, value);
        value = Mathf.Clamp01(value);

        int amount = Mathf.CeilToInt(value * Dots.Length);

        for (int i = 0; i < Dots.Length; i++)
        {
            Dots[i].color = i < amount ? colorGradient.Evaluate((amount - 1) / (Dots.Length - 1f)) : Color.gray;
        }
    }
}
