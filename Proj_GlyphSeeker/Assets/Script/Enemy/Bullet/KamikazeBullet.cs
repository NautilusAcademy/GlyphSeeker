using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeBullet : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float explosionRadius;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    public void Explode() // Crea una sfera attorno a se e danneggia tutti gli oggetti con l'interfaccia IDamageable e distrugge quelli con IDestroyable
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Vector3 dir = nearbyObject.transform.position - transform.position;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit))
            {
                bool isDamageable = hit.transform.GetComponent<IDamageable>() != null;
                bool isDestroyable = hit.transform.GetComponent<IDestroyable>() != null;

                if (isDamageable || isDestroyable)
                {
                    if (isDamageable)
                    {
                        HealthSystem target = hit.transform.GetComponent<HealthSystem>();
                        target.TakeDamage(damage);
                    }
                    else
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
            else
                continue;
        }

        Destroy(gameObject);
    }
}
