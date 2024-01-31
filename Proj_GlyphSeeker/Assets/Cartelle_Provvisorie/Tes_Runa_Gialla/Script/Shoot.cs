using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float range = 20f;
    public float timeBeetweenHit = 1f;
    public float nextTimeToFire = 0f;

    public Transform firePoint;
    public AudioSource shootSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextTimeToFire)
        {
            ShootBullet();
            nextTimeToFire = Time.time + timeBeetweenHit;

            if(timeBeetweenHit > 0.2f)
            {
                timeBeetweenHit -= 0.2f;
            }
            else
            {
                timeBeetweenHit = 0.2f;
            }
        }

        if(Input.GetButtonUp("Fire1"))
        {
            timeBeetweenHit = 1f;
        }
    }

    private void ShootBullet()
    {
        RaycastHit hit;
        if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.CompareTag("Chargable"))
            {
                IChargable chargable = hit.transform.GetComponent<IChargable>();
                chargable.Charge();
            }
            if(hit.transform.CompareTag("Enemy"))
            {
                EnemyStats enemyStats = hit.transform.GetComponent<EnemyStats>();
                enemyStats.TakeDamage(1);
            }
        }

        shootSound.Play();
    }
}
