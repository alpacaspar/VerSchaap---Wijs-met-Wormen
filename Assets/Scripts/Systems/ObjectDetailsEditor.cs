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
    private Stack<ObjectDetailsPanel> detailsPanels = new Stack<ObjectDetailsPanel>();

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
                // Create a button for a list
                detailsPanel.CreateInteractableForField(field, obj);
                CreateDetailPanelFromObject(field.GetValue(obj));

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

                /*
                if (detailsPanels.Count > 1)
                {
                    detailsPanels.Pop();
                    detailsPanel = detailsPanels.Pop();
                    detailsPanels.Push(detailsPanel);
                }
                */

                if (detailsPanels.Count > 0)
                {
                    var tmpDetailsPanel = detailsPanels.Pop();

                    if (detailsPanels.Count > 0)
                    {
                        detailsPanel = detailsPanels.Pop();
                        detailsPanels.Push(detailsPanel);
                    }

                    else
                    {
                        detailsPanel = tmpDetailsPanel;
                    }
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

        // Make a new detailsPanel prefab
        var objDetailsPanel = Instantiate(prefabDetailsPanel, transform);
        detailsPanel = objDetailsPanel.GetComponent<ObjectDetailsPanel>();
        //detailsPanels.Push(detailsPanel);
        
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        FieldsAndItem(fields, obj);

        /*
        // Remove the panel when its done
        detailsPanels.Pop();

        if (detailsPanels.Count > 0)
        {
            // Set detailsPanel to the upper panel by popping it from the stack
            detailsPanel = detailsPanels.Pop();
            // Add it back to the stack
            detailsPanels.Push(detailsPanel);
        }
        */
    }
}
