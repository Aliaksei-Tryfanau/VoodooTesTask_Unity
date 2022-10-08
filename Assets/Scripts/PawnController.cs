using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour
{
    public event Action<PawnController> EventPawnKilled;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;

    private int teamId;
    private int health;
    private int attack;
    private int attackSpeed;
    private int speed;

    public void SetInfo(int argTeamId, int argHealth, int argAttack, int argAttackSpeed, int argSpeed, Mesh argMesh, Material material)
    {
        teamId = argTeamId;
        health = argHealth;
        attack = argAttack;
        attackSpeed = argAttackSpeed;
        speed = argSpeed;
        meshFilter.mesh = argMesh;
        meshRenderer.sharedMaterial = material;
    }

    public void Activate()
    {
        if (health <= 0)
        {
            EventPawnKilled?.Invoke(this);
            return;
        }
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            EventPawnKilled?.Invoke(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
