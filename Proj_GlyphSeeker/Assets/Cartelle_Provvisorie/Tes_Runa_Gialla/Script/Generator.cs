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

    [SerializeField] List<MonoBehaviour> scriptToActivate;
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
        //door.SetActive(true);
        foreach (MonoBehaviour scripts in scriptToActivate)
        {
            scripts.enabled = enabled;
        }
    }
}