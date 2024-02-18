using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunEnemy : SimpleEnemy_AI
{
    public override IEnumerator Fire()
    {
        Rigidbody bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.velocity = firePoint.forward * bulletSpeed;
        canFire = false;

        yield return new WaitForSeconds(fireRate);

        canFire = true;
    }
}
