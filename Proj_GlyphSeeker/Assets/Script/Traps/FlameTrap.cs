using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrap : MonoBehaviour
{
    [SerializeField] Transform flameStartPoint;
    [Space(10)]
    [SerializeField] float maxFlameDist = 6;
    [SerializeField] float flameSpeed = 5;
    [SerializeField] Vector2 boxcastDim = Vector2.one;
    [Space(10)]
    [SerializeField] float startTimeOffset;
    [SerializeField] float maxFlameTime = 10,
                           maxWaitTime = 7.5f;
    float currentFlameTime,
          currentWaitTime;
    
    bool isOn;
    float currentFlameDist;
    RaycastHit[] flameHits;
    bool hasHit;



    void Awake()
    {
        isOn = false;


        debug_ObjToMove_startPos = debug_ObjToMove.position;
    }

    /*
     * TODO:
     * - Aggiungi l'offset iniziale
     * - che il timer scorre solo quando è arrivato in posizione (0 o max)
     */

    void Update()
    {
        //Gestione di entrambi i Timer
        if (isOn)
        {
            #region Timer (acceso)

            //Reset del Wait Timer
            currentWaitTime = 0;

            //Alla fine del timer da attivo...
            if(currentFlameTime >= maxFlameTime)
            {
                //---

                isOn = false;
            }
            else
            {
                //Aumenta 
                currentFlameTime += Time.deltaTime;
            }

            #endregion
        }
        else
        {
            #region Timer (spento)

            //Reset del Flame Timer
            currentFlameTime = 0;

            //Alla fine del timer da spento...
            if(currentWaitTime >= maxWaitTime)
            {
                //---

                //Attiva la fiamma
                isOn = true;
            }
            else
            {
                currentWaitTime += Time.deltaTime;
            }

            #endregion
        }
     
        
        //Calcola la distanza di quanto deve andare
        //(spento = 0, acceso = max)
        float flame_targetDist = isOn
                                  ? maxFlameDist
                                  : 0;
        
        currentFlameDist = Mathf.MoveTowards(currentFlameDist, flame_targetDist, Time.deltaTime * flameSpeed);


        //Fa un BoxCast e ritorna ogni oggetto colpito
        flameHits = Physics.BoxCastAll(flameStartPoint.position,
                                       boxcastDim * 0.5f,
                                       flameStartPoint.forward,
                                       flameStartPoint.rotation,
                                       currentFlameDist,
                                       ~0,
                                       QueryTriggerInteraction.Collide);

        //Controlla ogni oggetto
        CheckIfHasHit();



        DebugFunction();
    }

    void CheckIfHasHit()
    {
        //Controllo per ogni oggetto colpito
        //se e' uno di quelli interessati (nemico, giocatore o scudo)
        foreach (RaycastHit hit in flameHits)
        {
            bool isShield = hit.transform.GetComponent<ShieldRune>();
            bool isEnemyOrPlayer = hit.transform.CompareTag("Enemy")
                                    ||
                                   hit.transform.CompareTag("Player");
            //HealthSystem enemyOrPlayerCheck = hit.transform.GetComponent<HealthSystem>();


            //Se ha colpito un nemico o il giocatore
            if(isShield || isEnemyOrPlayer)//enemyOrPlayerCheck)
            {
                if (isEnemyOrPlayer)//enemyOrPlayerCheck)
                {
                    //Lo danneggia
                    //enemyOrPlayerCheck.TakeDamage(enemyOrPlayerCheck.GetMaxHealth() * 0.5f);
                }

                hasHit = true;

                //DEBUG
                debug_flameHit = hit;

                return;
            }
        }


        //Se non ha colpito gli oggetti desiderati...
        hasHit = false;
        return;
    }



    #region EXTRA - Cambiare l'Inspector

    private void OnValidate()
    {
        //Limita ogni variabile numerica
        //(sempre positivi)
        startTimeOffset = Mathf.Clamp(startTimeOffset, 0, startTimeOffset);
        maxFlameTime = Mathf.Clamp(maxFlameTime, 0, maxFlameTime);
        maxWaitTime = Mathf.Clamp(maxWaitTime, 0, maxWaitTime);
        maxFlameDist = Mathf.Clamp(maxFlameDist, 0, maxFlameDist);
        flameSpeed = Mathf.Clamp(flameSpeed, 0, flameSpeed);
    }

    #endregion


    #region EXTRA - Gizmo

    private void OnDrawGizmos()
    {
        //Disegna la distanza max a cui puo' arrivare la fiamma
        Gizmos.color = new Color(0.85f, 0.85f, 0.85f, 0.65f);
        Gizmos.DrawCube(flameStartPoint.position + flameStartPoint.forward * maxFlameDist,
                        new Vector3(0.4f, 0.4f, 0.03f));
    }

    private void OnDrawGizmosSelected()
    {
        //Disegna il BoxCast
        Gizmos.color = Color.white * 0.85f;
        Gizmos.DrawLine(flameStartPoint.position,
                        flameStartPoint.position + flameStartPoint.forward * currentFlameDist);
        Gizmos.DrawWireCube(flameStartPoint.position + flameStartPoint.forward * currentFlameDist,
                            (Vector3)boxcastDim + Vector3.forward * 0.1f);
        

        if (hasHit)
        {
            Gizmos.color = Color.green;
            //Disegna dove ha colpito se ha colpito un'oggetto solido (no trigger)
            Gizmos.DrawLine(flameStartPoint.position, debug_flameHit.point);
            Gizmos.DrawCube(debug_flameHit.point, Vector3.one * 0.1f);
        }
    }

    #endregion
    

    #region DEBUG

    [Space(10), Header("\tDEBUG")]
    [SerializeField] Transform debug_scaleObj;
    [SerializeField] Transform debug_ObjToMove;
    Vector3 debug_ObjToMove_startPos;
    
    RaycastHit debug_flameHit;

    void DebugFunction()
    {
        debug_scaleObj.position = flameStartPoint.position + flameStartPoint.forward * ((hasHit ? debug_flameHit.distance : currentFlameDist) / 2);
        debug_scaleObj.localScale = (Vector3)boxcastDim + flameStartPoint.forward * (hasHit ? debug_flameHit.distance : Mathf.Clamp(currentFlameDist, 0.01f, maxFlameDist));


        debug_ObjToMove.position = debug_ObjToMove_startPos + Vector3.right * 2 * Mathf.Sin(Time.realtimeSinceStartup * 2);
    }

    #endregion
}
