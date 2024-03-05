using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable // Sistema di salute che eredita l'interfaccia IDamageable
{
    [Header("Salute")]
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected int currentHealth;

    private float invincibleSec;
    private bool isInvincible = false;
    private bool isDead = false;

    public virtual void Start() 
    {
        RefillFullHealth();
    }

    public void RefillFullHealth() // Setta la salute attuale uguale alla salute massima
    {
        currentHealth = maxHealth;
    }

    public void RefillHealth(int health) // Funzione da chiamare quando il giocatore recupera salute
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
