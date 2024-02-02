using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barile : MonoBehaviour, IChargable
{
    [SerializeField]
    private int charge = 0;
    private int maxCharge = 3;
    public ParticleSystem particleSystem;

    private void Update()
    {
        if (charge >= maxCharge)
        {
            FullCharged();
        }
    }

    public void Charge()
    {
        charge++;
    }

    public void FullCharged()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                Destroy(nearbyObject.gameObject);
            }
        }

        particleSystem.gameObject.SetActive(true);
    }
}
