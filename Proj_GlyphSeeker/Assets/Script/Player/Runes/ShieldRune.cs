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

    [Space(10), Header("—— Feedback ——")]
    [SerializeField] AudioSource shieldDamagedSfx;
    [SerializeField] AudioSource shieldBrokenSfx;
    [SerializeField] AudioSource parrySfx;
    [SerializeField] ParticleSystem parry_part,
                                    shieldBroken_part;




    private void Start()
    {
        coll = GetComponent<Collider>();
        coll.enabled = false;
        shieldModel.SetActive(false);

        currentParryTime = 0;
        currentOpenTime = 0;
    }

    void Update()
    {
        InputAction inputShield = GameManager.inst.inputManager.Player.Fire;


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
            if (currentOpenTime >= velLoseAmmo)
            {
                currentAmmo--;

                currentOpenTime = 0;


                //Feedback - danno allo scudo
                FeedbackDamagedShield();
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
        bool canUseShield = currentAmmo > 0 && isShieldActive;

        coll.enabled = canUseShield;
        shieldModel.SetActive(canUseShield);


        //Porta lo scudo davanti alla telecamera
        //(con offset aggiunto)
        //(P.S. si moltiplica per la rotazione per renderlo "locale")
        Vector3 shieldPos = playerCam_Tr.position
                             + playerCam_Tr.forward * distance
                             + playerCam_Tr.rotation * offset;

        transform.position = shieldPos;
        shieldModel.transform.position = shieldPos;
        shieldModel.transform.rotation = playerCam_Tr.rotation;
    }


    private void OnTriggerEnter(Collider other)
    {
        IBullet bulletCheck = other.GetComponent<IBullet>();

        if (bulletCheck != null)
        {
            bool isFromFront = CheckFromFront(other.transform);

            if (canParry  &&  isFromFront)
            {
                //Se lo puo' parare e il proiettile arriva da davanti,
                //allora lo spedisce nella stessa direzione dello scudo
                other.GetComponent<EnemyBaseBullet>().ChangeVelocity(transform.forward, 0);

                //Feedback - parry
                parrySfx.Play();

                parry_part.transform.position = other.transform.position;
                parry_part.transform.rotation = other.transform.rotation;

                parry_part.Play();    //Mostra le particelle dopo averli posizionati
                                      //e ruotati correttamente
            }

            //Toglie munizioni allo scudo
            currentAmmo--;

            //Piccolo rinculo al giocatore
            movemScr.Knockback(other.transform.forward, knockbackForce_shield);

            //Distrugge il proiettile
            Destroy(other);

            //Feedback - danno allo scudo
            FeedbackDamagedShield();
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


    void FeedbackDamagedShield()
    {
        //Mostra il feedback del danno o della rottura
        //dello scudo rispetto alle munizioni
        if (currentAmmo <= 0)
        {
            shieldDamagedSfx.PlayOneShot(shieldDamagedSfx.clip);
        }
        else
        {
            shieldBrokenSfx.PlayOneShot(shieldBrokenSfx.clip);
            shieldBroken_part.Play();
        }
    }


    #region EXTRA - Gizmo

    private void OnDrawGizmosSelected()
    {
        Color myBlue = new Color(0, 0.35f, 0.75f),
              myLightBlue = new Color(0, 0.5f, 0.75f);
        Vector3 startPos = playerCam_Tr.position + playerCam_Tr.forward * distance,
                finalPos = playerCam_Tr.position
                            + playerCam_Tr.forward * distance
                            + playerCam_Tr.rotation * offset;

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
