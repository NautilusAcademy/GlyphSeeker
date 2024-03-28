using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] /*HealthSystem*/MonoBehaviour playerHealthSystScr;
    [SerializeField] RuneManager runeMng;

    [Header("——  Slider  ——")]
    [SerializeField] Image healthSlider;
    #region Tooltip()
    [Tooltip("Gli angoli in cui dovrà stare l'immagine-slider della salute"
             + "\n(entrambi sono tra [0°; 360°])"
             + "\n(P.S. il primo è sempre più piccolo del secondo e viceversa)")]
    #endregion
    [SerializeField] Vector2 healthAngleLimits = new Vector2(0, 60);
    [Space(10)]
    [SerializeField] Image ammoSlider;
    #region Tooltip()
    [Tooltip("L'angolo (sia positivo che negativo) in cui dovrà stare l'immagine-slider delle munizioni"
             + "\n(sempre tra [0; 0.5], ovvero [0°; 180°])")]
    #endregion
    [Range(0, 0.5f)]
    [SerializeField] float ammoSliderLimit = 0.2f;
    [SerializeField] TMP_Text currentAmmoText,
                              maxAmmoText;

    [Space(10), Header("——  Rune  ——")]
    [SerializeField] RectTransform runesWheel;
    [SerializeField] List<Image> allRunesImages;
    [SerializeField] float wheelRotSpeed = 7.5f;
    [Range(1, 3)]
    [SerializeField] float selectedRuneSizeMult = 1.75f;
    List<Vector2> startRunesImgSizes;
    [SerializeField] MeshRenderer arm_meshRndr;
    [SerializeField] Light arm_light;

    [Space(10)]
    [SerializeField] Image slotObjectPurpleRune;
    [SerializeField] Image slotObjectBGPurpleRune;
    [Space(10)]
    [SerializeField] Sprite spriteGenericObject;
    [SerializeField] Sprite spriteExplosiveBarrel;
    [SerializeField] Sprite spriteGenericEnemy;
    [SerializeField] Sprite spriteExplosiveEnemy;

    [Space(10), Header("——  Mirino  ——")]
    [SerializeField] Image crosshair;
    [Space(10)]
    [SerializeField] Color colorDefault_ch = Color.white;
    [SerializeField] Color colorUnavailable_ch = Color.white * 0.5f;
    [SerializeField] Color colorObjectInSlot_ch = Color.green;
    [SerializeField] List<Color> colorRunes_ch
                                        = new List<Color>()
                                        {
                                            Color.yellow,
                                            Color.red,
                                            Color.blue,
                                            Color.magenta
                                        };




    void Awake()
    {
        startRunesImgSizes = new();   //Reset della lista

        //Salva le dimensioni di ogni immagine delle rune
        foreach (Image runeImg in allRunesImages)
        {
            startRunesImgSizes.Add(runeImg.rectTransform.sizeDelta);
        }
    }

    void Update()
    {

    }


    #region Sez. Salute e Munizioni

    void ClampAnglesHealthSlider()
    {
        //Calcola la percentuale della salute,
        //la percentuale dell'angolo inizale
        //e la differenza tra i due angoli
        float hp_percent = 0.666f,//healthScr.GetCurrentHealth()
                                  // / healthScr.GetMaxHealth(),
              startAngle = healthAngleLimits.x / 360,
              angles_diff = (healthAngleLimits.y / 360) - startAngle;


        /* Limita il numero tra 0 e 1,
         * poi lo rende interno nell'angolo,
         * e infine gli aggiunge X
         * (--> da [0; 1] diventa [X; Y])
         */
        hp_percent = Mathf.Clamp01(hp_percent);
        hp_percent *= angles_diff;
        hp_percent += startAngle;


        healthSlider.fillAmount = hp_percent;
    }

    void ClampAnglesAmmoSlider()
    {
        //TODO: cambia lo slider (metti che quando hai la runa viola, si ricarica pian piano)

        //Calcola la percentuale della salute
        float ammo_percent = 0.555f;//runeMng.GetActiveRuneScript().currentAmmo
                                    // / runeMng.GetActiveRuneScript().maxAmmo;


        /* Limita il numero tra 0 e 1,
         * per poi limitarlo nel range [limite; 1-limite]
         * (es. limite=0.2 --> da [0; 1] diventa [0.2; 0.8])
         */
        ammo_percent = Mathf.Clamp01(ammo_percent);
        ammo_percent *= 1 - (2 * ammoSliderLimit);
        ammo_percent += ammoSliderLimit;


        ammoSlider.fillAmount = ammo_percent;
    }

    void ChangeAmmoTexts()
    {
        //currentAmmoText.text = runeMng.GetActiveRuneScript().currentAmmo;
        //maxAmmoText.text = runeMng.GetActiveRuneScript().maxAmmo;
    }

    #endregion


    #region Sez. Ruota delle Rune

    //void SetTriggerAnimator(string triggerName)
    //{
    //    runesWheel.SetTrigger(triggerName);
    //}
    //void SetTriggerAnimator(int triggerId)
    //{
    //    runesWheel.SetTrigger(triggerId);
    //}

    void RotateRunesWheel()
    {
        //Calcola l'angolo rispetto alla runa selezionata
        float targetAngle = runeMng.GetSelectedRuneIndex() switch
                            {
                                2 => 90,
                                3 => 180,
                                4 => -90,
                                _ => 0,
                            };

        /* Gli angoli:
         *  - Runa Gialla in alto ---> 0°
         *  - Runa Rossa in alto ----> 90°
         *  - Runa Blu in alto ------> 180°
         *  - Runa Viola in alto ----> -90°
         */


        Quaternion targetRot = Quaternion.Euler(Vector3.forward * targetAngle);    //Calcola la rotaz.


        //Ruota la wheel delle rune
        runesWheel.rotation = Quaternion.Slerp(runesWheel.rotation,
                                               targetRot,
                                               Time.deltaTime * wheelRotSpeed);


        //Ferma la rotazione delle immagini delle rune
        foreach (var rune in allRunesImages)
        {
            rune.transform.rotation = Quaternion.identity;
        }
    }

    void ShowUnlockedRunesImages()
    {
        //Mostra solo le immagini di ogni runa sbloccata solo
        //quando sono state sbloccate; se no, le nasconde tutte
        for (int i = 0; i < allRunesImages.Count; i++)
        {
            allRunesImages[i].enabled = runeMng.GetUnlockedRunesNum() > 0
                                          &&
                                        i < runeMng.GetUnlockedRunesNum();
        }
    }

    void ChangeRuneSize()
    {
        for (int i = 0; i < allRunesImages.Count; i++)
        {
            bool isThisRuneSelected = i == runeMng.GetSelectedRuneIndex()-1;
            Vector2 sizeToChange,
                    finalSize;

            //Se la runa selezionata corrisponde a questo indice,
            //la ingrandisce; se no, la porta alla dimensione originale
            sizeToChange = startRunesImgSizes[i] * (isThisRuneSelected ? selectedRuneSizeMult : 1);

            //Ridimensiona l'immagine
            finalSize = Vector2.Lerp(allRunesImages[i].rectTransform.sizeDelta,
                                      sizeToChange,
                                      Time.deltaTime * wheelRotSpeed * 2);

            //Imposta la nuova dimensione
            allRunesImages[i].rectTransform.sizeDelta = finalSize;
        }
    }

    void ChangeArmColor()
    {
        Material mat = arm_meshRndr.material;
        Color finalColor,
              colorToChange;

        //Prende il colore rispetto alla runa selezioanta
        finalColor = colorRunes_ch[runeMng.GetSelectedRuneIndex() - 1];

        //Cambia il colore del materiale
        colorToChange = Color.Lerp(mat.color, finalColor, Time.deltaTime * wheelRotSpeed * 2);

        //Imposta il colore della luce e dell'Emissive
        arm_light.color = colorToChange;
        mat.SetColor("_EmissionColor", colorToChange);
        

        //TODO: da rifinire il SetColor
    }

    #endregion


    #region Sez. Oggetto immagazzinato

    void ShowObjectSlot()
    {
        GameObject slotObj_obj = slotObjectPurpleRune.gameObject,
                   slotObjBG_obj = slotObjectBGPurpleRune.gameObject;

        //Attiva l'immagine e lo sfondo di essa
        //solo quando il giocatore ha selezionato la Runa Viola
        slotObj_obj.SetActive(runeMng.GetIsPurpleRuneActive());
        slotObjBG_obj.SetActive(runeMng.GetIsPurpleRuneActive());

        //Attiva l'immagine e lo sfondo di essa
        //solo quando il giocatore ha selezionato la Runa Viola
        slotObj_obj.SetActive(runeMng.GetIsPurpleRuneActive());
        slotObjBG_obj.SetActive(runeMng.GetIsPurpleRuneActive());
    }

    void ChangeTypeObjectSlot()
    {
        //Controllo su quale oggetto ha immagazzinato il giocatore
        slotObjectBGPurpleRune.sprite = runeMng.GetObjectInSlot() switch
        {
            RuneManager.SlotObjectType.Empty => null,
            RuneManager.SlotObjectType.GenericObj => spriteGenericObject,
            RuneManager.SlotObjectType.ExplosiveBarrel => spriteExplosiveBarrel,
            RuneManager.SlotObjectType.GenericEnemy => spriteGenericEnemy,
            RuneManager.SlotObjectType.KamikazeEnemy => spriteExplosiveEnemy,
            _ => null,
        };
    }

    #endregion


    #region Sez. Mirino

    void ChangeCrosshairColor()
    {
        //Prende la runa selezionata (macchina a stati)
        PlayerShoot selectedRune = runeMng.GetActiveRuneScript();


        if (selectedRune.GetRune_IsObjectInSlot())
        {
            //Quando ho un oggetto immagazzinato
            crosshair.color = colorObjectInSlot_ch;
        }
        else
        {
            if (selectedRune.GetRune_CanInteract())
            {
                //Quando puo' "interagire" con la runa selezionata
                //(funzione diversa per ogni script, tranne BaseShoot)
                colorObjectInSlot_ch = colorRunes_ch[runeMng.GetSelectedRuneIndex()-1];
            }
            else
            {
                if (selectedRune.GetRune_IsUnavailable())
                {
                    //Quando non posso interagire
                    //(es. non posso raccogliere un oggetto con la Runa Viola)
                    crosshair.color = colorUnavailable_ch;
                }
                else
                {
                    //Se non ha soddisfatto nessuna condizione
                    //imposta il colore di default
                    crosshair.color = colorDefault_ch;
                }
            }
        }
    }

    #endregion

    

    #region EXTRA - Cambiare l'Inspector

    private void OnValidate()
    {
        //Limita il range degli angoli della salute
        //(con un min di 0° e un max di 360°)
        healthAngleLimits.y = Mathf.Clamp(healthAngleLimits.y, healthAngleLimits.x, 360);
        healthAngleLimits.x = Mathf.Clamp(healthAngleLimits.x, 0, healthAngleLimits.y);

        //Imposta l'immagine della salute come "Filled - Radial 360"
        healthSlider.type = Image.Type.Filled;
        healthSlider.fillMethod = Image.FillMethod.Radial360;
        healthSlider.fillOrigin = 2;
        healthSlider.fillClockwise = false;



        //Imposta l'immagine delle munizioni come "Filled - Radial 180"
        ammoSlider.type = Image.Type.Filled;
        ammoSlider.fillMethod = Image.FillMethod.Radial180;
        ammoSlider.fillOrigin = 2;
        ammoSlider.fillClockwise = false;



        //Limita la vel. di rotazione della ruota delle rune
        wheelRotSpeed = Mathf.Clamp(wheelRotSpeed, 1, wheelRotSpeed);

        //Imposta la lista delle immagini delle rune sempre a 4 max
        LimitElementsInList(ref allRunesImages, 4, null);

        //Imposta la lista dei colori delle rune sempre a 4 max
        LimitElementsInList(ref colorRunes_ch, 4, Color.white);
    }

    /// <summary>
    /// Limita gli elementi nella lista passata,
    /// <br></br>non andando oltre il massimo o togliendone se in eccesso
    /// </summary>
    /// <param name="list">la lista da limitare</param>
    /// <param name="maxElem">il numero max di elementi</param>
    /// <param name="defaultElemet">elemento di default (tampone)</param>
    void LimitElementsInList<T>(ref List<T> list, int maxElem, T defaultElemet)
    {
        //Imposta la lista dei colori delle rune sempre al max
        if(list.Count != maxElem)
        {
            //Ne aggiunge se sono meno del max
            for (int i = list.Count; i < maxElem; i++)
            {
                list.Add(defaultElemet);
            }

            //Toglie tutti gli elementi in eccesso (oltre l'indice max)
            list.RemoveRange(maxElem, list.Count - maxElem);
        }
    }

    #endregion


    #region EXTRA - Gizmo

    private void OnDrawGizmosSelected()
    {
        //FillCrosshair_DARINOMINARE();
        //Disegna due linee verdi sull'immagine per indicare
        //gli angoli dello slider della salute
        Quaternion health_minRot = Quaternion.Euler(Vector3.forward * healthAngleLimits.x),
                   health_maxRot = Quaternion.Euler(Vector3.forward * healthAngleLimits.y);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(healthSlider.rectTransform.position, health_minRot * Vector3.up * 25);
        Gizmos.DrawRay(healthSlider.rectTransform.position, health_maxRot * Vector3.up * 25);



        //Disegna due linee blu sull'immagine per indicare
        //gli angoli dello slider delle munizioni
        float ammoAngle = (1-2*ammoSliderLimit) * 90;
        Quaternion ammo_minRot = Quaternion.Euler(Vector3.forward * -ammoAngle),
                   ammo_maxRot = Quaternion.Euler(Vector3.forward * ammoAngle);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ammoSlider.rectTransform.position, ammo_minRot * Vector3.down * 25);
        Gizmos.DrawRay(ammoSlider.rectTransform.position, ammo_maxRot * Vector3.down * 25);
    }

    #endregion
}
