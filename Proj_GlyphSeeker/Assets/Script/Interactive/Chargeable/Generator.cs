using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : SwitchClass, IChargeable
{
    [SerializeField]
    GestoreGenerator gestore;

    public override void ToggleSwitch()
    {
        base.ToggleSwitch();

        if (isActive)
            gestore.UpdateCharge();
        else
            gestore.DecreaseCharge();
    }
}