using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackPattern
{
    Undefined = 0,
    TargetClosest = 1,
    TargetWithLessHP = 2
}

public class PawnsManager : MonoBehaviour
{
    [Header("Scriptables")] [Space(10f)]
    [SerializeField] private DefaultStatsSO defaultStats;
    [SerializeField] private ShapeModifiersSO shapeModifiersSO;
    [SerializeField] private SizeModifiersSO sizeModifiersSO;
    [SerializeField] private ColorModifiersSO colorModifiersSO;
    [Header("Spawn fields")] [Space(10f)]
    [SerializeField] private Transform firstTeamSpawnPoint;
    [SerializeField] private Transform secondTeamSpawnPoint;
    [SerializeField] private int spawnCount = 20;
    [SerializeField] private PawnController pawnPrefab;
    [SerializeField] private Transform spawnRoot;

    private List<PawnController> pawnControllers = new List<PawnController>();

    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var firstTeamPawn = Instantiate(pawnPrefab, firstTeamSpawnPoint.position + new Vector3(0f, 0f, -i), Quaternion.identity, spawnRoot);
            var shapeModifier = shapeModifiersSO.ShapeModifiers[Random.Range(0, shapeModifiersSO.ShapeModifiers.Length)];
            var sizeModifier = sizeModifiersSO.SizeModifiers[Random.Range(0, sizeModifiersSO.SizeModifiers.Length)];
            var colorModifier = colorModifiersSO.ColorModifiers[Random.Range(0, colorModifiersSO.ColorModifiers.Length)];
            firstTeamPawn.SetInfo(
                1, 
                GetHealthWithModifiers(shapeModifier, sizeModifier, colorModifier), 
                GetAttackWithModifiers(shapeModifier, colorModifier),
                defaultStats.AttackSpeed + colorModifier.AttackSpeedModifier,
                defaultStats.Speed + colorModifier.SpeedModifier,
                shapeModifier.Mesh,
                colorModifier.ColorMaterial
                );
            firstTeamPawn.EventPawnKilled += OnPawnKilled;
            pawnControllers.Add(firstTeamPawn);

            var secondTeamPawn = Instantiate(pawnPrefab, secondTeamSpawnPoint.position + new Vector3(0f, 0f, -i), Quaternion.identity, spawnRoot);
            shapeModifier = shapeModifiersSO.ShapeModifiers[Random.Range(0, shapeModifiersSO.ShapeModifiers.Length)];
            sizeModifier = sizeModifiersSO.SizeModifiers[Random.Range(0, sizeModifiersSO.SizeModifiers.Length)];
            colorModifier = colorModifiersSO.ColorModifiers[Random.Range(0, colorModifiersSO.ColorModifiers.Length)];
            secondTeamPawn.SetInfo(
                2,
                GetHealthWithModifiers(shapeModifier, sizeModifier, colorModifier),
                GetAttackWithModifiers(shapeModifier, colorModifier),
                defaultStats.AttackSpeed + colorModifier.AttackSpeedModifier,
                defaultStats.Speed + colorModifier.SpeedModifier,
                shapeModifier.Mesh,
                colorModifier.ColorMaterial
                );
            secondTeamPawn.EventPawnKilled += OnPawnKilled;
            pawnControllers.Add(secondTeamPawn);
        }
    }

    private void OnDisable()
    {
        foreach (var pawnController in pawnControllers)
        {
            pawnController.EventPawnKilled -= OnPawnKilled;
        }
    }

    private void OnPawnKilled(PawnController pawnController)
    { 

    }

    private int GetHealthWithModifiers(ShapeModifier argShapeModifier, SizeModifier argSizeModifier, ColorModifier argColorModifier)
    {
        var health = defaultStats.HitPoints;
        health += argShapeModifier.HealthModifier;
        health += argSizeModifier.HealthModifier;
        health += argColorModifier.HealthModifier;

        if (health == 0)
        {
            Debug.Log("");
        }

        return health;
    }

    private int GetAttackWithModifiers(ShapeModifier argShapeModifier, ColorModifier argColorModifier)
    {
        var attack = defaultStats.Attack;
        attack += argShapeModifier.AttackModifier;
        attack += argColorModifier.AttackModifier;

        return attack;
    }
}
