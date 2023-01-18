using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmAudioSource : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioSource AudioSource => audioSource ??= GetComponent<AudioSource>();

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
        AudioSource.Play();
    }
}
