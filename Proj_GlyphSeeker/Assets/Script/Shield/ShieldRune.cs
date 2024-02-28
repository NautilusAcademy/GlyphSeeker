using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class ShieldRune : MonoBehaviour//PlayerShoot
{
    //[SerializeField] PlayerRBMovement movemScr;
    [SerializeField] GameObject shieldModel;
    [SerializeField] Transform posToMove;
    [SerializeField] Camera playerCam;
    [Min(0)]
    [SerializeField] float distance;

    [Space(10)]   //Sezione da mettere in PlayerShoot
    [SerializeField] int maxShieldHp;
    int shieldHp;
    [Range(0, 5)]
    [SerializeField]
    float reloadMaxTime;

    [Space(10)]
    [SerializeField] float maxParryTime = 1;
    bool canParry = false;
    float currentParryTime = 0,
          currentOpenTime = 0;

    [Space(10)]
    [SerializeField] float knockbackForce_shield = 2.5f;




    private /*override*/ void Start()
    {
        shieldHp = maxShieldHp;

        //base.Start();

        currentParryTime = 0;
    }

    void Update()
    {
        //InputAction inputShield = GameManager.inst.inputManager.Player.Fire;
        //          inputReload = GameManager.inst.inputManager.Player.Aim;


        bool isShieldActive = false, //inputShield.ReadValue<float>() > 0,
             isShieldTriggered = false, //inputShield.triggered,
             isReloading = false, //inputReload.ReadValue<float>() > 0,
             canReload = isReloading && shieldHp < maxShieldHp;



        if (isShieldActive)
        {
            #region Parry

            //Attiva il Parry una volta
            //quando si preme il pulsante nel primo frame
            if (isShieldTriggered)
            {
                canParry = true;
            }

            if (currentParryTime >= maxParryTime)
            {
                //Se finisce il timer,
                //toglie la possibilita' di Parry
                canParry = false;
            }
            else
            {
                //Aumenta il timer se esso NON ha finito
                currentParryTime += Time.deltaTime;
            }

            #endregion


            #region Diminuzione dei proiettili col passare del tempo

            //Aumenta il numero di quando e' aperto
            currentOpenTime += Time.deltaTime;
            int openTime_int = Mathf.FloorToInt(currentOpenTime);

            //Dopo tot tempo, toglie ammo allo scudo
            if(openTime_int >= 1)
            {
                shieldHp--;
                print("Tolti AMMO scudo");

                currentOpenTime = 0;
            }

            #endregion
        }
        else
        {
            //Reset del Parry quando non si preme nulla
            canParry = false;

            currentParryTime = 0;
            currentOpenTime = 0;
        }



        //(Dis)Attiva lo scudo,
        //solo se si tiene premuto il pulsante & ha ancora HP
        shieldModel.SetActive(!(shieldHp > 0  &&  isShieldActive));


        //Porta lo scudo davanti alla telecamera
        Vector3 shieldPos = posToMove.position + posToMove.forward * distance;

        shieldModel.transform.position = shieldPos;
        shieldModel.transform.rotation = playerCam.transform.rotation;
    }


    private void OnTriggerEnter(Collider other)
    {
        //IBullet bulletCheck = other.GetComponent<IEnemy>();

        if (other.CompareTag("Bullet")) //(bulletCheck != null)
        {
            bool isFromFront = CheckFromFront(other.transform);

            if (canParry  &&  isFromFront)
            {
                //Se lo puo' parare e il proiettile arriva da davanti,
                //allora lo spedisce nella stessa direzione dello scudo
        //        other.ChangeVelocity(transform.forward, 0);
                
                //Piccolo rinculo al giocatore
        //        movemScr.Knockback(knockbackForce_shield);
            }
                
            //Toglie munizioni allo scudo
    //        shieldhp--;

            //Distrugge il proiettile
            Destroy(other);
        }
    }

    /// <summary>
    /// Calcola la direzione nel range [-1; 1]
    /// <br></br><i>(se si avvicina a 1 ----> sono nella stessa direzione)
    /// <br></br>(se si avvicina a -1 ---> sono in direzioni opposte)
    /// <br></br>(se si avvicina a 0 ----> sono perpendicolari)</i>
    /// </summary>
    bool CheckFromFront(Transform bullet)
    {
        float directionRange = Vector3.Dot(bullet.forward.normalized,
                                           transform.forward);

        return directionRange < 0;
    }
}
