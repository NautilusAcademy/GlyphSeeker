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
    [SerializeField] List<PlayerShoot> playerShoot_scr;
    [SerializeField] Smaterializzatore purpleRune_scr;

    int i_selectedRune = 0;
    RuneType selectedRune;

    const int RUNES_MAX_NUM = 4;

    bool isAiming,
         canAim = true,
         isFirstRuneUnlocked,
         isPurpleRuneActive;



    private void Awake()
    {
        i_selectedRune = 0;
    }

    void Update()
    {
        //Controlla se ha attiva la Runa Viola (Smaterializzatore)
        //    (serve per capire se la mira e' quella
        //     normale o quella diminuita)
        isPurpleRuneActive = selectedRune == RuneType.Purple_Rune;

        //Puo' mirare solo quando NON ha selezionato
        //la runa blu (scudo) & quando ha un oggetto (con la runa viola)
        canAim = i_selectedRune < 3
                  ||
                 (selectedRune == RuneType.Purple_Rune  &&  purpleRune_scr.GetIsObjectInSlot());



        #region Controllo della mira / "azione"

        if (canAim)
        {
            //Prende l'input di mira
            InputAction inputAim = GameManager.inst.inputManager.Player.Aim;

            isAiming = inputAim.ReadValue<float>() > 0;

            //Avvicinamento della Camera quando si puo' mirare
            ChangeCamPos();
        }

        #endregion



        #region Cambiamento della Runa selez.

        //switch(opt_SO.GetRuneSelect())
        {
            #region --Selezione a rotazione--

            //case RuneSelectionType.MouseWheel:

                //Prende l'input di selezione delle rune
                InputAction inputNext = GameManager.inst.inputManager.Player.NextRune,
                            inputPrevious = GameManager.inst.inputManager.Player.PreviousRune;

                //Controllo e Switch delle rune
                if(inputNext.triggered)
                {
                    NextRune();
                }
                if(inputPrevious.triggered)
                {
                    PreviousRune();
                }

                //break;

            #endregion


            #region --Selezione con 4 tasti--

            //case RuneSelectionType.HoldAndSelect:

                //Prende l'input del tasto per selezionare una runa
                //e quello per ognuna delle rune
                /*
                InputAction inputSelect = GameManager.inst.inputManager.Player.RunePressDown;
                InputAction inputElectric = GameManager.inst.inputManager.Player.ElectricRune,
                            inputExplosive = GameManager.inst.inputManager.Player.ExplosiveRune,
                            inputShield = GameManager.inst.inputManager.Player.ShieldRune,
                            inputPurple = GameManager.inst.inputManager.Player.PurpleRune;
                //*/

                /*
                if(inputSelect.ReadValue<float>() > 0)
                {
                    if(inputElectric.triggered)
                    {
                        //TODO: da decidere la funzione per cambiare la runa con questo metodo
                        break;
                    }

                    if(inputExplosive.triggered)
                    {
                        //TODO: da decidere la funzione per cambiare la runa con questo metodo
                        break;
                    }

                    if(inputShield.triggered)
                    {
                        //TODO: da decidere la funzione per cambiare la runa con questo metodo
                        break;
                    }

                    if(inputPurple.triggered)
                    {
                        //TODO: da decidere la funzione per cambiare la runa con questo metodo
                        break;
                    }
                }
                //*/

                //break;

            #endregion
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
        //(e se non ha selezionato quella base)
        playerShoot_scr[0].enabled = !isFirstRuneUnlocked
                                      &&
                                     selectedRune == RuneType.BaseRune;




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

        return false;
    }

    void NextRune()
    {
        //if (isFirstRuneUnlocked)
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
        //if (isFirstRuneUnlocked)
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


    void ChangeCamPos()
    {
        cameraScr.SwitchMaxDist(isAiming, isPurpleRuneActive);
    }


    public int GetActiveRune()
    {
        return (int)selectedRune;
    }
}
