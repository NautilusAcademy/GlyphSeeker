using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
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
        damage -= decrementForSecond * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage((int)damage);
        }
    }
}
