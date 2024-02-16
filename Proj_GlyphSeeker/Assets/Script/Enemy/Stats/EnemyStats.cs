using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : HealthSystem, IEnemy
{
    [Header("Variabili")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private int damage;
    [SerializeField]
    private bool isShieldActive;

    [Header("Munizioni"), Space(5)]
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    private int currentAmmo;

    public override void TakeDamage(int damage)
    {
        if (!isShieldActive)
        {
            currentHealth -= damage;
            CheckDeath();
        }
    }
}