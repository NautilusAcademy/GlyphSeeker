using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBullet : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float radiusExplosion;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<IPlayer>() == null)
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
                    IChargeable chargable = nearbyObject.GetComponent<IChargeable>();
                    chargable.Charge();
                }
            }

            Destroy(gameObject);
        }
    }
}
