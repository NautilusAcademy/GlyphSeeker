using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runa_Rossa : MonoBehaviour
{
    [SerializeField]
    public Transform Camera;
    public Transform AimPoint;
    public GameObject Projectile;
    public int Ammo;
    public float ShootCD;
    //public KeyCode ShootKey = KeyCode.Mouse0;
    public float ShootForce;
    public float ShootUpwardForce;
    bool CanShoot;

    void Start()
    {
        CanShoot = true;
    }

    private void Update()
    {
        if(GameManager.inst.inputManager.Player.Fire.ReadValue<float>() > 0 && CanShoot==true && Ammo>0)
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        CanShoot = false;
        GameObject toshoot = Instantiate(Projectile, AimPoint.position, Camera.rotation);
        Rigidbody ProjRB = toshoot.GetComponent<Rigidbody>();
        Vector3 ForceToAdd = Camera.transform.forward * ShootForce + transform.up * ShootUpwardForce;
        ProjRB.AddForce(ForceToAdd, ForceMode.Impulse);
        Ammo--;

        Invoke(nameof(ResetShootTime), ShootCD);
    }

    private void ResetShootTime()
    {
        CanShoot = true;
    }
}
