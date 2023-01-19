using UnityEngine;

public class ArduinoConnector : MonoBehaviour
{
    public SerialController serialController;
    private bool messageSent = true;

    private void OnEnable()
    {
        EventSystem.AddListener(EventType.onAlarmRang, OnAlarmRang);
    }

    private void OnDisable()
    {
        EventSystem.RemoveListener(EventType.onAlarmRang, OnAlarmRang);
    }

    private void OnAlarmRang()
    {
        messageSent = false;
    }

    private void Start()
    {
        serialController = FindObjectOfType<SerialController>();
    }

    private void Update()
    {
        if (!messageSent)
        {
            serialController.SendSerialMessage("A");

            messageSent = true;
        }

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
            Debug.Log("Message arrived: " + message);

    }
}
