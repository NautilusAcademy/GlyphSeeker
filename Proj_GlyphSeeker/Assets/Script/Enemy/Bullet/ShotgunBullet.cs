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
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage((int)damage); // Faccio un cast di damage ad int poichè per decrescerlo l'ho dovuto rendere float
        }
    }
}
