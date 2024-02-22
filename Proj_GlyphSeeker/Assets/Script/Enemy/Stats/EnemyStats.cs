using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : HealthSystem, IEnemy // Statistiche comuni a tutti i nemici
{
    [Header("Variabili")]
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float fireRate;
    [SerializeField]
    private bool isShieldActive; // Bool da utilizzare nel caso in cui il nemico abbia la corazza

    // Override per controllare se il nemico ha la corazza, in quel caso non può subire danni prima che venga distrutta
    public override void TakeDamage(int damage)
    {
        if (!isShieldActive)
        {
            currentHealth -= damage;
            CheckDeath();
        }
    }
}