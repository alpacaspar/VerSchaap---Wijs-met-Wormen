using UnityEngine;
using UnityEngine.UI;

public class SyncWithDatabase : MonoBehaviour
{
    [SerializeField] private Button syncButton;
    private bool isRunning;

    private void OnEnable()
    {
        EventSystem.AddListener(EventType.syncedWithDatabase, OnDatabaseSynced);
    }

    private void OnDisable()
    {
        EventSystem.RemoveListener(EventType.syncedWithDatabase, OnDatabaseSynced);
    }
    
    public void Sync()
    {
        syncButton.interactable = false;
        EventSystem.InvokeEvent(EventType.performSync);
    }

    private void OnDatabaseSynced()
    {
        syncButton.interactable = true;
    }
}
