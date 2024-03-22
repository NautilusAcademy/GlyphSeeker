using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryToCharge : SwitchClass, IChargeable
{
    [SerializeField]
    private int charge;
    [SerializeField]
    private int maxCharge;
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

    public override void ToggleSwitch()
    {
        charge++;

        if (charge >= maxCharge)
        {
            isActive = true;
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
}
