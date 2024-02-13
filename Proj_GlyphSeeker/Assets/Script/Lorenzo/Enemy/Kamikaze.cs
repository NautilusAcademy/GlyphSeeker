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
    private bool isExploded = false;
    
    private GameObject player;
    private NavMeshAgent agent;
    private Renderer renderer;
    public GameObject particle;

    private void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (!isExploded && distance <= distanceToExplode)
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            StartCoroutine(Explode());
            isExploded = true;
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
            Vector3 dir = nearbyObject.transform.position - transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit))
            {
                bool valore = !hit.transform.CompareTag("Player") && !hit.transform.CompareTag("Destroy");
                Debug.Log(valore + " " + hit.transform.name);

                if (valore)
                {
                    continue;
                }

                if (nearbyObject.CompareTag("Player"))
                {
                    PlayerStats player = nearbyObject.GetComponent<PlayerStats>();
                    player.TakeDamage(1);
                }

                if (nearbyObject.CompareTag("Destroy"))
                {
                    Destroy(nearbyObject.gameObject);
                }
            }  
        }

        StartCoroutine(AfterDestroy());
    }

    IEnumerator AfterDestroy()
    {
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject.transform.parent.gameObject);
        yield return null;
    }
}