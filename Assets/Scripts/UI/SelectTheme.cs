using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectTheme : MonoBehaviour
{
    public ColorPalette theme;
    
    private IEnumerable<TextMeshProUGUI> TextComponents => FindObjectsOfType<TextMeshProUGUI>(true);
    
    private IEnumerable<GameObject> BackgroundImageComponents => FindAllGameObjectsWithTag("background");
    private IEnumerable<GameObject> ContextBackgroundImageComponents => FindAllGameObjectsWithTag("contextBackground");
    private IEnumerable<GameObject> ButtonImageComponents => FindAllGameObjectsWithTag("button");
    private IEnumerable<GameObject> IconImageComponents => FindAllGameObjectsWithTag("Icon");

    private void OnEnable()
    {
        EventSystem.AddListener(EventType.onSceneReady, UpdateTheme);
    }
    
    private void OnDisable()
    {
        EventSystem.RemoveListener(EventType.onSceneReady, UpdateTheme);
    }
    
    private void Start()
    {
        SetTheme(theme);
        Invoke(nameof(UpdateTheme), 3);
    }

    /// <summary>
    /// Refresh the UI Theme.
    /// </summary>
    public void UpdateTheme()
    {
        SetTheme(theme);
    }

    /// <summary>
    /// Set the theme of the UI.
    /// </summary>
    /// <param name="theme"></param>
    public void SetTheme(ColorPalette theme)
    {
        foreach (TextMeshProUGUI component in TextComponents)
        {
            component.color = theme.textColor;
        }

        foreach (GameObject gameObj in BackgroundImageComponents)
        {
            Image image = gameObj.GetComponent<Image>();
            if (image != null)
            {
                image.color = theme.backgroundColor0;
            }

            TextMeshProUGUI text = gameObj.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.color = theme.backgroundColor0;
            }
        }
        
        foreach (GameObject gameObj in ContextBackgroundImageComponents)
        {
            Image image = gameObj.GetComponent<Image>();
            if (image != null)
            {
                image.color = theme.backgroundColor1;
            }
            
            TextMeshProUGUI text = gameObj.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.color = theme.backgroundColor1;
            }
        }

        foreach (GameObject gameObj in ButtonImageComponents)
        {
            Image image = gameObj.GetComponent<Image>();
            if (image != null)
            {
                image.color = theme.buttonColor;
            }
            
            TextMeshProUGUI text = gameObj.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.color = theme.buttonColor;
            }
        }

        foreach (GameObject gameObj in IconImageComponents)
        {
            Image image = gameObj.GetComponent<Image>();
            if (image != null)
            {
                image.color = theme.iconColor;
            }
            
            TextMeshProUGUI text = gameObj.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.color = theme.iconColor;
            }
        }
    }

    /// <summary>
    /// Find all GameObjects within the current scene with specified tag, includes inactive GameObjects.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static IEnumerable<GameObject> FindAllGameObjectsWithTag(string tag)
    {
        Transform[] transforms = FindObjectsOfType<Transform>(true);

        return (from tf in transforms where tf.CompareTag(tag) select tf.gameObject);
    }
}
