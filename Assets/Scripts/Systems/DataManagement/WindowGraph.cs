using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField]
    private Sprite circleSprite;
    private RectTransform graphContainer;
    
    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        //CreateCircle(new Vector2(200, 200));

        List<int> values = new List<int>() { 5, 10, 20, 30, 35, 40 };
        ShowGraph(values);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject go = new GameObject("circle", typeof(Image));
        go.transform.SetParent(graphContainer, false);
        go.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        //rectTransform.localScale = Vector3.one * .1f;
        return go;
    }

    private void ShowGraph(List<int> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMax = 100f;
        float xSize = 50f;

        GameObject lastObj = null;

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMax) * graphHeight;
            GameObject obj = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastObj != null) CreateDotConnection(lastObj.GetComponent<RectTransform>().anchoredPosition, obj.GetComponent<RectTransform>().anchoredPosition);
            lastObj = obj;
        }
    }

    private void CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        GameObject go = new GameObject("dotconnection", typeof(Image));
        go.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(100, 3f);
        rectTransform.anchoredPosition = dotPosA;
    }
}
