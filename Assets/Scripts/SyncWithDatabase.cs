using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SyncWithDatabase : MonoBehaviour
{
    [SerializeField] private Button syncButton;
    [SerializeField] private float buttonTimeout = 5;

    private bool isRunning;
    
    public void Sync()
    {
        StartCoroutine(SyncDatabase());
    }

    private IEnumerator SyncDatabase()
    {
        syncButton.interactable = false;

        yield return new WaitForSeconds(buttonTimeout);
        
        syncButton.interactable = true;
    }
}
