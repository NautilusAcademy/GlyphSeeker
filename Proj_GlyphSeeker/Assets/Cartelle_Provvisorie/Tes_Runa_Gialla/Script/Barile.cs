using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barile : MonoBehaviour, IChargable
{
    [SerializeField]
    private int charge = 0;
    [SerializeField]
    private int maxCharge = 3;
    public ParticleSystem particleSystem;

    public bool canPickUp = false;
    public float delayExplosion;

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
        canPickUp = true;

        yield return new WaitForSeconds (delayExplosion);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 10);

        foreach (Collider nearbyObject in colliders)
        {
            Debug.Log(nearbyObject.name);
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

        particleSystem.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
