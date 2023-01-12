using UnityEngine;
using UnityEngine.UI;

public class AdviceDisplay : MonoBehaviour
{
    [SerializeField] private Gradient fillGradient;
    public Image fillImageComponent;

    private float fill;
    private bool updated;
    
    private WeatherInfo info;

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
        if (updated) return;
        
        fillImageComponent.fillAmount = fill;
        fillImageComponent.color = fillGradient.Evaluate(fill);
            
        updated = true;
    }
}
