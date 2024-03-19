using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergeticRune : PlayerShoot
{
    [Header("Variabili")]
    [SerializeField]
    private int damage;
    [SerializeField]
    private float raycastRange = 20f;
    private float nextTimeToFire = 0f;
    private float timeBeetweenHit;
    
    [Header("Componenti")]
    [SerializeField]
    private Image crosshairs;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private Transform fakeFirePoint;
    [SerializeField]
    private AudioSource shootSound;
    [SerializeField]
    private AudioSource errorSound;
    [SerializeField]
    private LineRenderer lineRenderer;

    private void Start()
    {
        timeBeetweenHit = fireRate;
    }

    void Update()
    {
        if (GameManager.inst.inputManager.Player.Fire.ReadValue<float>() > 0)
        {
            if (Time.time > nextTimeToFire)
            {
                Shoot();
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
            else if (timeBeetweenHit == fireRate)
                errorSound.Play();
        }

        if(GameManager.inst.inputManager.Player.Fire.WasReleasedThisFrame())
        {
            timeBeetweenHit = fireRate;
        }

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, raycastRange))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                crosshairs.color = Color.red;
            }
            else if (hit.transform.GetComponent<IChargeable>() != null)
            {
                crosshairs.color = Color.yellow;
            }
            else
                crosshairs.color = Color.black;
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(firePoint.position,
                                      firePoint.forward,
                                      out hit,
                                      raycastRange,
                                      ~0,
                                      QueryTriggerInteraction.Ignore);

        if (hasHit)
        {
            if (hit.transform.GetComponent<IChargeable>() != null)
            {
                IChargeable chargable = hit.transform.GetComponent<IChargeable>();
                chargable.Charge();
            }
            else if (hit.transform.GetComponent<IEnemy>() != null)
            {
                EnemyShield enemyShield = hit.transform.GetComponent<EnemyShield>();

                if (!enemyShield.isShieldActive)
                {
                    HealthSystem enemy = hit.transform.GetComponent<HealthSystem>();
                    enemy.TakeDamage(damage);
                }
            }
        }

        lineRenderer.SetPosition(0, fakeFirePoint.position);
        lineRenderer.SetPosition(1, hit.collider != null
                                     ? hit.point
                                     : fakeFirePoint.position + firePoint.forward * raycastRange);
        StartCoroutine(TrailShoot(timeBeetweenHit - 0.01f));

        shootSound.Play();
    }

    IEnumerator TrailShoot(float f)
    {
        lineRenderer.gameObject.SetActive(true);

        yield return new WaitForSeconds(f);
 
        lineRenderer.gameObject.SetActive(false);
    }
}