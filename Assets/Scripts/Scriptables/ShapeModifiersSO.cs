using UnityEngine;

public enum Shape
{
    Undefined = 0,
    Cube = 1,
    Sphere = 2
}

[System.Serializable]
public struct ShapeModifier
{
    public Shape Shape;
    public int HealthModifier;
    public int AttackModifier;
    public AttackPattern AttackPattern;
    public Mesh Mesh;
}

[CreateAssetMenu(fileName = "ShapeModifiersSO", menuName = "ScriptableObjects/ShapeModifiersSO", order = 2)]
public class ShapeModifiersSO : ScriptableObject
{
    public ShapeModifier[] ShapeModifiers;
}