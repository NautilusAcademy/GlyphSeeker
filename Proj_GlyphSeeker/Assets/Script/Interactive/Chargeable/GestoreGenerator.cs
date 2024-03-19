using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoreGenerator : MonoBehaviour
{
    [SerializeField]
    private int currentCharge;
    [SerializeField]
    private int maxCharge;

    private void Start()
    {
        currentCharge = 0;
    }

    public void UpdateCharge()
    {
        currentCharge++;

        if(currentCharge >= maxCharge)
        {
            
        }
    }

    public void DecreaseCharge()
    {
        currentCharge--;
    }
}
