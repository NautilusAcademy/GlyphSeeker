using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestoreGenerator : MonoBehaviour
{
    [SerializeField]
    private int currentCharge;
    [SerializeField]
    private int maxCharge;

    [SerializeField]
    UnityEvent onMaxCharge;
    [SerializeField]
    UnityEvent onMinCharge;

    private void Start()
    {
        currentCharge = 0;
    }

    public void UpdateCharge()
    {
        currentCharge++;

        if(currentCharge >= maxCharge)
        {
            onMaxCharge.Invoke();
        }
    }

    public void DecreaseCharge()
    {
        if (currentCharge == maxCharge)
        {
            onMinCharge.Invoke();
        }

        currentCharge--;
    }
}
