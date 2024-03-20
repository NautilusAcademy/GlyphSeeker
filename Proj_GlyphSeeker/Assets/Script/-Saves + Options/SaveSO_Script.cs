using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Save (S.O.)", fileName = "Save_SO")]
public class SaveSO_Script : ScriptableObject
{
    [SerializeField] string fileName = "utility";
    [SerializeField] bool useEncryption = true;

    [Space(20), Header("—— Save File ——")]
    [SerializeField] int currentScene;
    [SerializeField] int currentLevel;
    [SerializeField] Vector3 checkpointPos,
                             checkpointDir;
    [SerializeField] float currentPlayerHealth,
                           maxPlayerHealth;

    [Space(10)]
    [SerializeField] float purpleRuneFireRate;
    [SerializeField] int currentElectricAmmo,
                         maxElectricAmmo;
    [SerializeField] int currentExplosiveAmmo,
                         maxExplosiveAmmo;
    [SerializeField] int currentShieldAmmo,
                         maxShieldAmmo;

    [Space(10)]
    [SerializeField] List<bool> unlockedRunes;
    [SerializeField] List<bool> completedLevels,
                                completedScenes;
    [SerializeField] Dictionary<string, bool> unlockedCollectibles = new Dictionary<string, bool>();

    [Space(20), Header("—— Auto Save ——")]
    [SerializeField] Vector3 temp_checkpointPos;
    [SerializeField] Vector3 temp_checkpointDir;




    #region Funz. Get personalizzate

    public string GetFileName() => fileName;
    public bool GetUseEncryption() => useEncryption;


        #region File di salvataggio

    public int GetCurrentLevel() => currentLevel;
    public float GetCurrentHealth() => currentPlayerHealth;
    public float GetMaxHealth() => maxPlayerHealth;

    public string GetRuneName(int runeIndex)
    {
        switch(runeIndex)
        {
            default:
            case 0:
                return "Electric";

            case 1:
                return "Explosive";
                
            case 2:
                return "Shield";
        };
    }

    public int GetCurrentAmmo(int runeIndex)
    {
        switch(runeIndex)
        {
            default:
            case 0:
                return currentElectricAmmo;

            case 1:
                return currentExplosiveAmmo;
                
            case 2:
                return currentShieldAmmo;
        };
    }

    public int GetMaxAmmo(int runeIndex)
    {
        switch(runeIndex)
        {
            default:
            case 0:
                return maxElectricAmmo;

            case 1:
                return maxExplosiveAmmo;
                
            case 2:
                return maxShieldAmmo;
        };
    }

    public float GetPurpleRuneFireRate() => purpleRuneFireRate;

    public List<bool> GetUnlockedRunes() => unlockedRunes;
    public List<bool> GetCompletedLevels() => completedLevels;
    public List<bool> GetCompletedScenes() => completedScenes;
    public Dictionary<string, bool> GetUnlockedCollectibles() => unlockedCollectibles;

    #endregion


        #region Funzioni piu' specifiche

    public bool FindUnlockedCollectible(string id)
    {
        if (unlockedCollectibles.ContainsKey(id))
        {
            return unlockedCollectibles[id];
        }
        else
        {
            Debug.LogError($"Collezionabile ({id}) non trovato nell'insieme");

            return default;
        }
    }

        #endregion


        #region Salv. automatico

    public Vector3 GetTemp_CheckpointPos() => temp_checkpointPos;

    public Vector3 GetTemp_CheckpointDir() => temp_checkpointDir;

        #endregion
    
    #endregion


    #region Funz. Set personalizzate

    public void SetTemp_CheckpointPos(Vector3 newPos)
    {
        temp_checkpointPos = newPos;
    }

    public void SetTemp_CheckpointDir(Vector3 newDir)
    {
        temp_checkpointDir = newDir;
    }


        #region Funz. per cambiare una variabile tra multiple

    public void ChangeUnlockedRune(int index, bool newIsUnlocked)
    {
        unlockedRunes[index] = newIsUnlocked;
    }

    public void ChangeCompletedLevelAtIndex(int index, bool newIsCompleted)
    {
        completedLevels[index] = newIsCompleted;
    }
    public void ChangeCompletedSceneAtIndex(int index, bool newIsCompleted)
    {
        completedScenes[index] = newIsCompleted;
    }

    public void ChangeUnlockedCollectibleValue(string keyToFind, bool newIsCollected)
    {
        if(unlockedCollectibles.ContainsKey(keyToFind))
        {
            //Cambia il valore se la chiave esiste gia'...
            unlockedCollectibles[keyToFind] = newIsCollected;
        }
        else
        {
            //...se no, ne aggiunge uno nuovo alla lista
            unlockedCollectibles.Add(keyToFind, newIsCollected);
        }
    }

        #endregion

    #endregion



    #region Funz. Load personalizzate


        #region -- Livello e Scena + Giocatore --

    public void LoadLevel(int level)
    {
        currentLevel = level;
    }

    public void LoadScene(int scene)
    {
        currentScene = scene;
    }

    public void LoadCheckpointPos(Vector3 newPos)
    {
        checkpointPos = newPos;
    }

    public void LoadCheckpointDir(Vector3 newDir)
    {
        checkpointDir = newDir;
    }

    public void LoadCurrentHealth(float newCurrentHp)
    {
        currentPlayerHealth = newCurrentHp;
    }

    public void LoadMaxHealth(float newMaxHp)
    {
        maxPlayerHealth = newMaxHp;
    }

        #endregion


        #region -- Rune --

    public void LoadCurrentAmmo(int runeIndex, int newAmmo)
    {
        switch (runeIndex)
        {
            case 0:
                currentElectricAmmo = newAmmo;
                break;

            case 1:
                currentExplosiveAmmo = newAmmo;
                break;

            case 2:
                currentShieldAmmo = newAmmo;
                break;
        }
    }

    public void LoadMaxAmmo(int runeIndex, int newAmmo)
    {
        switch (runeIndex)
        {
            case 0:
                maxElectricAmmo = newAmmo;
                break;

            case 1:
                maxExplosiveAmmo = newAmmo;
                break;

            case 2:
                maxShieldAmmo = newAmmo;
                break;
        }
    }

    public void LoadFireRate(float newFireRate)
    {
        purpleRuneFireRate = newFireRate;
    }

    public void LoadUnlockedRune(bool isUnlocked)
    {
        unlockedRunes.Add(isUnlocked);
    }

        #endregion


        #region -- Completati e Sbloccati --

    public void LoadCompletedLevel(bool isCompleted)
    {
        completedLevels.Add(isCompleted);
    }

    public void LoadCompletedScene(bool isCompleted)
    {
        completedScenes.Add(isCompleted);
    }

    public void LoadUnlockedCollectibles(string[] keysAndValues)
    {
        ReadDictionary(keysAndValues);
    }

        #endregion


    #endregion



    #region Funz. Clear personalizzate

    public void ClearUnlockedRunes()
    {
        unlockedRunes.Clear();
    }


    public void ClearCompletedLevels()
    {
        completedLevels.Clear();
    }

    public void ClearCompletedScenes()
    {
        completedScenes.Clear();
    }

    public void ClearUnlockedCollectible()
    {
        unlockedCollectibles.Clear();
    }

    #endregion



    #region Funzioni di utilita'

    /// <summary>
    /// Usando il metodo "string.Join('\n', Dictionary);" per scriverlo,
    /// <br></br>questo metodo legge e modifica un Dictionary senza problemi
    /// </summary>
    /// <param name="keysAndValues">Un'array di stringhe contenenti tutti i key e value da leggere,
    ///                             <br></br>di solito sono scritti "[key, value]"</param>
    void ReadDictionary(string[] keysAndValues)
    {
        //Cancella tutti i valori vecchi del Dictionary
        unlockedCollectibles.Clear();

        //Cerca la chiave e il valore di ognuno
        //e li mette nel Dictionary (ora vuoto)
        foreach (string s in keysAndValues)
        {
            //Trasforma "[key, value]" --> "key,value"
            string finale = s.TrimStart('[').Trim(' ').TrimEnd(']');

            //Li divide in un'array
            //contenente solo due valori: [key, value]
            string[] key_value = finale.Split(',');

            //In cui...
            string newKey = key_value[0];               //indice [0] è la key
            bool newValue = bool.Parse(key_value[1]);   //indice [1] è il value

            //Li aggiunge uno per uno nel Dictionary
            unlockedCollectibles.Add(newKey, newValue);
        }
    }

    #endregion
}
