using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour // Script da associare soltanto ai nemici con corazza
{
    [SerializeField]
    private int maxShieldHealth;
    private int currentShieldHealth;
<<<<<<< HEAD

    //private PickUp pickUp;
    
    private void RemoveShield() // Chiama EnablePickUp e disattiva il gameobject della corazza
    {
        EnablePickUp();
=======
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

        if(gameObject.transform.GetComponentInParent<PickUp>() != null)
        {
            EnablePickUp();
        }

>>>>>>> origin/Enemy
        gameObject.SetActive(false);
    }

    private void EnablePickUp() // Attiva il bool canPickUp del padre per permettere il giocatore di smaterializzarlo
    {
<<<<<<< HEAD
        /*PickUp father = gameObject.transform.GetComponentInParent<PickUp>();
        father.canPickUp = true;*/
=======
        PickUp father = gameObject.transform.GetComponentInParent<PickUp>();
        father.canPickUp = true;
>>>>>>> origin/Enemy
    }
}
