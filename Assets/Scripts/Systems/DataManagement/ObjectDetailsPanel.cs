using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using TMPro;
using System;

public class ObjectDetailsPanel : MonoBehaviour
{
    public GameObject prefabButton;
    public GameObject prefabInputField;
    public GameObject pnlContent;

    public void CreateInteractableForField<T>(FieldInfo f, T item)
    {
        Type itemType = f == null ? item.GetType() : f.FieldType;

        if (itemType == typeof(string))
        {   
            var objInteractable = Instantiate(prefabInputField, pnlContent.transform);
            var interactable = objInteractable.GetComponent<TMP_InputField>();
            interactable.SetTextWithoutNotify(f?.GetValue(item).ToString());
            interactable.onValueChanged.AddListener(delegate { f?.SetValue(item, interactable.text); });
        }
        else if (itemType == typeof(float))
        {
            var objInteractable = Instantiate(prefabInputField, pnlContent.transform);
            var interactable = objInteractable.GetComponent<TMP_InputField>();
            interactable.contentType = TMP_InputField.ContentType.DecimalNumber;
            interactable.SetTextWithoutNotify(f?.GetValue(item).ToString());
            interactable.onValueChanged.AddListener(delegate { f?.SetValue(item, Convert.ToSingle(interactable.text)); });
        }

        else
        {   
            var objButton = Instantiate(prefabButton, pnlContent.transform);
            var txtButton = objButton.GetComponentInChildren<TextMeshProUGUI>();
            txtButton.SetText(f != null ? f.Name : item.GetType().ToString());
        }
    }
}
