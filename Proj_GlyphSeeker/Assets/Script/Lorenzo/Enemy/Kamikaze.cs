using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kamikaze : MonoBehaviour
{
    [Header("Distanze")]
    public float distance;
    public int distanceToGo;
    public int distanceToExplode;
    public float radiusForExplosion;

    [Header("Variabili"), Space(10)]
    public float timeToExplode;
    public int damage;
    public Color startColor, endColor;

    private GameObject player;
    private NavMeshAgent agent;
    private Renderer renderer;

    private void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= distanceToExplode )
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            StartCoroutine(Explode());
        }

        else if (distance <= distanceToGo && distance >= distanceToExplode)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    IEnumerator Explode()
    {
        float tick = 0f;

        while (renderer.material.color != endColor)
        {
            tick += Time.deltaTime / 2;
            renderer.material.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusForExplosion);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Player"))
            {
                PlayerStats player = nearbyObject.GetComponent<PlayerStats>();
                player.TakeDamage(1);
            }
            if(nearbyObject.CompareTag("Destroy"))
            {
                Destroy(nearbyObject.gameObject);
            }
        }

        Destroy(gameObject);
    }
}