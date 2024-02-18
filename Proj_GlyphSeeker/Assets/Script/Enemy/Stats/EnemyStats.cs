using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : HealthSystem, IEnemy
{
    [Header("Variabili")]
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float fireRate;
    [SerializeField]
    private bool isShieldActive;

    [Header("Munizioni")]
    [SerializeField]
    protected int maxAmmo;
    [SerializeField]
    protected int currentAmmo;

    public override void TakeDamage(int damage)
    {
        if (!isShieldActive)
        {
            currentHealth -= damage;
            CheckDeath();
        }
    }
}