using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectTheme : MonoBehaviour
{
    public ColorPalette theme;
    
    private TextMeshProUGUI[] TextComponents => FindObjectsOfType<TextMeshProUGUI>();
    
    private List<GameObject> BackgroundImageComponents => FindAllGameObjectsWithTag("background");
    private List<GameObject> ContextBackgroundImageComponents => FindAllGameObjectsWithTag("contextBackground");
    private List<GameObject> ButtonImageComponents => FindAllGameObjectsWithTag("button");
    private List<GameObject> IconImageComponents => FindAllGameObjectsWithTag("Icon");

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

    public void UpdateTheme()
    {
        SetTheme(theme);
    }

    public void SetTheme(ColorPalette theme)
    {
        foreach (TextMeshProUGUI component in TextComponents)
        {
            component.color = theme.textColor;
        }

        foreach (GameObject gameObj in BackgroundImageComponents)
        {
            gameObj.GetComponent<Image>().color = theme.backgroundColor0;
        }
        
        foreach (GameObject gameObj in ContextBackgroundImageComponents)
        {
            gameObj.GetComponent<Image>().color = theme.backgroundColor1;
        }

        foreach (GameObject gameObj in ButtonImageComponents)
        {
            gameObj.GetComponent<Image>().color = theme.buttonColor;
        }

        foreach (GameObject gameObj in IconImageComponents)
        {
            gameObj.GetComponent<Image>().color = theme.iconColor;
        }
    }

    public static List<GameObject> FindAllGameObjectsWithTag(string tag)
    {
        Transform[] transforms = FindObjectsOfType<Transform>(true);

        return (from tf in transforms where tf.CompareTag(tag) select tf.gameObject).ToList();
    }
}
