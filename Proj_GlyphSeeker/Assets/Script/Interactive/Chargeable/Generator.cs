using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IChargeable
{
    [SerializeField]
    private int charge = 0;
    [SerializeField]
    private int maxCharge = 1;

    public void Charge()
    {
        charge++;

        if (charge >= maxCharge)
        {
            FullCharged();
        }
    }

    public void FullCharged()
    {
        
    }
}