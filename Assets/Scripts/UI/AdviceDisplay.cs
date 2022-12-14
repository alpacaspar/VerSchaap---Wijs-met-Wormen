using UnityEngine;
using UnityEngine.UI;

public class AdviceDisplay : MonoBehaviour
{
    public int weeksOnDisplay = 4;
    public int DaysOnDisplay => weeksOnDisplay * 7;

    [SerializeField] private Image estimateFillImageComponent;
    [SerializeField] private Image adviceFillImageComponent;

    private void Start()
    {
        SetAdviceFillSize();
        SetAdviceFillPosition(8);
        
        SetEstimateFillSize();
        SetEstimateFillPosition(1);
    }
    
    /// <summary>
    /// Sets the size of the advice needle according to the amount of days that the diagram can display.
    /// </summary>
    public void SetAdviceFillSize()
    {
        adviceFillImageComponent.fillAmount = 1f / DaysOnDisplay;
    }

    /// <summary>
    /// Sets the position of the advice needle according to dayIndex integer.
    /// </summary>
    /// <param name="dayIndex">Index of the day, starts from 0.</param>
    public void SetAdviceFillPosition(int dayIndex)
    {
        adviceFillImageComponent.rectTransform.eulerAngles = Vector3.forward * (360f / DaysOnDisplay * -dayIndex);
    }
    
    /// <summary>
    /// Sets the size of the estimate needle according to the amount of weeks that the diagram can display.
    /// </summary>
    public void SetEstimateFillSize()
    {
        estimateFillImageComponent.fillAmount = 1f / weeksOnDisplay;
    }

    /// <summary>
    /// Sets the position of the estimate needle according to weekIndex integer.
    /// </summary>
    /// <param name="dayIndex">Index of the week, starts from 0.</param>
    public void SetEstimateFillPosition(int weekIndex)
    {
        estimateFillImageComponent.rectTransform.eulerAngles = Vector3.forward * (360f / weeksOnDisplay * -weekIndex);
    }
}
