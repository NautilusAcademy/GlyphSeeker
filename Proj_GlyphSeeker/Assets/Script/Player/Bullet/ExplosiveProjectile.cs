using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private int life;
    [SerializeField]
    private float ExplosionRadius;
    [SerializeField]
    private Rigidbody rb;

    private void Start()
    {
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<IPlayer>() == null)
        {
            Collider[] colliders = Physics.OverlapSphere(rb.transform.position, ExplosionRadius);

            foreach (Collider nearbyObject in colliders)
            {
                Vector3 dir = nearbyObject.transform.position - transform.position;
                RaycastHit hit;

                if (Physics.Raycast(transform.position, dir, out hit))
                {
                    bool isDamageable = hit.transform.GetComponent<IDamageable>() != null;
                    bool isDestroyable = hit.transform.GetComponent<IDestroyable>() != null;

                    if (isDamageable || isDestroyable)
                    {
                        if (isDamageable)
                        {
                            if(hit.transform.GetComponent<IEnemy>() != null)
                            {
                                EnemyShield enemyShield = hit.transform.GetComponent<EnemyShield>();

                                if(enemyShield.isShieldActive)
                                {
                                    enemyShield.CrackShield(damage);
                                }
                                else
                                {
                                    HealthSystem target = hit.transform.GetComponent<HealthSystem>();
                                    target.TakeDamage(damage);
                                } 
                            }

                            else if(hit.transform.GetComponent<IPlayer>() != null)
                            {
                                HealthSystem target = hit.transform.GetComponent<HealthSystem>();
                                target.TakeDamage(damage);
                            }
                        }
                        else
                        {
                            Destroy(hit.transform.gameObject);
                        }
                    }
                }
                else
                    continue;
            }

            Destroy(this.gameObject);
        }
    }
}