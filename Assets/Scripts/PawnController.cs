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
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float dieTime = 1f;

    private int teamId;
    private int health;
    private int attack;
    private int attackSpeed;
    private int speed;

    public void SetInfo(int argTeamId, int argHealth, int argAttack, int argAttackSpeed, int argSpeed, Mesh argMesh, Material material, float argSizeModifier)
    {
        teamId = argTeamId;
        health = argHealth;
        attack = argAttack;
        attackSpeed = argAttackSpeed;
        speed = argSpeed;
        transform.localScale = new Vector3(argSizeModifier, argSizeModifier, argSizeModifier);
        navMeshAgent.radius = argSizeModifier;
        meshFilter.mesh = argMesh;
        meshRenderer.sharedMaterial = material;
    }

    public void Activate()
    {

    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        sphereCollider.enabled = false;
        EventPawnKilled?.Invoke(this);
        transform.DOMove(transform.position + new Vector3(0f, -2f, 0f), dieTime);
    }
}
