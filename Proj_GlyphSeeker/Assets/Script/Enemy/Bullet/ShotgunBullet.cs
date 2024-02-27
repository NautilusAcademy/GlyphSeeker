using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour // Proiettile del nemico shotgun
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private int decrementForSecond;
<<<<<<< HEAD
=======
    [SerializeField]
    private int life = 2;

    private void Start()
    {
        Destroy(gameObject, life);
    }
>>>>>>> origin/Enemy

    private void FixedUpdate() 
    {
        // Diminuisce il danno del proiettile ad ogni frame per dare un effetto di danno maggiore quando si è vicini al nemico
        damage -= decrementForSecond * Time.deltaTime;
<<<<<<< HEAD

        if(damage <= 0.49f)
        {
            Destroy(gameObject);
        }
=======
>>>>>>> origin/Enemy
    }

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        if (other.GetComponent<IPlayer>() != null)
=======
        if (other.GetComponent<IDamageable>() != null) // Da sostituire con IPlayer
>>>>>>> origin/Enemy
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage((int)damage);
        }
    }
}
