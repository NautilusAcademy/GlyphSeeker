using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth;
    protected int currentHealth;

    [SerializeField]
    private float invincibleSec;
    private bool isInvincible = false;
    private bool isDead = false;

    private void Start()
    {
        RefillFullHealth();
    }

    public void RefillFullHealth()
    {
        currentHealth = maxHealth;
    }

    public void RefillHealth(int health)
    {
        currentHealth += health;
        Mathf.Clamp(currentHealth, currentHealth, maxHealth);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        CheckDeath();
    }

    public void CheckDeath()
    {
        if(currentHealth <= 0)
        {
            isDead = true;
            InstantDeath();
        }
    }

    public void InstantDeath()
    {
        Destroy(gameObject);
    }
}
