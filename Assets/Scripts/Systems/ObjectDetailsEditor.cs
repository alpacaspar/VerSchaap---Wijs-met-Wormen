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

    public void CreateVisualsForItem<T>(T obj, FieldInfo info = null,  bool expand = false)
    {
        // Get members if the object is a class
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        if (expand)
        {
            var objDetailsPanel = Instantiate(prefabDetailsPanel, transform);
            objDetailsPanel.name = "detail" + currentDepth;
            detailsPanel = objDetailsPanel.GetComponent<ObjectDetailsPanel>();
            detailsPanels.Push(detailsPanel);
        } 

        currentDepth++;

        if (detailsPanel == null)
        {
            currentDepth++;
            // Make a new detailsPanel prefab
            var objDetailsPanel = Instantiate(prefabDetailsPanel, transform);
            objDetailsPanel.name = "detail" + currentDepth;
            detailsPanel = objDetailsPanel.GetComponent<ObjectDetailsPanel>();
            detailsPanels.Push(detailsPanel);
        }

        // Or array
        bool bIsListOrArray = typeof(IList).IsAssignableFrom(obj.GetType());
        bool bIsValueType = obj.GetType().IsValueType;
        bool bIsEnum = obj.GetType().IsEnum;
        //Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.
        bool bIsPrimitive = obj.GetType().IsPrimitive;
        bool bIsString = obj.GetType() == typeof(string);
        bool bIsClass = obj.GetType().IsClass;


        // TODO fix array (if necessary)
        if (bIsListOrArray)
        {
            // Show list contents
            if (!expand)
            {
                // The list button which show the contents if you click it
                var button = detailsPanel.MakeButton(null, obj);
                button.onClick.AddListener(delegate { CreateVisualsForItem(obj, null, true); });
            }

            // Show list as button to expand
            else
            {
                foreach (var item in obj as IList)
                {
                    CreateVisualsForItem(item);
                }
            }
        }

        //Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.
        else if (bIsPrimitive)
        {
            detailsPanel?.CreateInteractableForField(null, obj);
        }

        else if (bIsEnum)
        {
            detailsPanel?.CreateInteractableForField(null, obj);
        }

        else if (bIsString)
        {
            detailsPanel?.CreateInteractableForField(null, obj);
        }

        else if (bIsClass)
        {
            if (!expand)
            {
                // The list button which show the contents if you click it
                var button = detailsPanel.MakeButton(null, obj);
                button.onClick.AddListener(delegate { CreateVisualsForItem(obj, null, true); });
            }

            else
            {

                foreach (var field in fields)
                {
                    CreateVisualsForItem(field.GetValue(obj), field);
                    //var fieldValue = field.GetValue(obj);
                }
            }
        }

        currentDepth--;
    }


    public void GetNamesOfClass<T>(T obj)
    {
        var objDetailsPanel = Instantiate(prefabDetailsPanel, transform);
        objDetailsPanel.name = "detail" + currentDepth;
        detailsPanel = objDetailsPanel.GetComponent<ObjectDetailsPanel>();

        FieldInfo[] fields;
        fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        
        if (obj.GetType() == typeof(string))
        {
            var inputfield = detailsPanel.MakeInputField(null, obj);
            
            //inputfield.onValueChanged.AddListener(delegate { obj = inputfield.text.ToString() as T; });
        }



        /*
        Debug.Log("class=" + obj.GetType().ToString());

        

        foreach (var field in fields)
        {
            {
                var fieldValue = field.GetValue(obj);

                // You would probably need to do a null check
                // somewhere to avoid a NullReferenceException.

                // Check if this is a list/array
                if (typeof(IList).IsAssignableFrom(field.FieldType))
                {
                    // Create a button for a list
                    detailsPanel.MakeButton(field, obj);
                    //CreateDetailPanelFromObject(field.GetValue(obj));

                    // By now, we know that this is assignable from IList, so we can safely cast it.
                    foreach (var item in fieldValue as IList)
                    {
                        FieldInfo[] subInfo = item.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

                        //FieldsAndItem(subInfo, item);
                        foreach (FieldInfo sub in subInfo)
                        {
                            detailsPanel.MakeButton(sub, item);
                        }

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
        */
    }


    public void CreateDetailPanelFromObject<T>(T obj)
    {

        GetNamesOfClass(obj);
        return;
        currentDepth++;
        CreateVisualsForItem(obj, null, true);
    }
}
