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

    bool rune_isUnavailable,
         rune_isObjectInSlot,
         rune_canInteract;



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


    #region Funz. Set personalizzate

    protected virtual void SetRune_IsUnavailable(bool value)
    {
        rune_isUnavailable = value;
    }

    protected virtual void SetRune_IsObjectInSlot(bool value)
    {
        rune_isObjectInSlot = value;
    }

    protected virtual void SetRune_CanInteract(bool value)
    {
        rune_canInteract = value;
    }

    #endregion


    #region Funz. Get personalizzate

    public bool GetRune_IsUnavailable() => rune_isUnavailable;
    public bool GetRune_IsObjectInSlot() => rune_isObjectInSlot;
    public bool GetRune_CanInteract() => rune_canInteract;


    #endregion
}
