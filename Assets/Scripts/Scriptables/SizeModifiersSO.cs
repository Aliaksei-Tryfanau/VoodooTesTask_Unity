using UnityEngine;

public enum Size
{
    Undefined = 0,
    Big = 1,
    Small = 2
}

[System.Serializable]
public struct SizeModifier
{
    public Size Size;
    public int HealthModifier;
    public float ScaleModifier;
}

[CreateAssetMenu(fileName = "SizeModifiersSO", menuName = "ScriptableObjects/SizeModifiersSO", order = 3)]
public class SizeModifiersSO : ScriptableObject
{
    public SizeModifier[] SizeModifiers;
}
