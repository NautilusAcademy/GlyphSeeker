using System.Collections;
using UnityEngine;

public class NemicoPistola : EnemyBase
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject fire, hitPoint;
    [SerializeField] private float timeBetweenShots = 2f;
    private float nextShotTime = 0f;
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Vector3.Distance(transform.position, target.position) <= attackRange && Time.time >= nextShotTime)
        {
            StartCoroutine(Shooting());
            nextShotTime = Time.time + timeBetweenShots;
        }
    }

    IEnumerator Shooting()
    {
        RaycastHit hit;

        if(Physics.Raycast(firePoint.position , transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            Debug.DrawRay(firePoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

            GameObject a = Instantiate(fire, firePoint.position, Quaternion.identity);
            GameObject b = Instantiate(hitPoint, hit.point, Quaternion.identity);

            Destroy(a, 1);
            Destroy(b, 1);
            
            PlayerStats playerStat = hit.transform.GetComponent<PlayerStats>();
            PlayerRBMovement playerRBmov = hit.transform.GetComponent<PlayerRBMovement>();

            if (playerStat != null)
            {
                
                playerStat.TakeDamage(1);
                playerRBmov.Rinculo();
                Debug.Log("colpito!");
            }
            
        }
        //nextShotTime = Time.time + timeBetweenShots;
        yield return null;
    }
}
