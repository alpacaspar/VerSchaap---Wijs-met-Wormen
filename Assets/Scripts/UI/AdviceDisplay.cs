using UnityEngine;
using UnityEngine.UI;

public class AdviceDisplay : MonoBehaviour
{
    public Image fillImageComponent;

    private float fill;
    private bool updated;

    private AdviceDisplay()
    {
        EventSystem<double>.AddListener(EventType.onAdviceValueCalculated, OnAdviceUpdate);
    }
    
    private void OnDestroy()
    {
        EventSystem<double>.RemoveListener(EventType.onAdviceValueCalculated, OnAdviceUpdate);
    }

    /// <summary>
    /// Update Advice UI.
    /// </summary>
    /// <param name="value">The calculated value received from the event system.</param>
    public void OnAdviceUpdate(double value)
    {
        fill = Mathf.InverseLerp(-50f, 200f, (float)value);

        updated = false;
    }

    private void Update()
    {
        if (!updated)
        {
            fillImageComponent.fillAmount = fill;
            updated = true;
        }
    }
}
