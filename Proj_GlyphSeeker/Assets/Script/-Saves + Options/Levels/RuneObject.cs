using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RuneObject : MonoBehaviour
{
    [SerializeField] SaveSO_Script save_SO;
    [Space(10)]
    //[SerializeField] RuneManager.RuneType thisRuneType;
    bool isUnlocked;



    void Awake()
    {
        //Controlla se e' gia' stato raccolto
        CheckUnlocked();
    }


    private void OnTriggerEnter(Collider other)
    {
        //IPlayer playerCheck = other.GetComponent<IPlayer>();

        if (other.CompareTag("Player"))//playerCheck != null)
        {
            isUnlocked = true;


            //Lo aggiunge come "raccolta" alla lista
            //save_SO.ChangeUnlockedRune((int)thisRuneType - 1, isCollected);

            //Seleziona in automatico questa runa
            //other.GetComponentInChildren<RuneManager>().AutoSelectRune(thisRuneType);


            //Toglie la runa dalla scena
            CheckUnlocked();
        }
    }

    void CheckUnlocked()
    {
        //(Dis)Attiva la runa se e' stato raccolto o no
        gameObject.SetActive(!isUnlocked);
    }


    public void LoadRuneObj(bool newIsCollected)
    {
        //Cambia la runa da 
        //e lo attiva o disattiva di conseguenza
        isUnlocked = newIsCollected;
            
        CheckUnlocked();
    }
}
