using UnityEngine;

[CreateAssetMenu(fileName = "DefaultStatsSO", menuName = "ScriptableObjects/DefaultStatsSO", order = 1)]
public class DefaultStatsSO : ScriptableObject
{
    public int HitPoints = 100;
    public int Attack = 10;
    public int Speed = 10;
    public int AttackSpeed = 1;
}

[CreateAssetMenu(fileName = "CubeModifiersSO", menuName = "ScriptableObjects/CubeModifiersSO", order = 2)]
public class CubeModifiersSO : ScriptableObject
{
    public int HealthModifier = 100;
    public int AttackModifier = 10;
    public AttackPattern AttackPattern;
}

[CreateAssetMenu(fileName = "SphereModifiersSO", menuName = "ScriptableObjects/SphereModifiersSO", order = 3)]
public class SphereModifiersSO : ScriptableObject
{
    public int HealthModifier = 50;
    public int AttackModifier = 20;
    public AttackPattern AttackPattern;
}

[CreateAssetMenu(fileName = "BigModifiersSO", menuName = "ScriptableObjects/BigModifiersSO", order = 4)]
public class BigModifiersSO : ScriptableObject
{
    public int HealthModifier = 50;
}

[CreateAssetMenu(fileName = "SmallModifiersSO", menuName = "ScriptableObjects/SmallModifiersSO", order = 5)]
public class SmallModifiersSO : ScriptableObject
{
    public int HealthModifier = -50;
}

[CreateAssetMenu(fileName = "BlueModifiersSO", menuName = "ScriptableObjects/BlueModifiersSO", order = 6)]
public class BlueModifiersSO : ScriptableObject
{
    public int AttackModifier = -15;
    public int AttackSpeedModifier = 4;
    public int SpeedModifier = 10;
}

[CreateAssetMenu(fileName = "GreenModifiersSO", menuName = "ScriptableObjects/GreenModifiersSO", order = 7)]
public class GreenModifiersSO : ScriptableObject
{
    public int AttackModifier = -100;
    public int AttackSpeedModifier = 20;
    public int SpeedModifier = -5;
}

[CreateAssetMenu(fileName = "RedModifiersSO", menuName = "ScriptableObjects/RedModifiersSO", order = 8)]
public class RedModifiersSO : ScriptableObject
{
    public int AttackModifier = 200;
    public int AttackSpeedModifier = 40;
    public int SpeedModifier = -9;
}