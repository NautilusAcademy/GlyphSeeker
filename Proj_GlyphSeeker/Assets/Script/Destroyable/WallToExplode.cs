using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallToExplode : ObjectToDestroy
{
    [SerializeField] UnityEvent onDestroyed;


    [Header("——  Feedback  ——")]
    [SerializeField] AudioSource destroyedSfx;
    [SerializeField] ParticleSystem destroy_part;



    public void DestroyWall()
    {
        //Attiva ogni funzione dentro dello UnityEvent
        onDestroyed.Invoke();


        //Feedback
        destroyedSfx.Play();
        destroy_part.Play();


        //Distrugge l'oggetto con lo script
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
