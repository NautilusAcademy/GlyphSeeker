using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosiveRune : PlayerShoot
{
    [Header ("Componenti")]
    [SerializeField]
    public GameObject Projectile;
    [SerializeField]
    public Transform firePoint;
    [SerializeField]
    public AudioSource shootSound;
    [SerializeField]
    public Transform Cam;

    [Header("Variabili")]
    [SerializeField]
    public int Ammo;
    [SerializeField]
    public float ShootForce;
    [SerializeField]
    public float ShootCooldown;
    [SerializeField]
    public bool CanShoot;

    private void Update()
    {
        if (GameManager.inst.inputManager.Player.Fire.triggered && CanShoot==true && Ammo>0)
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        CanShoot = false;
        //Crea proiettili da sparare
        GameObject projectile = Instantiate(Projectile, firePoint.position, Cam.rotation);
        //Prende il rigidbody
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        //Aggiunge la forza al proiettile
        Vector3 forceToAdd = Cam.transform.forward * ShootForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        Ammo--;
        Invoke(nameof(ResetShootCD), ShootCooldown);
    }
    private void ResetShootCD()
    {
        CanShoot = true;
    }
}
