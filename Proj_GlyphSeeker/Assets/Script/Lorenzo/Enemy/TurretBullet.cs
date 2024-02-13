using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private int life = 2;

    private void Start()
    {
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            player.TakeDamage(damage);
        }
    }
}
