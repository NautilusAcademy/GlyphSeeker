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
    public bool canShoot;
    public float delayShoot = 0.4f;

    [Space(10)]
    public Transform firePoint;
    public Rigidbody bullet;
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

            if(canShoot == true)
            {
                StartCoroutine(Shoot());
            }
        }
        else if(distance > distanceToLook && distance <= distanceToFire)
        {
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        Vector3 rot = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(rot);
        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * 5);
    }

    IEnumerator Shoot()
    {
        Rigidbody clone;
        clone = Instantiate(bullet, firePoint.position, firePoint.rotation);
        clone.velocity = firePoint.forward * 20;
        canShoot = false;

        yield return new WaitForSeconds(delayShoot);

        canShoot = true;
    }
}
