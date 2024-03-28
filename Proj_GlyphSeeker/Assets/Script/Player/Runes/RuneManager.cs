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
    
    public enum SlotObjectType
    {
        Empty,
        GenericObj,
        ExplosiveBarrel,
        GenericEnemy,
        KamikazeEnemy
    }

    
    [Header("—— Script base ——")]
    //[SerializeField] SaveSO_Script save_SO;
    //[SerializeField] OptionsSO_Script opt_SO;
    [SerializeField] CameraRotation cameraScr;

    [Header("—— Rune ——")]
    [SerializeField] List<PlayerShoot> playerShoot_scr;
    [SerializeField] /*PurpleRune*/MonoBehaviour purpleRune_scr;

    int i_selectedRune = 0;
    RuneType selectedRune;
    SlotObjectType objectInSlot;

    const int RUNES_MAX_NUM = 4;
    int unlockedRunesNum;

    bool isAiming,
         canAim = true,
         isFirstRuneUnlocked,
         isPurpleRuneActive;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource selectRuneSfx;
    [SerializeField] AudioSource aimingSfx;
    [SerializeField] AudioSource removeAimingSfx;



    private void Awake()
    {
        i_selectedRune = 0;
    }

    void Update()
    {
        //Converte l'indice della runa selezionata nell'Enum
        selectedRune = (RuneType)i_selectedRune;


        //Controlla se ha attiva la Runa Viola (Smaterializzatore)
        //    (serve per capire se la mira e' quella
        //     normale o quella diminuita)
        isPurpleRuneActive = selectedRune == RuneType.Purple_Rune;

        //Puo' mirare solo quando NON ha selezionato
        //la runa blu (scudo)
        canAim = i_selectedRune < 3;
                 // ||
                 //(isPurpleRuneActive && purpleRune_scr.GetIsObjectInSlot());



        #region Controllo della mira / "azione"

        if (canAim)
        {
            //Prende l'input di mira
            InputAction inputAim = GameManager.inst.inputManager.Player.Aim;

            isAiming = inputAim.ReadValue<float>() > 0;

            //Avvicinamento della Camera quando si puo' mirare
            ChangeCamPos();


            //Feedback
            if (inputAim.triggered  ||  inputAim.WasReleasedThisFrame())
            {
                if (isAiming)
                {
                    aimingSfx.Play();
                }
                else
                {
                    removeAimingSfx.Play();
                }
            }
        }

        #endregion



        #region Cambio della Runa selezionata

        //Controlla se ho sbloccato la prima runa
        if (CanSwitchRune())
        {
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
        }

        #endregion
    }


    #region Gestione di selezione Rune

    bool CanSwitchRune()
    {
        //Quando posso cambiare runa...
        if (isFirstRuneUnlocked)
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

            return true;
        }
        else
        {
            return false;
        }
    }

    void NextRune()
    {
        #region OLD
        /*
        //Cambia l'indice della runa selezionata
        //con quello dopo, ciclandolo
        i_selectedRune = i_selectedRune >= RUNES_MAX_NUM
                            ? 1
                            : ++i_selectedRune;
        //*/
        #endregion

        UpdateUnlockedRunes();

        //Cambia l'indice della runa selezionata
        //con quello dopo, ciclandolo
        //(seleziona la prima runa successiva sbloccata)
        i_selectedRune = i_selectedRune >= unlockedRunesNum
                            ? 1
                            : ++i_selectedRune;


        //Feedback
        selectRuneSfx.PlayOneShot(selectRuneSfx.clip);
    }

    void PreviousRune()
    {
        #region OLD
        /*
        //Cambia l'indice della runa selezionata
        //con quello prima, ciclandolo
        i_selectedRune = i_selectedRune <= 1
                            ? RUNES_MAX_NUM
                            : --i_selectedRune;
        //*/
        #endregion

        UpdateUnlockedRunes();

        //Cambia l'indice della runa selezionata
        //con quello prima, ciclandolo
        //(seleziona la prima runa precedente sbloccata)
        i_selectedRune = i_selectedRune <= 1
                            ? unlockedRunesNum
                            : --i_selectedRune;


        //Feedback
        selectRuneSfx.PlayOneShot(selectRuneSfx.clip);
    }

    public void UpdateUnlockedRunes()
    {
        unlockedRunesNum = 0;   //Reset del numero

        //Aggiorna il numero di rune sbloccate
        //foreach (bool b in save_SO.GetUnlockedRunes())
        {
            if (true)//b)
            {
                unlockedRunesNum++;
            }
        }

        unlockedRunesNum = Mathf.Clamp(unlockedRunesNum, 1, 4);    //Clamp di sicurezza
    }

    #endregion


    public void UnlockFirstRune()
    {
        isFirstRuneUnlocked = true;
        i_selectedRune = 1;
    }

    public void AutoSelectRune(RuneType runeToSelect)
    {
        i_selectedRune = (int)runeToSelect;
    }


    void ChangeCamPos()
    {
        cameraScr.SwitchMaxDist(isAiming, isPurpleRuneActive);
    }


    #region Funz. Get personalizzate

    public int GetSelectedRuneIndex() => (int)selectedRune;

    public PlayerShoot GetActiveRuneScript() => playerShoot_scr[i_selectedRune];

    public bool GetIsPurpleRuneActive() => isPurpleRuneActive;

    public int GetUnlockedRunesNum() => unlockedRunesNum;

    public SlotObjectType GetObjectInSlot() => objectInSlot;

    #endregion
}
