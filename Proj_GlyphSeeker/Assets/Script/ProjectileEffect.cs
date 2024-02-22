using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField]
    private float ExplosionRadius;
    private bool targetHit;

    private void Start()
    {
        rb.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] TargetsHit = Physics.OverlapSphere(rb.transform.position, ExplosionRadius);
        foreach (var col in TargetsHit)
        {
            if (col.gameObject.CompareTag("DestructableObject"))
            {
                Destroy(col.gameObject);
            }
           if (col.gameObject.CompareTag("Enemy"))
            {
                //Bisogna inserire la variabile armatura direttamente dallo script dei nemici
                //if (nemico ha armatura--> distruggi)
                //else (infligge danni)

            }
           
        }
        Destroy(this);
    }
}
