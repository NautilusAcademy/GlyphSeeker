using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseBullet : MonoBehaviour, IBullet // Proiettile standard dei nemici
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private int life;
    [SerializeField]
    private bool isParried = false;
    [SerializeField]
    private float bulletSpeed;

    private void Start() // Si distrugge dopo "life" secondi
    {
        Destroy(gameObject, life);
    }

    private void OnEnable()
    {
        ChangeVelocity(transform.forward, 0);
    }

    public void IsParried()
    {
        isParried = true;
    }

    private void OnTriggerEnter(Collider other)
    {   
      /*Se ha colpito il player ed è danneggiabile e non è perriato lo danneggia
        Se è perriato e colpisce un nemico lo danneggia
        Se non colpisce un nemico o lo scudo si distrugge, diversamente non lo fa*/
        
        if (isParried == false)
        {
            if (other.GetComponent<IPlayer>() != null)
            {
                HealthSystem player = other.GetComponent<HealthSystem>();
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.GetComponent<IEnemy>() != null)
            {
                HealthSystem enemy = other.GetComponent<HealthSystem>();
                enemy.TakeDamage(damage);
            }
        } 
    }
    public void ChangeVelocity(Vector3 direction, float velToAdd)
    {
        transform.forward = direction;
        transform.GetComponent<Rigidbody>().velocity = transform.forward * (bulletSpeed + velToAdd);
    }
}