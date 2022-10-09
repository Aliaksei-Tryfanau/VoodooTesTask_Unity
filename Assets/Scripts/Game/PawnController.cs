using DG.Tweening;
using System;
using UnityEngine;

public class PawnController : MonoBehaviour
{
    public event Action<PawnController> EventPawnKilled;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float targetRefreshRate = 1.5f;

    public int TeamId { get; private set; }
    public int Health { get; private set; }

    private int attack;
    private int attackSpeed;
    private float speed;
    private AttackPattern attackPattern;
    private PawnController target;
    private bool canAttack;

    private void Update()
    {
        if (target != null && canAttack)
        {
            transform.LookAt(target.transform);
            transform.Translate(speed * Time.deltaTime * (target.transform.position - transform.position).normalized, Space.World);
        }
    }

    public void SetInfo(int argTeamId, int argHealth, int argAttack, int argAttackSpeed, int argSpeed, 
        Mesh argMesh, Material material, float argSizeModifier, AttackPattern argAttackPatteren)
    {
        TeamId = argTeamId;
        gameObject.layer = LayerMask.NameToLayer(TeamId == 1 ? "FirstTeam" : "SecondTeam");
        Health = argHealth;
        attack = argAttack;
        attackSpeed = argAttackSpeed;
        speed = argSpeed;
        transform.localScale = new Vector3(argSizeModifier, argSizeModifier, argSizeModifier);
        meshFilter.mesh = argMesh;
        meshRenderer.sharedMaterial = material;
        attackPattern = argAttackPatteren;
    }

    public void Activate()
    {
        canAttack = true;
        FindTarget();
        UpdateTarget();
    }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;

        if (Health <= 0)
        {
            Die();
        }
    }

    private void FindTarget()
    {
        switch (attackPattern)
        {
            case AttackPattern.TargetClosest:
                target = PawnsManager.Instance.GetNearestEnemyPawn(this);
                break;
            case AttackPattern.TargetWithLessHP:
                target = PawnsManager.Instance.GetEnemyWithLessHealth(this);
                break;
            default:
                Debug.LogError($"Unsupported attack pattern {attackPattern}");
                return;
        }
    }

    private void UpdateTarget()
    {
        DOVirtual.DelayedCall(targetRefreshRate, () =>
        {
            if (target == null)
            {
                FindTarget();
            }

            UpdateTarget();
        });
    }

    private void Die()
    {
        EventPawnKilled?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!canAttack)
        {
            return;
        }

        var enemyPawn = other.GetComponent<PawnController>();
        if (enemyPawn != null && enemyPawn == target)
        {
            enemyPawn.Damage(attack);
            canAttack = false;
            DOVirtual.DelayedCall(attackSpeed, () =>
            {
                canAttack = true;
            });
        }
    }
}
