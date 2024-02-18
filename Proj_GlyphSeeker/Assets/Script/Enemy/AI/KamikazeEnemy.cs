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

    private GameObject player;
    private NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
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
        yield return new WaitForSeconds(fireRate);

        Explode();
    }

    public void Explode()
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
                    return;
                }
                //else if (item != null)
                //{
                //    Destroy(nearbyObject);
                //    return;
                //}
            }
            else
                return;
            
        }

        Destroy(gameObject);
    }
}