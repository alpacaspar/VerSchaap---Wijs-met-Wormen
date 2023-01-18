using UnityEngine;

public class ArduinoConnector : MonoBehaviour
{
    public SerialController serialController;
    private bool messageSent = true;

    private void OnEnable()
    {
        EventSystem<LotObject>.AddListener(EventType.onAlarmRang, OnAlarmRang);
    }

    private void OnDisable()
    {
        EventSystem<LotObject>.RemoveListener(EventType.onAlarmRang, OnAlarmRang);
    }

    private void OnAlarmRang(LotObject LotObject)
    {
        serialController.SendSerialMessage("verLotn");
        Debug.Log("Sent verLotn to Serial Controller...");
        messageSent = false;
    }

    private void Start()
    {
        serialController = FindObjectOfType<SerialController>();
        
        // For testing only
        EventSystem<LotObject>.InvokeEvent(EventType.onAlarmRang, new LotObject());
    }

    // use this function if messages are not sent correctly through 
    // private void Update()
    // {
    //     if (messageSent) return;
    //
    //     serialController.SendSerialMessage("verLotn");
    //
    //     messageSent = true;
    // }
}
