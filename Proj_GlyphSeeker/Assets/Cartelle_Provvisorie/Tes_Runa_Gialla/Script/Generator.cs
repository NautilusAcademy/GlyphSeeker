using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IChargable
{
    private int charge = 0;
    private int maxCharge = 1;

    public GameObject door;

    private void Update()
    {
        if(charge >= maxCharge)
        {
            FullCharged();
        }
    }

    public void Charge()
    {
        charge++;
    }

    public void FullCharged()
    {
        door.SetActive(false);
    }
}