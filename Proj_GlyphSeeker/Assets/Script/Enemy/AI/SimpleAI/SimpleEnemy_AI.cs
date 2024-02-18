using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy_AI : MonoBehaviour
{
    [Header("Distanze")]
    private float distance;
    [SerializeField]
    private float maxRangeToPlayer;
    [SerializeField]
    private float aggroRange;
    [SerializeField]
    private float attackRange;

    [Header("Variabili"), Space(5)]
    [SerializeField]
    private float rotVelocity;
    [SerializeField]
    protected float bulletSpeed;
    [SerializeField]
    protected float cooldownFire;
    protected bool canFire;
    [SerializeField]
    private Transform[] patrolPoints;
    private int currentPatrolIndex;

    [Header("Componenti"), Space(5)]
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

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position); // Calcola distanza dal giocatore

        if(distance > aggroRange) // Se il giocatore è fuori dall'aggro il nemico segue il patrol
        {
            Patrol();
        }
        else if(distance <= aggroRange) // Se il giocatore è nell'aggro il nemico gli si avvicina e lo guarda
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
                currentPatrolIndex = (currentPatrolIndex + 1);
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
    }

    private void AggroPlayer() // Il nemico raggiunge il giocatore mantenendo una distanza di sicurezza (maxRangeToPlayer)
    {
        Vector3 playerPosition = player.transform.position - maxRangeToPlayer * player.transform.position.normalized;
        agent.SetDestination(playerPosition);
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