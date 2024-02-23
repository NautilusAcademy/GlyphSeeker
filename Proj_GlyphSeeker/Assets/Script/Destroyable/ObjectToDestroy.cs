using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToDestroy : MonoBehaviour, IDestroyable
{
    [SerializeField] bool requireExplosion = true;



    public void DestroyObject(bool a)
    {
        //Mettere la logica quando viene distrutto
    }
}
