using UnityEngine;

public class AdviceDisplayManager : MonoBehaviour
{
    private void Awake()
    {
        CronScheduler.instance.SetRepeat(this,"UpdateAdviceDisplays", 3600);
    }

    private void UpdateAdviceDisplays()
    {
        EventSystem.InvokeEvent(EventType.performAdviceUpdate);
    }
}
