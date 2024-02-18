using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Turret : MonoBehaviour
{
    [Header("Distanze")]
    private float distance;
    [SerializeField]
    private int distanceToLook;
    [SerializeField]
    private int distanceToFire;

    [Space(10)]
    private bool canFire = true;
    [SerializeField]
    private float cooldownFire;
    private float charge;
    [SerializeField]
    private int maxAmmo;
    private int currentAmmo;
    [SerializeField]
    private float rotVelocity;

    [Space(10)]
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private Rigidbody bullet;
    private GameObject player;
    [SerializeField]
    private AudioSource fire;

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
        clone.velocity = firePoint.forward * 20;
        canFire = false;
        currentAmmo--;

        yield return new WaitForSeconds(cooldownFire);

        canFire = true;
    }
}
