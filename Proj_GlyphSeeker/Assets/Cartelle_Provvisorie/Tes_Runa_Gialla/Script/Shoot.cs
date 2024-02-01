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
    public Transform fakeFirePoint;
    public AudioSource shootSound;
    public AudioSource errorSound;
    public LineRenderer lineRenderer;

    private void Start()
    {
        timeBeetweenHit = cooldownFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.inst.inputManager.Player.Fire.ReadValue<float>() > 0)
        {
            if (Time.time > nextTimeToFire)
            {
                ShootBullet();
                nextTimeToFire = Time.time + timeBeetweenHit;

                if (timeBeetweenHit > 0.2f)
                {
                    timeBeetweenHit -= 0.2f;
                }
                else
                {
                    timeBeetweenHit = 0.2f;
                }
            }
            else if (timeBeetweenHit == cooldownFire)
                errorSound.Play();
        }

        if(GameManager.inst.inputManager.Player.Fire.WasReleasedThisFrame())
        {
            timeBeetweenHit = cooldownFire;
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
            if (hit.transform.CompareTag("Enemy"))
            {
                EnemyStats enemyStats = hit.transform.GetComponent<EnemyStats>();
                enemyStats.TakeDamage(1);
            }

            if (hit.transform)
            {
                lineRenderer.SetPosition(0, fakeFirePoint.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
                lineRenderer.SetPosition(0, fakeFirePoint.position);
                lineRenderer.SetPosition(1, firePoint.forward);
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