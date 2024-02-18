using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Turret : EnemyStats
{
    [Header("Distanze")]
    [SerializeField]
    private int distanceToLook;
    [SerializeField]
    private int distanceToFire;
    private float distance;

    [Header("Variabili")]
    [SerializeField]
    private float rotVelocity;
    [SerializeField]
    private float bulletSpeed;
    private bool canFire = true;
    private float charge;

    [Header("Componenti")]
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private Rigidbody bullet;
    [SerializeField]
    private AudioSource fire;
    private GameObject player;


    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);

        if(distance <= distanceToLook)
        {
            LookAtPlayer();

            if(currentAmmo <= 0)
            {
                StartCoroutine(Charge());
            }
             
        }

        else if(distance > distanceToLook && distance <= distanceToFire)
        {
            fire.Stop();
            LookAtPlayer();
        }

        if (canFire == true && currentAmmo > 0)
        {
            StartCoroutine(Shoot());
        }
    }

    void LookAtPlayer()
    {
        Vector3 rot = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(rot);
        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotVelocity);
    }

    IEnumerator Charge()
    { 
        yield return new WaitForSeconds(charge);

        currentAmmo = maxAmmo;
    }

    IEnumerator Shoot()
    {
        fire.Play();

        Rigidbody clone;
        clone = Instantiate(bullet, firePoint.position, firePoint.rotation);
        clone.velocity = firePoint.forward * bulletSpeed;
        canFire = false;
        currentAmmo--;

        yield return new WaitForSeconds(fireRate);

        canFire = true;
    }
}
