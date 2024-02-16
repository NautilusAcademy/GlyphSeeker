using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField]
    private int maxShieldHealth;
    private int currentShieldHealth;

    //private PickUp pickUp;
    
    private void RemoveShield()
    {
        EnablePickUp();
        gameObject.SetActive(false);
    }

    private void EnablePickUp()
    {
        /*PickUp father = gameObject.transform.GetComponentInParent<PickUp>();
        father.canPickUp = true;*/
    }
}
