using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour // Script da associare soltanto ai nemici con corazza
{
    [SerializeField]
    private int maxShieldHealth;
    [SerializeField]
    private int currentShieldHealth;
    public bool isShieldActive = true;

    private void Start()
    {
        currentShieldHealth = maxShieldHealth;
    }

    public void CrackShield(int damage) // Danneggia lo scudo e lo distrugge se raggiunge 0 salute
    {
        currentShieldHealth -= damage;

        if(currentShieldHealth <= 0)
        {
            RemoveShield();
        }
    }

    private void RemoveShield() // Chiama EnablePickUp e disattiva il gameobject della corazza
    {
        isShieldActive = false;

        if(gameObject.transform.GetComponent<PickUp>() != null)
        {
            EnablePickUp();
        }
    }

    private void EnablePickUp() // Attiva il bool canPickUp del padre per permettere il giocatore di smaterializzarlo
    {
        PickUp pickUp = gameObject.transform.GetComponent<PickUp>();
        pickUp.canPickUp = true;
    }
}
