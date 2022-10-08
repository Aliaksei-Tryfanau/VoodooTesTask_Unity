using UnityEngine;

[CreateAssetMenu(fileName = "DefaultStatsSO", menuName = "ScriptableObjects/DefaultStatsSO", order = 1)]
public class DefaultStatsSO : ScriptableObject
{
    public int HitPoints = 100;
    public int Attack = 10;
    public int Speed = 10;
    public int AttackSpeed = 1;
}