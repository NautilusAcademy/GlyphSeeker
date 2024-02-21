using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour // Proiettile del nemico shotgun
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private int decrementForSecond;
    [SerializeField]
    private int life = 2;

    private void Start()
    {
        Destroy(gameObject, life);
    }

    private void FixedUpdate() 
    {
        // Diminuisce il danno del proiettile ad ogni frame per dare un effetto di danno maggiore quando si è vicini al nemico
        damage -= decrementForSecond * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null) // Da sostituire con IPlayer
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage((int)damage);
        }
    }
}
