using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseBullet : MonoBehaviour, IBullet // Proiettile standard dei nemici
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private int life = 2;
    [SerializeField]
    private bool isParried = false;

    private void Start() // Si distrugge dopo "life" secondi
    {
        Destroy(gameObject, life);
    }

    public void IsParried()
    {
        isParried = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
         * Se ha colpito il player ed è danneggiabile e non è perriato lo danneggia
         * Se è perriato e colpisce un nemico lo danneggia
         * Se non colpisce un nemico o lo scudo si distrugge, diversamente non lo fa
        */
        if (other.GetComponent<IDamageable>() != null) // Da sostituire con IPlayer
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage(damage);
        }
    }
}