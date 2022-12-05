using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ObjectDetailsEditor : MonoBehaviour
{
    public int maxDepth = 10;
    private int currentDepth = 0;
    public GameObject prefabDetailsPanel;

    public ObjectDetailsPanel detailsPanel;

    public void FieldsAndItem<T>(FieldInfo[] fields, T obj)
    {
        // button for the class / object
        detailsPanel.CreateInteractableForField(null, obj);
        
        foreach (var field in fields)
        {
            var fieldValue = field.GetValue(obj);

            // You would probably need to do a null check
            // somewhere to avoid a NullReferenceException.

            // Check if this is a list/array
            if (typeof(IList).IsAssignableFrom(field.FieldType))
            {
                //detailsPanel.CreateInteractableForField(field, obj);
                
                // By now, we know that this is assignable from IList, so we can safely cast it.
                foreach (var item in fieldValue as IList)
                {
                    FieldInfo[] subInfo = item.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

                    FieldsAndItem(subInfo, item);
                    /*foreach (FieldInfo sub in subInfo)
                    {
                        detailsPanel.CreateButtonFromField(sub, item);
                    }
                    */
                    
                    // Do you want to know the item type?
                    //var itemType = item.GetType();

                    // Do what you want with the items.
                    //detailsPanel.CreateButtonFromField(field, item);
                }
            }
            else
            {
                detailsPanel.CreateInteractableForField(field, obj);
            }
        }
    }
    
    public void CreateDetailPanelFromObject<T>(T obj)
    {
        currentDepth++;
        //if (obj == default) return;

        var objDetailsPanel = Instantiate(prefabDetailsPanel, transform);
        detailsPanel = objDetailsPanel.GetComponent<ObjectDetailsPanel>();
        FieldInfo[] fields = obj.GetType().GetFields();

        FieldsAndItem(fields, obj);

    }
}
