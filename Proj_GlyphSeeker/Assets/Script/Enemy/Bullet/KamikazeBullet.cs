using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeBullet : MonoBehaviour
{
    [SerializeField]
    private float explosionRadius;

    public void Explode() // Crea una sfera attorno a se e danneggia tutti gli oggetti con l'interfaccia IDamageable e distrugge quelli con IDestroyable
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.GetComponent<IDamageable>() != null /*|| nearbyObject.GetComponent<IDestroyable>() != null*/)
            {
                //IDestroyable item = nearbyObject.GetComponent<IDestroyable>();

                if (nearbyObject.GetComponent<IDamageable>() != null)
                {
                    HealthSystem target = nearbyObject.GetComponent<HealthSystem>();
                    target.TakeDamage(1);
                    break;
                }
                //else if (item != null)
                //{
                //    Destroy(nearbyObject);
                //    return;
                //}
            }
            else
                break;

        }

        Destroy(gameObject);
    }
}
