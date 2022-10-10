using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    public event Action<PawnController> EventPawnKilled;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private float targetRefreshRate = 1.5f;
    [SerializeField] private NavMeshAgent navMeshAgent;

    public int TeamId { get; private set; }
    public int Health { get; private set; }

    private int attack;
    private int attackSpeed;
    private AttackPattern attackPattern;
    private PawnController target;
    private bool canAttack;
    private Tween updateTargetTween;
    private Tween attackDelayTween;

    public void SetInfo(int argTeamId, int argHealth, int argAttack, int argAttackSpeed, int argSpeed, 
        Mesh argMesh, Material material, float argSizeModifier, AttackPattern argAttackPatteren)
    {
        TeamId = argTeamId;
        gameObject.layer = LayerMask.NameToLayer(TeamId == 1 ? "FirstTeam" : "SecondTeam");
        Health = argHealth;
        attack = argAttack;
        attackSpeed = argAttackSpeed;
        transform.localScale = new Vector3(argSizeModifier, argSizeModifier, argSizeModifier);
        navMeshAgent.radius = argSizeModifier / 2f;
        navMeshAgent.speed = argSpeed;
        meshFilter.mesh = argMesh;
        meshRenderer.sharedMaterial = material;
        attackPattern = argAttackPatteren;
    }

    public void Activate()
    {
        canAttack = true;
        FindTarget();
        UpdateTarget();

        if (target != null)
        {
            navMeshAgent.SetDestination(target.transform.position);
        }
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
        updateTargetTween = DOVirtual.DelayedCall(targetRefreshRate, () =>
        {
            if (canAttack)
            {
                if (target == null)
                {
                    FindTarget();
                }

                if (target != null)
                {
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(target.transform.position);
                }
            }

            UpdateTarget();
        });
    }

    private void Die()
    {
        if (updateTargetTween != null)
        {
            updateTargetTween.Kill();
        }

        if (attackDelayTween != null)
        {
            attackDelayTween.Kill();
        }

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

            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.isStopped = true;
            }

            attackDelayTween = DOVirtual.DelayedCall(attackSpeed, () =>
            {
                canAttack = true;
            });
        }
    }
}
