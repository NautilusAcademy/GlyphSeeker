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

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
        canFire = true;
        canRestart_phase3 = true;
    }

    private void Update()
    {
        LookAtPlayer();

        if (currentHealth <= maxHealth * 0.33f)
        {
            currentPhase = Phase.phase3;
        }
        else if (currentHealth <= maxHealth * 0.66f)
        {
            currentPhase = Phase.phase2;
        }
        else
            currentPhase = Phase.phase1;

        if(currentPhase == Phase.phase1 && canFire == true)
        {
            StartCoroutine(BasicShoot());
        }
        else if (currentPhase == Phase.phase2 && canFire == true)
        {
            StartCoroutine(KamikazeShoot());
        }
        else if (currentPhase == Phase.phase3 && canFire == true)
        {
            if(indexPhase3 == 0)
            {
                StartCoroutine(BasicShoot());
            }
            else
            {
                StartCoroutine(KamikazeShoot());
            }

            if(canRestart_phase3)
            {
                StartCoroutine(RandomizeShoot());
                canRestart_phase3 = false;
            }
            
        }
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

    public override void TakeDamage(int damage)
    {
        if(!shield.isShieldActive)
        {
            currentHealth -= damage;
        }
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

    private IEnumerator BasicShoot() // Spara con un delay di "fireRate" 
    {
        Rigidbody clone;
        clone = Instantiate(basicBullet, firePoint.position, firePoint.rotation);
        clone.velocity = firePoint.forward * bulletSpeed;
        canFire = false;

        yield return new WaitForSeconds(fireRate);

        canFire = true;
    }

    private IEnumerator KamikazeShoot() // Spara con un delay di "fireRate" 
    {
        Rigidbody clone;
        clone = Instantiate(kamikazeBullet, firePoint.position, firePoint.rotation);
        clone.velocity = firePoint.forward * bulletSpeed + firePoint.up * bulletUpSpeed;
        canFire = false;

        yield return new WaitForSeconds(fireRate);

        canFire = true;
    }

    private IEnumerator RandomizeShoot()
    {
        yield return new WaitForSeconds(delayRandomizer);

        canRestart_phase3 = true;

        if (indexPhase3 == 0)
        {
            indexPhase3++;
        }
        else
            indexPhase3 = 0;
    }
}
