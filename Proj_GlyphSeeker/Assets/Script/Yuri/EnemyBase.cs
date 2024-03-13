using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float followDistance = 5f;
    public float rotationSpeed = 5f;
    public Transform initialPoint;
    
    protected NavMeshAgent agent;
    protected Transform target;
    protected bool playerDetected;
    protected bool isChasing;

    public List<Transform> patrolPoints; // Lista di punti di pattuglia
    protected int currentPatrolIndex = 0;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindPlayer();
        Patrol();
    }

    protected virtual void FixedUpdate()
    {
        if (target != null)
        {
            DetectPlayer();

            if (playerDetected)
            {
                isChasing = true;
                FollowPlayer();

                if (Vector3.Distance(transform.position, target.position) <= attackRange)
                {
                    AttackPlayer();
                }
            }
            else if (isChasing)
            {
                Patrol();
            }
        }
    }

    protected virtual void FindPlayer()
    {
        if (target == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }
        }
    }

    protected virtual void DetectPlayer()
    {
        playerDetected = Vector3.Distance(transform.position, target.position) <= detectionRange;
    }

    protected virtual void FollowPlayer()
    {
        if (target != null)
        {
            Vector3 desiredDirection = target.position - transform.position;
            Vector3 desiredPosition = target.position - followDistance * desiredDirection.normalized;
            agent.SetDestination(desiredPosition);

            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    protected virtual void AttackPlayer()
    {
        Debug.Log("Attacco il giocatore!");
    }

    protected virtual void Patrol()
    {
        if (patrolPoints.Count > 0)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                // Se il nemico ha raggiunto il punto di pattuglia, scegli il prossimo punto in modo casuale
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
        else
        {
            // Se non ci sono punti di pattuglia, il nemico smette di inseguire
            isChasing = false;
        }
    }
}
