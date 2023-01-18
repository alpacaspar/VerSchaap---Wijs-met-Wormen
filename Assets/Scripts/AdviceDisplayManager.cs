using System.Collections.Generic;
using UnityEngine;

public class AdviceDisplayManager : MonoBehaviour
{
    private List<LotObject> LotObjects = new();
    
    private void Start()
    {
        CronScheduler.instance.SetRepeat(this,"UpdateAdviceDisplays", 3600);
    }

    // OLD
    private void UpdateAdviceDisplays()
    {
        EventSystem.InvokeEvent(EventType.performAdviceUpdate);
    }

    private void GetLotObjects()
    {
        LotObjects.Clear();
        
        var results = WurmAPI.MethodHandler<LotObject>(MethodType.Get, null);
        foreach (var result in results)
        {
            LotObjects.Add(JsonUtility.FromJson<LotObject>(result));
        }
    }
}
