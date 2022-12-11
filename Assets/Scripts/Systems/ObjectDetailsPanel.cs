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

    public TMP_InputField MakeInputField<T>(FieldInfo f, T item)
    {
        var objInteractable = Instantiate(prefabInputField, pnlContent.transform);
        var interactable = objInteractable.GetComponent<TMP_InputField>();
        interactable.SetTextWithoutNotify(f?.GetValue(item).ToString());
        interactable.onValueChanged.AddListener(delegate { f?.SetValue(item, interactable.text); });
        return interactable;
    }

    public Button MakeButton<T>(FieldInfo f, T item)
    {
        var objButton = Instantiate(prefabButton, pnlContent.transform);
        var txtButton = objButton.GetComponentInChildren<TextMeshProUGUI>();
        txtButton.SetText(f != null ? f.Name : item.GetType().ToString());
        return objButton.GetComponent<Button>();
    }

    public void CreateInteractableForField<T>(FieldInfo f, T item)
    {
        Type itemType = f == null ? item.GetType() : f.FieldType;

        if (itemType == typeof(string))
        {
            MakeInputField(f, item);
        }

        else if (itemType == typeof(float) || itemType == typeof(int) || itemType == typeof(long))
        {
            var objInteractable = Instantiate(prefabInputField, pnlContent.transform);
            var interactable = objInteractable.GetComponent<TMP_InputField>();
            interactable.contentType = TMP_InputField.ContentType.DecimalNumber;
            interactable.SetTextWithoutNotify(f?.GetValue(item).ToString());
            interactable.onValueChanged.AddListener(delegate { f?.SetValue(item, Convert.ToSingle(interactable.text)); });
        }

        else
        {
            MakeButton(f, item);

        }
    }
}
