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
    public int timeToExplode;

    private GameObject player;
    private NavMeshAgent agent;
    
    private void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
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
        yield return new WaitForSeconds(timeToExplode);

        // esplosione del nemico

        Destroy(gameObject);
    }
}