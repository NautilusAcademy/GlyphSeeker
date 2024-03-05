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
    private bool explodeOnImpact = false;

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
        //canPickUp = true;

        yield return new WaitForSeconds (delayExplosion);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusExplosion);

        foreach (Collider nearbyObject in colliders)
        {
            /*if (nearbyObject.GetComponent<IDamageable>() != null)
            {
                IDamageable damageable = nearbyObject.GetComponent<IChargable>();
                damageable.TakeDamage(damage);
            }*/

            if (nearbyObject.GetComponent<IChargeable>() != null)
            {
                IChargeable chargable = nearbyObject.GetComponent<IChargeable>();
                chargable.Charge();
            }
        }

        explosionParticle.gameObject.SetActive(true);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(explodeOnImpact)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radiusExplosion);

            foreach (Collider nearbyObject in colliders)
            {

                /*if (nearbyObject.GetComponent<IDamageable>() != null)
                {
                    IDamageable damageable = nearbyObject.GetComponent<IChargable>();
                    damageable.TakeDamage(damage);
                }*/

                if (nearbyObject.GetComponent<IChargeable>() != null)
                {
                    IChargeable chargable = nearbyObject.GetComponent<IChargeable>();
                    chargable.Charge();
                }
            }

            Destroy(gameObject);
        } 
    }
}
