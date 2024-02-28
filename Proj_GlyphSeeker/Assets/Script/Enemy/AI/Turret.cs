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
    [SerializeField]
    private int maxAmmo;
    private int currentAmmo;
    private bool canFire = true;
    [SerializeField]
    private float charge;

    [Header("Componenti")]
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private Rigidbody bullet;
    [SerializeField]
    private AudioSource fireSound;
    private GameObject player;


    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update() // Calcola la distanza dal giocatore ed in base alle distanze esegue diverse funzioni
    {
        if(currentAmmo <= 0)
        {
            fireSound.Stop();
        }

        distance = Vector3.Distance(player.transform.position, transform.position);

        if(distance <= distanceToLook) // Se avvista il giocatore lo guarda e carica i proiettili
        {
            LookAtPlayer();

            if(currentAmmo <= 0)
            {
                StartCoroutine(Charge());
            }
             
        }

        else if(distance > distanceToLook && distance <= distanceToFire)  
        {
            LookAtPlayer();
        }

        if (canFire == true && currentAmmo > 0) // Se la torretta è carica e può sparare
        {
            StartCoroutine(Shoot());
        }
    }

    private void LookAtPlayer()
    {
        firePoint.LookAt(player.transform.position);

        Vector3 rot = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(rot);
        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotVelocity);
    }

    private IEnumerator Charge() // Carica per "charge" tempo e assegna a currentAmmo maxAmmo
    { 
        yield return new WaitForSeconds(charge);

        currentAmmo = maxAmmo;
    }

    private IEnumerator Shoot() // Spara con un delay di "fireRate" fino a quando currentAmmo non diventa 0
    {
        fireSound.Play();

        Rigidbody clone;
        clone = Instantiate(bullet, firePoint.position, firePoint.rotation);
        clone.velocity = firePoint.forward * bulletSpeed;
        canFire = false;
        currentAmmo--;

        yield return new WaitForSeconds(fireRate);

        canFire = true;
    }
}
