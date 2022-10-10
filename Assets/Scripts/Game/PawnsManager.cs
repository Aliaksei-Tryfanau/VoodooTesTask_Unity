using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PawnsManager : MonoBehaviour
{
    public static PawnsManager Instance 
    { 
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PawnsManager>();
            }

            return _instance;
        } 
    }

    private static PawnsManager _instance;

    [Header("Scriptables")] [Space(10f)]
    [SerializeField] private DefaultStatsSO defaultStats;
    [SerializeField] private ShapeModifiersSO shapeModifiersSO;
    [SerializeField] private SizeModifiersSO sizeModifiersSO;
    [SerializeField] private ColorModifiersSO colorModifiersSO;
    [Header("Spawn fields")] [Space(10f)]
    [SerializeField] private Transform firstTeamSpawnPoint;
    [SerializeField] private Transform secondTeamSpawnPoint;
    [SerializeField] private Transform additionalPawnSpawnPoint;
    [SerializeField] private int spawnCount = 20;
    [SerializeField] private PawnController pawnPrefab;
    [SerializeField] private Transform spawnRoot;

    private KdTree<PawnController> firstTeamPawns = new KdTree<PawnController>();
    private KdTree<PawnController> secondTeamPawns = new KdTree<PawnController>();

    private void Start()
    {
        DOTween.SetTweensCapacity(200, 125);

        for (int i = 0; i < spawnCount; i++)
        {
            var firstTeamPawn = Instantiate(pawnPrefab, firstTeamSpawnPoint.position + new Vector3(0f, 0f, 1.5f * -i), Quaternion.identity, spawnRoot);
            RandomizePawnController(firstTeamPawn, 1);
            firstTeamPawn.EventPawnKilled += OnPawnKilled;
            firstTeamPawns.Add(firstTeamPawn);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            var secondTeamPawn = Instantiate(pawnPrefab, secondTeamSpawnPoint.position + new Vector3(0f, 0f, 1.5f * -i), Quaternion.identity, spawnRoot);
            RandomizePawnController(secondTeamPawn, 2);
            secondTeamPawn.EventPawnKilled += OnPawnKilled;
            secondTeamPawns.Add(secondTeamPawn);
        }
    }

    private void OnDisable()
    {
        foreach (var pawnController in firstTeamPawns)
        {
            pawnController.EventPawnKilled -= OnPawnKilled;
        }

        foreach (var pawnController in secondTeamPawns)
        {
            pawnController.EventPawnKilled -= OnPawnKilled;
        }
    }

    private void Update()
    {
        firstTeamPawns.UpdatePositions();
        secondTeamPawns.UpdatePositions();
    }

    public void StartSimulation()
    {
        foreach (var pawnController in firstTeamPawns)
        {
            pawnController.Activate();
        }

        foreach (var pawnController in secondTeamPawns)
        {
            pawnController.Activate();
        }
    }

    public void RandomizePawns()
    {
        foreach (var pawnController in firstTeamPawns)
        {
            RandomizePawnController(pawnController, 1);
        }

        foreach (var pawnController in secondTeamPawns)
        {
            RandomizePawnController(pawnController, 2);
        }
    }

    public PawnController GetNearestEnemyPawn(PawnController argPawnController)
    {
        if (argPawnController == null)
        {
            return null;
        }

        if (argPawnController.TeamId == 1)
        {
            var enemyPawn = secondTeamPawns.FindClosest(argPawnController.transform.position);
            if (enemyPawn != null)
            {
                return enemyPawn;
            }

            return null;
        }
        else
        {
            var enemyPawn = firstTeamPawns.FindClosest(argPawnController.transform.position);
            if (enemyPawn != null)
            {
                return enemyPawn;
            }

            return null;
        }
    }

    public PawnController GetEnemyWithLessHealth(PawnController argPawnController)
    {
        if (argPawnController.TeamId == 1)
        {
            var enemyPawn = secondTeamPawns.Find(p => p.Health <= argPawnController.Health);
            if (enemyPawn != null)
            {
                return enemyPawn;
            }

            return null;
        }
        else
        {
            var enemyPawn = firstTeamPawns.Find(p => p.Health <= argPawnController.Health);
            if (enemyPawn != null)
            {
                return enemyPawn;
            }

            return null;
        }
    }

    public void CreateAdditionalPawn()
    {
        var additionalPawn = Instantiate(pawnPrefab, additionalPawnSpawnPoint.position, Quaternion.identity, spawnRoot);
        var addtionalPawnTeam = Random.Range(1, 3);
        RandomizePawnController(additionalPawn, addtionalPawnTeam);
        additionalPawn.EventPawnKilled += OnPawnKilled;

        switch (addtionalPawnTeam)
        {
            case 1:
                firstTeamPawns.Add(additionalPawn);
                break;
            case 2:
                secondTeamPawns.Add(additionalPawn);
                break;
            default:
                Debug.LogError($"Unsupported team id {addtionalPawnTeam}");
                return;
        }

        additionalPawn.Activate();
    }

    private void RandomizePawnController(PawnController pawnController, int teamId)
    {
        var shapeModifier = shapeModifiersSO.ShapeModifiers[Random.Range(0, shapeModifiersSO.ShapeModifiers.Length)];
        var sizeModifier = sizeModifiersSO.SizeModifiers[Random.Range(0, sizeModifiersSO.SizeModifiers.Length)];
        var colorModifier = colorModifiersSO.ColorModifiers[Random.Range(0, colorModifiersSO.ColorModifiers.Length)];
        pawnController.SetInfo(
            teamId,
            GetHealthWithModifiers(shapeModifier, sizeModifier, colorModifier),
            GetAttackWithModifiers(shapeModifier, colorModifier),
            defaultStats.AttackSpeed + colorModifier.AttackSpeedModifier,
            defaultStats.Speed + colorModifier.SpeedModifier,
            shapeModifier.Mesh,
            colorModifier.ColorMaterial,
            sizeModifier.ScaleModifier,
            shapeModifier.AttackPattern);
    }

    private void OnPawnKilled(PawnController pawnController)
    {
        pawnController.EventPawnKilled -= OnPawnKilled;

        switch (pawnController.TeamId)
        {
            case 1:
                firstTeamPawns.RemoveAll(p => p == pawnController);
                break;
            case 2:
                secondTeamPawns.RemoveAll(p => p == pawnController);
                break;
            default:
                Debug.LogError($"Non-supportd team id {pawnController.TeamId}");
                return;
        }

        if (firstTeamPawns.Count == 0 || secondTeamPawns.Count == 0)
        {
            DOTween.KillAll();
            SceneManager.LoadScene(0);
        }
    }

    private int GetHealthWithModifiers(ShapeModifier argShapeModifier, SizeModifier argSizeModifier, ColorModifier argColorModifier)
    {
        var health = defaultStats.HitPoints;
        health += argShapeModifier.HealthModifier;
        health += argSizeModifier.HealthModifier;
        health += argColorModifier.HealthModifier;

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
