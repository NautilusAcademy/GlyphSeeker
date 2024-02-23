using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmBarile : MonoBehaviour
{
    public int radiusExplosion;
    private void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {        
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusExplosion);

        foreach (Collider nearbyObject in colliders)
        {

            if (nearbyObject.CompareTag("Enemy"))
            {
                Destroy(nearbyObject.gameObject);
            }

            if (nearbyObject.CompareTag("Chargable"))
            {
                IChargable chargable = nearbyObject.GetComponent<IChargable>();
                chargable.Charge();
            }
        }

        Destroy(gameObject);
    }
}
