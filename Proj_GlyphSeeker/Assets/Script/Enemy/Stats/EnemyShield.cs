using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour // Script da associare soltanto ai nemici con corazza
{
    [SerializeField]
    private int maxShieldHealth;
    private int currentShieldHealth;

    //private PickUp pickUp;
    
    private void RemoveShield() // Chiama EnablePickUp e disattiva il gameobject della corazza
    {
        EnablePickUp();
        gameObject.SetActive(false);
    }

    private void EnablePickUp() // Attiva il bool canPickUp del padre per permettere il giocatore di smaterializzarlo
    {
        /*PickUp father = gameObject.transform.GetComponentInParent<PickUp>();
        father.canPickUp = true;*/
    }
}
