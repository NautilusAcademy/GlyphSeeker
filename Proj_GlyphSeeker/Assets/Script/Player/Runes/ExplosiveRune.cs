using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosiveRune : PlayerShoot
{
    [Space(20)]
    [SerializeField] float bulletSpeed;
    [SerializeField] Rigidbody bullet;
    [SerializeField] Transform raycastStartPos,
                               fakeFirePoint;

    void Update()
    {
        //Puo' sparare solo se ha le munizioni
        //e non ha finito di caricare la cadenza di tiro
        canShoot = currentAmmo > 0;

        //Prende l'input di sparo
        InputAction shootInput = GameManager.inst.inputManager.Player.Fire;

        if (shootInput.triggered && !isCooldown)
        {
            ShootBullet(bullet);

            StartCoroutine(WaitToShoot());
        }

        /*
         * TODO:
         * da discutere e sistemare l'intero script
         * (nomi, capire se scritto cosi' va bene, ecc...)
         * 
         */
    }

    IEnumerator WaitToShoot()
    {
        isCooldown = true;

        yield return new WaitForSeconds(fireRate);

        isCooldown = false;
    }

    protected override void ShootBullet(Rigidbody bulletToShoot)
    {
        if (canShoot)
        {
            //Crea il proeittile nel punto specifico
            Rigidbody clone;
            clone = Instantiate(bulletToShoot, fakeFirePoint.position, raycastStartPos.rotation);
            clone.velocity = raycastStartPos.forward * bulletSpeed;

            /* 
             * TODO:
             * inserire le funzioni per girare il proiettile
             * e per spararlo
             * 
             */

            //Diminuisce le munizoni
            currentAmmo--;
        }
    }
}
