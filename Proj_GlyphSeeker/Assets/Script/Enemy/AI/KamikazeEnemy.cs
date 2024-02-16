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

    [Header("Variabili")]
    [SerializeField]
    private float chargingtime;

    private GameObject player;
    private NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= distanceToExplode )
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            StartCoroutine(StartExplosion());
        }

        else if (distance <= distanceToFollow && distance >= distanceToExplode)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(chargingtime);

        Explode();
    }

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            /*if ()//nearbyObject.GetComponent<IPlayer>())
            {
                HealthSystem player = nearbyObject.GetComponent<HealthSystem>();
                player.TakeDamage(1);
            }

            if ()//nearbyObject.GetComponent<IDestroyable>())
            {
                /*HealthSystem enemy = nearbyObject.GetComponent<HealthSystem>();
                enemy.TakeDamage(1);
            }*/
        }

        Destroy(gameObject);
    }
}