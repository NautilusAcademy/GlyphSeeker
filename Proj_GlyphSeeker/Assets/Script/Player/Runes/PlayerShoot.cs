using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("——  PlayerShoot  ——")]
    [SerializeField] protected int maxAmmo = 10;
    [SerializeField] protected float fireRate = 2;
    protected int currentAmmo = 0;
    
    protected bool canShoot,
                   isCooldown;



    void Awake()
    {
        FullyReloadAmmo();
    }


    protected virtual void ShootBullet(GameObject bulletToShoot) { }

    protected virtual void ActivateShield(GameObject shieldModel) { }

    protected virtual void ReloadAmmo(int ammo)
    {
        currentAmmo = ammo;
    }

    protected virtual void FullyReloadAmmo()
    {
        currentAmmo = maxAmmo;
    }
}
