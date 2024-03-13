using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float followDistance = 5f;
    public float rotationSpeed = 5f;
    public Transform initialPoint; // Punto iniziale del nemico
    private NavMeshAgent agent;
    private Transform target;
    private bool playerDetected;
    private bool isChasing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindPlayer();
    }

    void Update()
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
                ReturnToInitialPoint();
            }
        }
    }

    void FindPlayer()
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

    void DetectPlayer()
    {
        playerDetected = Vector3.Distance(transform.position, target.position) <= detectionRange;
    }

    void FollowPlayer()
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

    void AttackPlayer()
    {
        // Logica di attacco
        Debug.Log("Attacco il giocatore!");
    }

    void ReturnToInitialPoint()
    {
        // Se il nemico non è già nell'iniziale, torna al punto iniziale
        if (Vector3.Distance(transform.position, initialPoint.position) > 1f)
        {
            agent.SetDestination(initialPoint.position);
        }
        else
        {
            // Se il nemico è arrivato al punto iniziale, smetti di inseguire
            isChasing = false;
        }
    }
}
