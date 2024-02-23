using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour // Proiettile del nemico shotgun
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private int decrementForSecond;

    private void FixedUpdate() 
    {
        // Diminuisce il danno del proiettile ad ogni frame per dare un effetto di danno maggiore quando si è vicini al nemico
        damage -= decrementForSecond * Time.deltaTime;

        if(damage <= 0.49f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPlayer>() != null)
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage((int)damage);
        }
    }
}
