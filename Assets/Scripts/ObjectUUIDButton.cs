using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectUUIDButton : MonoBehaviour
{
    public Button button;
    public Button btnDelete;
    public ObjectUUID element;
    
    public virtual void OnButtonClicked()
    {

    }

    public virtual void OnDeleteButtonClicked()
    {

    }

    public virtual void OnStart()
    {
        button.onClick.AddListener(delegate { OnButtonClicked(); });
        btnDelete.onClick.AddListener(delegate { OnDeleteButtonClicked(); });
    }
    
    private void Start()
    {
        OnStart();
    }

}
