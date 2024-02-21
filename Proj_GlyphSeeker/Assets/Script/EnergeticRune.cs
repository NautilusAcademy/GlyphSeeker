using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergeticRune : MonoBehaviour
{
    [Header("Variabili")]
    [SerializeField]
    private float raycastRange = 20f;
    [SerializeField]
    private float fireRate = 1f;
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

    // Update is called once per frame
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
            else if (hit.transform.CompareTag("Chargable"))
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
        if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, raycastRange))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.CompareTag("Chargable"))
            {
                IChargable chargable = hit.transform.GetComponent<IChargable>();
                chargable.Charge();
            }
        }

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(0, fakeFirePoint.position);
            lineRenderer.SetPosition(1, hit.point);
            StartCoroutine(TrailShoot(timeBeetweenHit - 0.01f));
        }
        else
        {
            lineRenderer.SetPosition(0, fakeFirePoint.position);
            lineRenderer.SetPosition(1, fakeFirePoint.position + Camera.main.transform.forward * raycastRange);
            StartCoroutine(TrailShoot(timeBeetweenHit - 0.01f));
        }

        shootSound.Play();
    }

    IEnumerator TrailShoot(float f)
    {
        lineRenderer.gameObject.SetActive(true);

        yield return new WaitForSeconds(f);
 
        lineRenderer.gameObject.SetActive(false);
    }
}