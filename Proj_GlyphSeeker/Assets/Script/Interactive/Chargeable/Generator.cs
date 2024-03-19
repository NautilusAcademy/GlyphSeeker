using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IChargeable
{
    [SerializeField]
    private bool charged;
    [SerializeField]
    GestoreGenerator gestore;

    public void Charge()
    {
        if(charged == false)
        {
            charged = true;
            gestore.UpdateCharge();
        }
        else
        {
            charged = false;
            gestore.DecreaseCharge();
        }
    }
}