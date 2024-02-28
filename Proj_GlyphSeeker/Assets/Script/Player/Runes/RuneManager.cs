using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneManager : MonoBehaviour
{
    public enum RuneType
    {
        BaseRune,
        Yellow_Rune,
        Red_Rune,
        Blue_Rune,
        Purple_Rune,
    }
    
    
    [Header("—— Script base ——")]
    //[SerializeField] SaveSO_Script save_SO;
    //[SerializeField] OptionsSO_Script opt_SO;
    [SerializeField] CameraRotation cameraScr;

    [Header("—— Rune ——")]
    [SerializeField] List</*PlayerShoot*/MonoBehaviour> playerShoot_scr;
    [SerializeField] /*PurpleRune*/MonoBehaviour purpleRune_scr;
    int i_selectedRune = 0;
    RuneType selectedRune;

    const int RUNES_MAX_NUM = 4;

    bool isAiming = false,
         canAim = true,
         isFirstRuneUnlocked = false;



    private void Awake()
    {
        i_selectedRune = 0;
    }

    void Update()
    {
        //Puo' mirare solo quando ha selezionato le prime due rune
        //(quella elettrica e esplosiva, inclusa anche quella base)
        canAim = i_selectedRune < 3;


        #region Controllo della mira / "azione"

        if (canAim)
        {
            //Prende l'input di mira
            InputAction inputAim = GameManager.inst.inputManager.Player.Aim;

            isAiming = inputAim.ReadValue<float>() > 0;

            //Avvicinamento della Camera quando si puo' mirare
            ChangeCamPos(isAiming);
        }

        #endregion



        #region Cambiamento della Runa selez.

        //Prende l'input di selezione delle rune
        //InputAction inputSelect = GameManager.inst.inputManager.Player.RuneSelect;
        //InputAction inputNext = GameManager.inst.inputManager.Player.NextRune,
        //            inputPrevious = GameManager.inst.inputManager.Player.PreviousRune;

        //Controllo e Switch delle rune
        //if(inputNext.triggered)
        {
            //NextRune();
        }
        //if(inputPrevious.triggered)
        {
            //PreviousRune();
        }


        //
        SwitchRune();


        //Converte l'indice della runa selezionata nell'Enum
        selectedRune = (RuneType)i_selectedRune;

        #endregion
    }


    #region Gestione di selezione Rune

    bool SwitchRune()
    {
        //Disattiva tutti gli script
        //tranne quello attivo
        for (int i = 1; i < playerShoot_scr.Count; i++)
        {
            bool isScriptEnabled = i == i_selectedRune;

            playerShoot_scr[i].enabled = isScriptEnabled;
        }

        //Disattiva lo sparo base se il giocatore
        //ha sbloccato la prima runa
        playerShoot_scr[0].enabled = !isFirstRuneUnlocked;




        //
        switch (selectedRune)
        {
            //Runa Base
            case RuneType.BaseRune:
                break;


            //Runa Gialla - Elettrica
            case RuneType.Yellow_Rune:
                break;


            //Runa Rossa - Esplosiva
            case RuneType.Red_Rune:
                break;


            //Runa Blu - Scudo
            case RuneType.Blue_Rune:
                break;


            //Runa Gialla - Elettrica
            case RuneType.Purple_Rune:
                break;
        }


        //------------switch(opt_SO.GetRuneSelect())

        return false;
    }

    void NextRune()
    {
        if (isFirstRuneUnlocked)
        {
            //Cambia l'indice della runa selezionata
            //con quello dopo, ciclandolo
            i_selectedRune = i_selectedRune + 1 > RUNES_MAX_NUM
                               ? i_selectedRune = 1
                               : ++i_selectedRune;
        }
    }

    void PreviousRune()
    {
        if (isFirstRuneUnlocked)
        {
            //Cambia l'indice della runa selezionata
            //con quello prima, ciclandolo
            i_selectedRune = i_selectedRune - 1 <= 0
                               ? i_selectedRune = RUNES_MAX_NUM
                               : --i_selectedRune;
        }
    }

    #endregion


    public void UnlockFirstRune()
    {
        isFirstRuneUnlocked = true;
        i_selectedRune = 1;
    }


    void ChangeCamPos(bool isAiming)
    {
        cameraScr.SwitchMaxDist(isAiming);
    }


    public int GetActiveRune()
    {
        return (int)selectedRune;
    }
}
