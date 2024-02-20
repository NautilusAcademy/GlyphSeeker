using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : SimpleEnemy_AI
{
    public override IEnumerator Fire()
    {
        Rigidbody bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.velocity = firePoint.forward * bulletSpeed;
        canFire = false;

        yield return new WaitForSeconds(fireRate + Random.Range(-0.5f, 0.5f));

        canFire = true;
    }
}