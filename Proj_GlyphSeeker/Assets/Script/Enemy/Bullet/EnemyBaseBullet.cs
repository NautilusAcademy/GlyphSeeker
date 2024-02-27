using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class EnemyBaseBullet : MonoBehaviour // Proiettile standard dei nemici
=======
public class EnemyBaseBullet : MonoBehaviour, IBullet // Proiettile standard dei nemici
>>>>>>> origin/Enemy
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private int life = 2;
<<<<<<< HEAD
=======
    [SerializeField]
    private bool isParried = false;
>>>>>>> origin/Enemy

    private void Start() // Si distrugge dopo "life" secondi
    {
        Destroy(gameObject, life);
    }

<<<<<<< HEAD
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPlayer>() != null)
=======
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
>>>>>>> origin/Enemy
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage(damage);
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/Enemy
