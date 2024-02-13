using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Turret : MonoBehaviour
{
    [Header("Distanze")]
    public float distance;
    public int distanceToLook;
    public int distanceToFire;

    [Space(10)]
    public bool canFire = true;
    public float fireRate = 0.4f;
    public float charge;
    public int maxAmmo;
    public int currentAmmo;
    public float rotVelocity;

    [Space(10)]
    public Transform firePoint;
    public Rigidbody bullet;
    private GameObject player;
    public AudioSource fire;
    public ParticleSystem loaded;

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
        loaded.gameObject.SetActive(true);
    }

    IEnumerator Shoot()
    {
        fire.Play();

        Rigidbody clone;
        clone = Instantiate(bullet, firePoint.position, firePoint.rotation);
        clone.velocity = firePoint.forward * 20;
        canFire = false;
        currentAmmo--;

        yield return new WaitForSeconds(fireRate);

        canFire = true;

        if(currentAmmo <= 0)
        {
            loaded.gameObject.SetActive(false);
        }
    }
}
