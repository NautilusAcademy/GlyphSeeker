using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryToCharge : MonoBehaviour, IChargeable
{
    [SerializeField]
    private int charge = 0;
    [SerializeField]
    private int maxCharge = 3;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float delayExplosion;
    [SerializeField]
    private float radiusExplosion;
    [SerializeField]
    private ParticleSystem explosionParticle;

    [SerializeField]
    private PickUp pickUp;

    public void Charge()
    {
        charge++;

        if (charge >= maxCharge)
        {
            StartCoroutine(FullCharged());
        }
    }

    IEnumerator FullCharged()
    {
        pickUp.canPickUp = true;

        yield return new WaitForSeconds (delayExplosion);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusExplosion);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.GetComponent<IDamageable>() != null)
            {
                HealthSystem healthSystem = nearbyObject.GetComponent<HealthSystem>();
                healthSystem.TakeDamage(damage);
            }

            if (nearbyObject.GetComponent<IChargeable>() != null)
            {
                SwitchClass switchClass = nearbyObject.transform.GetComponent<SwitchClass>();
                switchClass.ToggleSwitch();
            }
        }

        explosionParticle.gameObject.SetActive(true);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusExplosion);

        foreach (Collider nearbyObject in colliders)
        {

            if (nearbyObject.GetComponent<IDamageable>() != null)
            {
                HealthSystem healthSystem = nearbyObject.GetComponent<HealthSystem>();
                healthSystem.TakeDamage(damage);
            }

            if (nearbyObject.GetComponent<IChargeable>() != null)
            {
                SwitchClass switchClass = nearbyObject.transform.GetComponent<SwitchClass>();
                switchClass.ToggleSwitch();
            }
        }

        Destroy(gameObject);
    }
}
