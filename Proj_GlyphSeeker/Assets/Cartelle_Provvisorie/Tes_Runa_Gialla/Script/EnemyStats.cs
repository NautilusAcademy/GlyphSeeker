using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public AudioSource audioSource;
    public int health = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        audioSource.Play();

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}