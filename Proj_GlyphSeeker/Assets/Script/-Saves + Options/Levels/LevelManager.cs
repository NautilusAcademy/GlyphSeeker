using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] SaveSO_Script save_SO;
    
    [SerializeField] int levelNumber;
    bool isCompleted = false;

    [Space(25)]
    [SerializeField] UnityEvent onLevelCompleted;
    [SerializeField] UnityEvent onLevelNotCompleted;



    void Awake()
    {
        //Controlla se e' gia' stato completato
        CheckCompleted();
    }

    void Update()
    {
        
    }

    void CheckCompleted()
    {
        //Prende l'evento rispetto a 
        //e lo attiva
        UnityEvent lvlEv = save_SO.GetCompletedLevels()[levelNumber - 1]
                             ? onLevelCompleted
                             : onLevelNotCompleted;

        lvlEv.Invoke();
    }

    public void CompleteLevel()
    {
        isCompleted = true;

        CheckCompleted();
    }


    public void LoadCompletedLevel(bool newIsCompleted)
    {
        //Cambia il livello completato
        //e attiva cio' che serve
        isCompleted = newIsCompleted;
            
        CheckCompleted();
    }

    public int GetLevelNum() => levelNumber;



    #region EXTRA - Cambiare l'inspector

    private void OnValidate()
    {
        //Limita il numero del livello
        //(sempre positivo)
        levelNumber = Mathf.Clamp(levelNumber, 0, levelNumber);
    }

    #endregion
}
