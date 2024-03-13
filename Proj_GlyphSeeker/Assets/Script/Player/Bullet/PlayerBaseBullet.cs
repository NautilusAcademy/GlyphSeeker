using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseBullet : MonoBehaviour, IBullet // Proiettile standard dei nemici
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private int life;

    private void Start() // Si distrugge dopo "life" secondi
    {
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IEnemy>() != null)
        {
            HealthSystem enemy = other.GetComponent<HealthSystem>();
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}