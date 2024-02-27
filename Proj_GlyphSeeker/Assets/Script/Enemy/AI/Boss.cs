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
    private float bulletSpeed;
    [SerializeField]
    private float bulletUpSpeed;
    [SerializeField]
    private float rotVelocity;

    private bool canFire;

    [Header("Componenti")]
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
            StartCoroutine(Shoot());
        }
    }

    private void LookAtPlayer()
    {
        Vector3 rot = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(rot);
        Quaternion current = transform.localRotation;

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

    private IEnumerator Shoot() // Spara con un delay di "fireRate" 
    {
        Rigidbody clone;
        clone = Instantiate(kamikazeBullet, firePoint.position, firePoint.rotation);
        clone.velocity = firePoint.forward * bulletSpeed + firePoint.up * bulletUpSpeed;
        canFire = false;

        yield return new WaitForSeconds(fireRate);

        canFire = true;
    }
}
