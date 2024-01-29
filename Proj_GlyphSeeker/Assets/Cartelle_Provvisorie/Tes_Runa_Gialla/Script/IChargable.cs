using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChargable
{
    public void Charge()
    {
        Debug.Log("Carica");
    }

    public void FullCharged()
    {

    }
}
