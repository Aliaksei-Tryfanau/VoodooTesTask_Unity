using UnityEngine;

public enum Color
{
    Undefined = 0,
    Blue = 1,
    Green = 2,
    Red = 3
}

[System.Serializable]
public struct ColorModifier
{
    public Color Color;
    public int HealthModifier;
    public int AttackModifier;
    public int AttackSpeedModifier;
    public int SpeedModifier;
    public Material ColorMaterial;
}

[CreateAssetMenu(fileName = "ColorModifiersSO", menuName = "ScriptableObjects/ColorModifiersSO", order = 4)]
public class ColorModifiersSO : ScriptableObject
{
    public ColorModifier[] ColorModifiers;
}