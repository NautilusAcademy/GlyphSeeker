using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsantiRunaGialla : MonoBehaviour, IChargable
{
    [SerializeField] private int charge = 0;
    [SerializeField] private int maxCharge = 1;

    //public ParticleSystem particleSystem;
    [SerializeField] private MeshRenderer buttonRenderer;
    public GameObject toActive;
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
        buttonRenderer.material.color = Color.blue;
        //particleSystem.gameObject.SetActive(true);
        toActive.SetActive(true);
    }
}