using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class ShieldRune : PlayerShoot
{
    [SerializeField] PlayerRBMovement movemScr;
    [SerializeField] GameObject shieldModel;
    [SerializeField] Transform playerCam_Tr;
    Collider coll;
    [Min(0)]
    [SerializeField] float distance;
    [SerializeField] Vector2 offset;

    [Space(10)]
    [Min(0.1f)]
    [SerializeField] float maxParryTime = 1;
    [Min(0.1f)]
    [SerializeField] float velLoseAmmo = 1;
    bool canParry = false;
    float currentParryTime = 0,
          currentOpenTime = 0;

    [Space(10)]
    [SerializeField] float knockbackForce_shield = 2.5f;




    private void Start()
    {
        coll = GetComponent<Collider>();

        currentParryTime = 0;
        currentOpenTime = 0;
    }

    void Update()
    {
        InputAction inputShield = GameManager.inst.inputManager.Player.Aim;


        bool isShieldActive = inputShield.ReadValue<float>() > 0,
             isShieldTriggered = inputShield.triggered;



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

            //Aumenta il "timer" da quando e' stato aperto
            currentOpenTime += Time.deltaTime;

            //Dopo tot tempo, toglie ammo allo scudo
            if(currentOpenTime >= velLoseAmmo)
            {
                currentAmmo--;

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
        bool canUseShield = currentAmmo > 0  &&  isShieldActive;
        
        coll.enabled = canUseShield;
        shieldModel.SetActive(canUseShield);


        //Porta lo scudo davanti alla telecamera
        Vector3 shieldPos = playerCam_Tr.position
                             + playerCam_Tr.forward * distance
                             + (Vector3)offset;

        transform.position = shieldPos;
        transform.position = shieldPos;
        shieldModel.transform.position = shieldPos;
        shieldModel.transform.rotation = playerCam_Tr.rotation;
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
                movemScr.Knockback(other.transform.forward, knockbackForce_shield);
            }
                
            //Toglie munizioni allo scudo
            currentAmmo--;

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


    #region EXTRA - Gizmo

    private void OnDrawGizmosSelected()
    {
        Color myBlue = new Color(0, 0.35f, 0.75f),
              myLightBlue = new Color(0, 0.5f, 0.75f);
        Vector3 startPos = playerCam_Tr.position + playerCam_Tr.forward * distance,
                finalPos = playerCam_Tr.position
                            + playerCam_Tr.forward * distance
                            + (Vector3)offset;

        //Disegna una sfera dove si trovera' lo scudo rispetto alla cam
        //(inizialmente blu,
        // ma diventa azzurrino quando e se si aggiunge l'offset)
        Gizmos.color = offset == Vector2.zero
                        ? myBlue
                        : myLightBlue;
        Gizmos.DrawSphere(finalPos, 0.15f);

        //Disegna la linea dalla posizione iniziale a quella finale
        Gizmos.color = myBlue;
        Gizmos.DrawLine(playerCam_Tr.position, startPos);
        Gizmos.color = myLightBlue;
        Gizmos.DrawLine(startPos, finalPos);
    }

    #endregion
}
