using System.Collections.Generic;
using UnityEngine;

public class AdviceDisplayManager : MonoBehaviour
{
    private List<WeideObject> weideObjects = new();
    
    private void Start()
    {
        CronScheduler.instance.SetRepeat(this,"UpdateAdviceDisplays", 3600);
    }

    // OLD
    private void UpdateAdviceDisplays()
    {
        EventSystem.InvokeEvent(EventType.performAdviceUpdate);
    }

    private void GetWeideObjects()
    {
        weideObjects.Clear();
        
        var results = WurmAPI.MethodHandler<WeideObject>(MethodType.Get, null);
        foreach (var result in results)
        {
            weideObjects.Add(JsonUtility.FromJson<WeideObject>(result));
        }
    }
}
