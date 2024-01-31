using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public float range = 20f;
    public float cooldownFire = 1f;
    public float timeBeetweenHit;
    public float nextTimeToFire = 0f;

    public Image mirino;
    public Transform firePoint;
    public AudioSource shootSound;
    public Transform firstPosCam;
    public Transform secondPosCam;
    public Camera camera;

    private void Start()
    {
        timeBeetweenHit = cooldownFire;
    }

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
            timeBeetweenHit = cooldownFire;
        }

        if(Input.GetButton("Fire2"))
        {
            camera.transform.position = firstPosCam.position;
        }
        else
        {
            camera.transform.position = secondPosCam.position;
        }

        if (Time.time < nextTimeToFire && timeBeetweenHit == cooldownFire)
            mirino.color = Color.red;
        else if (timeBeetweenHit < cooldownFire)
            mirino.color = Color.green;
        else
            mirino.color = Color.black;
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

    private void OnDrawGizmos()
    {
        //Disegna una linea grigia del Raycast
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.forward * range);
    }
}