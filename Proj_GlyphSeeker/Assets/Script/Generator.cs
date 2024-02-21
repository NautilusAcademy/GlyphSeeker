using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IChargable
{
    [SerializeField]
    private int charge = 0;
    private int maxCharge = 1;

    public ParticleSystem particleSystem;
    public GameObject door;

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
        particleSystem.gameObject.SetActive(true);
        door.SetActive(false);
    }
}