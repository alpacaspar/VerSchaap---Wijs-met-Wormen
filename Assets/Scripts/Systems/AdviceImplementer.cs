using UnityEngine;
using UnityEngine.Events;

public class AdviceImplementer : MonoBehaviour
{
    public int weeksOnDisplay = 4;
    public int DaysOnDisplay => weeksOnDisplay * 7;

    public UnityEvent<int> onUpdateDisplaySize;
    public UnityEvent<int> onUpdateAdvice;

    private void Start()
    {
        onUpdateDisplaySize?.Invoke(weeksOnDisplay);
    }

    private void OnEnable()
    {
        UpdateAdvice();
    }

    public void UpdateAdvice()
    {
        for (int i = 0; i < DaysOnDisplay; i++)
        {
            // Use the function for the advice formula
            // if function returns true > onUpdateAdvice?.Invoke(i); break;
        }
    }
}
