using UnityEngine;

public class TestAlarm : MonoBehaviour
{
    public void Test()
    {
        EventSystem.InvokeEvent(EventType.onAlarmRang);
    }
}
