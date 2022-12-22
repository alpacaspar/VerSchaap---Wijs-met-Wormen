using UnityEngine;
using UnityEngine.UI;

public class AdviceDisplay : MonoBehaviour
{
    public float lat;
    public float lon;
    public WeatherInfo info;
    
    public Image fillImageComponent;

    private float fill;
    private bool updated;

    private void Start()
    {
        EventSystem.AddListener(EventType.performAdviceUpdate, OnAdviceUpdate);
    }
    
    private void OnDestroy()
    {
        EventSystem.RemoveListener(EventType.performAdviceUpdate, OnAdviceUpdate);
    }

    /// <summary>
    /// Perform Update to Advice Display.
    /// </summary>
    public void OnAdviceUpdate()
    {
        if (info == null) return;
        fill = Mathf.InverseLerp(-50f, 200f, (float)VerweidAdvisor.CalcValue(info));

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
