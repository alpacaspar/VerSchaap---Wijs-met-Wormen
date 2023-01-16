using UnityEngine;

[CreateAssetMenu(fileName = "New Color Palette", menuName = "ScriptableObjects/Color Palette")]
public class ColorPalette : ScriptableObject
{
    public Color backgroundColor0;
    public Color backgroundColor1;
    public Color textColor;
    public Color buttonColor;
    public Color iconColor;
}
