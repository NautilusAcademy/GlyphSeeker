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
    [SerializeField] bool startOnAwake;
    float currentFlameTime,
          currentWaitTime,
          currentStartTime;

    [Space(20)]
    [Range(0, 90)]
    [SerializeField] float shieldAngleTollerance = 50f;
    float shield_radAngle;
    
    bool isOn,
         start_doOnce;
    float currentFlameDist;
    bool hasHit;
    List<RaycastHit> flameHits;
    
    RaycastHit debug_flameHit;



    void Awake()
    {
        isOn = startOnAwake;

        //Converte l'angolo per comparare dopo
        //se lo scudo ha parato questa trappola da davanti
        shield_radAngle = -Mathf.Cos(shieldAngleTollerance * Mathf.Deg2Rad);

        /* Trasforma l'angolo gradi --> radianti,
         * calcola il suo Cos(),
         * e poi lo moltiplica per -1
         */


        if(debug_ObjToMove)
            debug_ObjToMove_startPos = debug_ObjToMove.position;
    }

    void Update()
    {
        #region Timer (offset iniziale)
        
        //All'inizio del gioco, aspetta l'offset del timer
        //e appena finisce, puo' attivare la trappola
        if (!start_doOnce)
        {
            if(currentStartTime >= startTimeOffset)
            {
                start_doOnce = true;
            }
            else
            {
                currentStartTime += Time.deltaTime;
            }

        }

        #endregion


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
            else if (start_doOnce)
            {
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
            else if (start_doOnce)
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
        RaycastHit[] allHits = Physics.BoxCastAll(flameStartPoint.position,
                                                  boxcastDim * 0.5f,
                                                  flameStartPoint.forward,
                                                  flameStartPoint.rotation,
                                                  currentFlameDist,
                                                  ~0,
                                                  QueryTriggerInteraction.Collide);

        flameHits = new List<RaycastHit>(allHits);


        //Controlla ogni oggetto colpito
        //(solo quando la trappola e' attiva)
        if (isOn)
        {
            CheckEveryHit();
        }



        DebugFunction();
    }


    void CheckEveryHit()
    {
        #region Ri-organizza gli Hits

        //Ri-organizza la lista di Hits,
        //mettendo come primi quelli con lo script dello scudo
        for (int i = 0; i < flameHits.Count; i++)
        {
            if (flameHits[i].transform.GetComponent<ShieldRune>())
            {
                //Sposta gli script dello scudo
                //che trova nella lista come primi
                RaycastHit temp_hit = flameHits[i];
                flameHits.RemoveAt(i);
                flameHits.Insert(0, temp_hit);
            }
        }

        #endregion


        //Controllo per ogni oggetto colpito
        //se e' uno di quelli interessati (nemico, giocatore o scudo)
        foreach (RaycastHit hit in flameHits)
        {
            bool isShield = hit.transform.GetComponent<ShieldRune>();
            HealthSystem enemyOrPlayerCheck = hit.transform.GetComponent<HealthSystem>();


            //Se ha colpito lo scudo (da davanti),
            //o un nemico o il giocatore
            if ((isShield && CheckFromFront(hit.transform)) || enemyOrPlayerCheck)
            {
                if (enemyOrPlayerCheck)
                {
                    //Lo danneggia
                    //enemyOrPlayerCheck.TakeDamage(enemyOrPlayerCheck.GetMaxHealth() * 0.5f);;
                    print($"{name}.{nameof(FlameTrap)}: Danneggiato \"{hit.transform.name}\"");
                }

                //DEBUG
                debug_flameHit = hit;

                hasHit = true;

                return;
            }
        }


        //Se non ha colpito gli oggetti desiderati...
        hasHit = false;
        return;
    }

    /// <summary>
    /// Calcola la direzione nel range [-1; 1]
    /// <br></br><i>(se si avvicina a 1 ----> sono nella stessa direzione)
    /// <br></br>(se si avvicina a -1 ---> sono in direzioni opposte)
    /// <br></br>(se si avvicina a 0 ----> sono perpendicolari)</i>
    /// </summary>
    bool CheckFromFront(Transform shieldObj)
    {
        float directionRange = Vector3.Dot(shieldObj.forward,
                                           flameStartPoint.forward);

        return directionRange < shield_radAngle;
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


        //Disegna il BoxCast
        Gizmos.color = Color.white * 0.85f;
        Gizmos.DrawLine(flameStartPoint.position,
                        flameStartPoint.position + flameStartPoint.forward * currentFlameDist);
        Gizmos.DrawWireCube(flameStartPoint.position + flameStartPoint.forward * currentFlameDist,
                            (Vector3)boxcastDim + Vector3.forward * 0.1f);
        

        if (hasHit && isOn)
        {
            //Disegna il vettore "davanti" (utile per vedere lo scudo)
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(debug_flameHit.point, debug_flameHit.transform.forward);
        

            //Disegna dove ha colpito se ha colpito un collider
            Gizmos.color = Color.green;
            Gizmos.DrawRay(debug_flameHit.point, -flameStartPoint.forward * debug_flameHit.distance);
            Gizmos.DrawCube(debug_flameHit.point, Vector3.one * 0.1f);
        }
    }

    #endregion
    

    #region -- DEBUG --

    [Space(10), Header("\tDEBUG")]
    [SerializeField] Transform debug_scaleObj;
    [SerializeField] Transform debug_ObjToMove;
    Vector3 debug_ObjToMove_startPos;

    void DebugFunction()
    {
        if (debug_scaleObj)
        {
            debug_scaleObj.position = flameStartPoint.position
                                      + flameStartPoint.forward * ((hasHit
                                                                      ? debug_flameHit.distance
                                                                      : currentFlameDist) / 2);
            debug_scaleObj.localScale = (Vector3)boxcastDim
                                        + flameStartPoint.forward * (hasHit
                                                                      ? debug_flameHit.distance
                                                                      : Mathf.Clamp(currentFlameDist, 0.01f, maxFlameDist));
        }


        if (debug_ObjToMove)
        {
            debug_ObjToMove.position = debug_ObjToMove_startPos
                                       + Vector3.right * 2 * Mathf.Sin(Time.realtimeSinceStartup * 2);
        }
    }

    #endregion
}
