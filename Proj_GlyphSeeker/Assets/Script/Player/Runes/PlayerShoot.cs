using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] int maxAmmo;
    [SerializeField] protected float fireRate;
    [SerializeField] GameObject bulletRune;
    int currentAmmo = 0;
    
    bool canShoot,
         isCooldown;



    protected virtual void ShootBullet(GameObject bulletToShoot) { }

    protected virtual void ActivateShield(GameObject shieldModel) { }

    protected virtual void ReloadAmmo(int ammo) { }

    protected virtual void FullyReloadAmmo() { }
}
