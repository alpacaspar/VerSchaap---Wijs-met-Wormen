using UnityEngine;
using UnityEngine.UI;

public class AdviceDisplay : MonoBehaviour
{
    [SerializeField] private Gradient fillGradient;
    [SerializeField] private Image fillImageComponent;

    public float fill;
    private bool updated;

    private void Update()
    {
        fillImageComponent.fillAmount = fill;
        fillImageComponent.color = fillGradient.Evaluate(fill);
    }
}
