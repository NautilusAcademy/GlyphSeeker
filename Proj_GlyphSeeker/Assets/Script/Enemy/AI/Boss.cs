using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : HealthSystem, IEnemy, IBoss
{
    private enum Phase
    {
        phase1,
        phase2,
        phase3
    }

    [SerializeField]
    private Phase currentPhase;

    [Header("Variabili")]
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float delayRandomizer;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float bulletUpSpeed;
    [SerializeField]
    private float rotVelocity;
    private int indexPhase3;
    private bool canFire;
    private bool canRestart_phase3;

    [Header("Componenti")]
    [SerializeField]
    private Rigidbody basicBullet;
    [SerializeField]
    private Rigidbody kamikazeBullet;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private EnemyShield shield;
    private GameObject player;

    private System.Action[] actions = null;
    private System.Action lastAction = null;
    private float nextActionTime;
    private float delay;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
        canFire = true;
        canRestart_phase3 = true;

        actions = new System.Action[] { BasicShoot, KamikazeShoot, RotationShoot };
    }

    private void Update()
    {
        LookAtPlayer();

        if(Time.time >= nextActionTime && canFire == true)
        {
            System.Action randomAction;

            if(currentPhase == Phase.phase1)
            {
                Invoke("BasicShoot", 0);
            }
            else if (currentPhase == Phase.phase2)
            {
                randomAction = actions[Random.Range(0, 2)];
                randomAction.Invoke();
            }
            else if (currentPhase == Phase.phase3)
            {
                do
                {
                    randomAction = actions[Random.Range(0, actions.Length)];
                }
                while (randomAction == lastAction);

                randomAction.Invoke();
                lastAction = randomAction;
            }

            nextActionTime = Time.time + delay;
        }
        
        
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        CheckPhase();
    }

    private void LookAtPlayer()
    {
        firePoint.LookAt(player.transform.position);

        Quaternion rotation = Quaternion.LookRotation(player.transform.position);
        Quaternion current = transform.rotation;
        Vector3 rot = player.transform.position - transform.position;
        rot.y = 0;
        rotation = Quaternion.LookRotation(rot);
        current = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotVelocity);
    }

    private void CheckPhase()
    {
        if(currentHealth <= maxHealth * 0.33f)
        {
            currentPhase = Phase.phase3;
        }
        else if(currentHealth <= maxHealth * 0.66f)
        {
            currentPhase = Phase.phase2;
        }
        else
            currentPhase = Phase.phase1;
    }

    void BasicShoot() 
    {
        if(canFire == true)
        {
            Rigidbody clone;
            clone = Instantiate(basicBullet, firePoint.position, firePoint.rotation);
            clone.velocity = firePoint.forward * bulletSpeed;
            canFire = false;

            StartCoroutine(CooldownShoot());
        }
    }

    void KamikazeShoot()
    {
        if (canFire == true)
        {
            Rigidbody clone;
            clone = Instantiate(kamikazeBullet, firePoint.position, firePoint.rotation);
            clone.velocity = firePoint.forward * bulletSpeed + firePoint.up * bulletUpSpeed;
            canFire = false;

            StartCoroutine(CooldownShoot());
        }
    }

    void RotationShoot()
    {
        if (canFire == true)
        {
            Rigidbody clone;
            clone = Instantiate(basicBullet, firePoint.position, firePoint.rotation);
            clone.velocity = firePoint.forward * bulletSpeed;
            canFire = false;

            StartCoroutine(CooldownShoot());
        }
    }
    
    IEnumerator CooldownShoot()
    {
        yield return new WaitForSeconds(fireRate);

        canFire = true;
    }
}
