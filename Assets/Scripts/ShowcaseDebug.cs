using UnityEngine;

public class ShowcaseDebug : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventSystem.InvokeEvent(EventType.onAlarmRang);
        }
    }
}
