using UnityEngine;

public class ArduinoConnector : MonoBehaviour
{
    public SerialController serialController;
    private bool messageSent = true;

    private void OnEnable()
    {
        EventSystem<WeideObject>.AddListener(EventType.onAlarmRang, OnAlarmRang);
    }

    private void OnDisable()
    {
        EventSystem<WeideObject>.RemoveListener(EventType.onAlarmRang, OnAlarmRang);
    }

    private void OnAlarmRang(WeideObject weideObject)
    {
        serialController.SendSerialMessage("verweiden");
        Debug.Log("Sent verweiden to Serial Controller...");
        messageSent = false;
    }

    private void Start()
    {
        serialController = FindObjectOfType<SerialController>();
        
        // For testing only
        EventSystem<WeideObject>.InvokeEvent(EventType.onAlarmRang, new WeideObject());
    }

    // use this function if messages are not sent correctly through 
    // private void Update()
    // {
    //     if (messageSent) return;
    //
    //     serialController.SendSerialMessage("verweiden");
    //
    //     messageSent = true;
    // }
}
