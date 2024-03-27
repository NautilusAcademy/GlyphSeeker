using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchClass : MonoBehaviour
{
    [SerializeField]
    protected bool isActive;

    public virtual void ToggleSwitch()
    {
        isActive = !isActive;     
    }
}
