using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy_AI : EnemyStats
{
    [Header("Distanze")]
    [SerializeField]
    private float maxRangeToPlayer;
    [SerializeField]
    private float aggroRange;
    [SerializeField]
    private float attackRange;
    private float distance;

    [Header("Variabili")]
    [SerializeField]
    private float rotVelocity;
    [SerializeField]
    protected float bulletSpeed;
    protected bool canFire = true;
    [SerializeField]
    private Transform[] patrolPoints;
    private int currentPatrolIndex;
    private bool playerSeen;

    [Header("Componenti")]
    [SerializeField]
    protected Transform firePoint;
    [SerializeField]
    protected Rigidbody bulletPrefab;
    private GameObject player;
    private NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        currentPatrolIndex = 0;
    }

    protected virtual void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position); // Calcola distanza dal giocatore

        if(distance > aggroRange) // Se il giocatore è fuori dall'aggro il nemico segue il patrol
        {
            if(playerSeen)
            {
                agent.ResetPath();
                playerSeen = false;
            }
            Patrol();
        }
        else // Se il giocatore è nell'aggro il nemico gli si avvicina e lo guarda
        {
            AggroPlayer();
            LookAtPlayer();
        }

        if(distance <= attackRange) // Se il giocatore è dentro il range d'attacco il nemico lo attacca
        {
            Attack();
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length > 0)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.2f)
            {
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);

                if (currentPatrolIndex == patrolPoints.Length - 1)
                    currentPatrolIndex = 0;
                else
                    currentPatrolIndex = (currentPatrolIndex + 1);
            }   
        }
    }

    private void AggroPlayer() // Il nemico raggiunge il giocatore mantenendo una distanza di sicurezza (maxRangeToPlayer)
    {
        Vector3 playerPosition = player.transform.position - maxRangeToPlayer * player.transform.position.normalized;
        agent.SetDestination(playerPosition);
        playerSeen = true;
    }

    private void LookAtPlayer() // Il nemico guarda il giocatore
    {
        Vector3 rot = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(rot);
        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotVelocity);
    }

    private void Attack() // Il nemico attacca quando il bool canFire è true e richiama la Coroutine Fire che subisce un override nelle sotto classi enemy
    {
        if(canFire == true)
        {
            StartCoroutine(Fire());
        }
    }

    public virtual IEnumerator Fire() // Effettua l'attacco e aspetta il cooldown per riattivare il bool canFire
    {
        yield return null;
    }
}