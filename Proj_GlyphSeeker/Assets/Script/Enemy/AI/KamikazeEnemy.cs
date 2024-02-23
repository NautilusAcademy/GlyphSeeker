using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikazeEnemy : EnemyStats
{
    [Header("Distanze")]
    [SerializeField]
    private int distanceToFollow;
    [SerializeField]
    private int distanceToExplode;
    [SerializeField]
    private float explosionRadius;

    [SerializeField]
    private int damage;
    private GameObject player;
    private NavMeshAgent agent;

    public override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    private void Update() // Calcola la distanza dal giocatore ed in base alle distanze esegue diverse funzioni
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= distanceToExplode) // Se è abbastanza vicino si ferma e inizia ad esplodere
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            StartCoroutine(StartExplosion());
        }

        else if (distance <= distanceToFollow && distance >= distanceToExplode) // Se avvista il giocatore gli si avvicina
        {
            agent.SetDestination(player.transform.position);
        }
    }

    IEnumerator StartExplosion() // Aspetta un cooldown "fireRate" e chiama la funzione "Explode"
    {
        yield return new WaitForSeconds(fireRate);

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
                bool isDamageable = hit.transform.GetComponent<IDamageable>() != null ;
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