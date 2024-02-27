using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSelection : MonoBehaviour
{
    [SerializeField] private GameObject runeActive1, runeActive2, runeActive3, runeActive4;
    [SerializeField] private GameObject weaponMenu;
    [SerializeField] private Shoot runaGiallaScript;
    [SerializeField] private Runa_Rossa runaRossaScript;
    [SerializeField] private GameObject runaBluScriptObj;
    [SerializeField] private Smaterializzatore runaViolaScript;

    private void Start()
    {
        weaponMenu.SetActive(false);
        ActivateGialla();
    }

    void Update()
    {
        if (GameManager.inst.inputManager.Player.WeaponMenu.IsPressed())
        {
            weaponMenu.SetActive(true);

            // Controlla il tasto premuto in combinazione con il tasto per la selezione
            if (GameManager.inst.inputManager.Player.Jump.WasPressedThisFrame())
            {
                ActivateGialla();
            }
            else if (GameManager.inst.inputManager.Player.Aim.WasPressedThisFrame())
            {
                ActivateRossa();
            }
            else if (GameManager.inst.inputManager.Player.Fire.WasPressedThisFrame())
            {
                ActivateBlu();
            }
            else if (GameManager.inst.inputManager.Player.Provvisorio.WasPressedThisFrame())
            {
                ActivateViola();
            }

        }
        else
        {
            weaponMenu.SetActive(false);
        }
    }

    void ActivateRune(GameObject rune)
    { 
        // Fornisce la lista di oggetti da disattivare quando viene richiamata una runa nello specifico
        runeActive1.SetActive(false);
        runeActive2.SetActive(false);
        runeActive3.SetActive(false);
        runeActive4.SetActive(false);

        rune.SetActive(true);
    }

    void ActivateGialla()
    {
                ActivateRune(runeActive1);
                runaGiallaScript.enabled = enabled;
                runaRossaScript.enabled = false;
                runaBluScriptObj.SetActive(false);
                runaViolaScript.enabled = false;
    }

    void ActivateRossa()
    {
                ActivateRune(runeActive2);
                runaGiallaScript.enabled = false;
                runaRossaScript.enabled = enabled;
                runaBluScriptObj.SetActive(false);
                runaViolaScript.enabled = false;
    }

    void ActivateBlu()
    {
                ActivateRune(runeActive3);
                runaGiallaScript.enabled = false;
                runaRossaScript.enabled = false;
                runaBluScriptObj.SetActive(true);
                runaViolaScript.enabled = false;
    }

    void ActivateViola()
    {
                ActivateRune(runeActive4);
                runaGiallaScript.enabled = false;
                runaRossaScript.enabled = false;
                runaBluScriptObj.SetActive(false);
                runaViolaScript.enabled = enabled;
    }
}
