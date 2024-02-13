using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProj : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField]
    private float ExplosionRadius;

    private void Start()
    {
        rb=GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        Collider[] TargetsHit = Physics.OverlapSphere(rb.transform.position, ExplosionRadius);
        foreach (var col in TargetsHit)
        {
            //Raycast per vedere se l'esplosione ha colpito il nemico e non un muro.
            
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
           if (col.gameObject.CompareTag("Player"))
            {
                //Danneggia il giocatore.
            }
           
        }
        Destroy(this);
    }
}
