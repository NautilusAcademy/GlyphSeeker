using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : HealthSystem, IEnemy, IBoss
{
    
    private enum Phase
    {
        phase1,
        phase2,
        phase3
    }

    [SerializeField]
    private Phase currentPhase;

    [Header("Componenti")]
    [SerializeField]
    private GameObject kamikazeBullet;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private EnemyShield shield;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (currentHealth <= maxHealth * 0.33f)
        {
            currentPhase = Phase.phase3;
        }
        else if (currentHealth <= maxHealth * 0.66f)
        {
            currentPhase = Phase.phase2;
        }
        else
            currentPhase = Phase.phase1;
    }

    public override void TakeDamage(int damage)
    {
        if(!shield.isShieldActive)
        {
            currentHealth -= damage;
        }
    }

    private void CheckPhase()
    {
        if(currentHealth <= maxHealth * 0.33f)
        {
            currentPhase = Phase.phase3;
        }
        else if(currentHealth <= maxHealth * 0.66f)
        {
            currentPhase = Phase.phase2;
        }
        else
            currentPhase = Phase.phase1;
    }
}
