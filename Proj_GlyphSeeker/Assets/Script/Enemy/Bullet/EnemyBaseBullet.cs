using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseBullet : MonoBehaviour // Proiettile standard dei nemici
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private int life = 2;

    private void Start() // Si distrugge dopo "life" secondi
    {
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null) // Da sostituire con IPlayer
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage(damage);
        }
    }
}