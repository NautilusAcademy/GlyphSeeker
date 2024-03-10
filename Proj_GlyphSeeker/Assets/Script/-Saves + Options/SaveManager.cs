using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [Header("—— Variabili delle informazioni ——")]
    [SerializeField] SaveSO_Script save_SO;
    [SerializeField] OptionsSO_Script opt_SO;
    [SerializeField] GameObject playerObj;

    string file_path;

    #region Tooltip()
    [Tooltip("Una chiave a stringa che serve per criptare un'altra stringa \ne de-criptarla per riportarla alla sua forma originale")]
    #endregion
    private readonly string encryptionKey = "l5WbyRgKCX";

    const string CURRENT_LEVEL_TITLE = "# CURRENT LEVEL #",
                 PLAYER_TITLE = "# PLAYER #",
                 RUNES_TITLE = "# RUNES #",
                 UNLOCKED_RUNES_TITLE = "# UNLOCKED RUNES #",
                 COMPLETED_LEVELS_TITLE = "# COMPLETED LEVELS #",
                 COMPLETED_SCENES_TITLE = "# COMPLETED SCENES #",
                 COLLECTIBLES_TITLE = "# COLLECTIBLES #",
                 OPTIONS_TITLE = "# OPTIONS #";




    private void Awake()
    {
        //Prende il percorso dell'applicazione
        file_path = Path.Combine(Application.dataPath, save_SO.GetFileName() + ".save");
    }


    public void SaveGame()
    {
        string saveString = "";


        #region -- Livello e Scena attuale --

        saveString += CURRENT_LEVEL_TITLE + "\n";

        //Aggiunge dove si trova il giocatore (livello e scena attuale)
        saveString += SceneManager.GetActiveScene().buildIndex + "\n";
        saveString += save_SO.GetCurrentLevel() + "\n";

        #endregion


        #region -- Stats Giocatore --

        saveString += "\n" + PLAYER_TITLE + "\n";

        //Aggiunge la posizione e direzione dell'ultimo checkpoint
        saveString += save_SO.GetTemp_CheckpointPos().x + "\n";
        saveString += save_SO.GetTemp_CheckpointPos().y + "\n";
        saveString += save_SO.GetTemp_CheckpointPos().z + "\n";
        saveString += save_SO.GetTemp_CheckpointDir().x + "\n";
        saveString += save_SO.GetTemp_CheckpointDir().y + "\n";
        saveString += save_SO.GetTemp_CheckpointDir().z + "\n";


        //Aggiunge la vita del giocatore (attuale e max)
        saveString += save_SO.GetCurrentHealth() + "\n";
        saveString += save_SO.GetMaxHealth() + "\n";

        #endregion


        #region -- Rune (stats) --

        saveString += "\n" + RUNES_TITLE + "\n";

        //Aggiunge le munizioni (attuali e max)
        //delle tre Rune (Elettrica, Esplosiva e Scudo)
        for (int i = 0; i < 3; i++)
        {
            saveString += save_SO.GetRuneName(i) + "\n";
            saveString += save_SO.GetCurrentAmmo(i) + "\n";
            saveString += save_SO.GetMaxAmmo(i) + "\n";
        }

        //Aggiunge la cadenza di tiro della Runa Viola
        saveString += save_SO.GetPurpleRuneFireRate() + "\n";

        #endregion


        #region -- Rune (raccolte) --

        saveString += "\n" + UNLOCKED_RUNES_TITLE + "\n";

        //Aggiunge il Count della lista
        saveString += "[" + save_SO.GetUnlockedRunes().Count + "]"+ "\n";

        //Aggiunge quali rune sono state sbloccate
        foreach (bool isCollected in save_SO.GetUnlockedRunes())
        {
            saveString += isCollected + "\n";
        }

        #endregion


        #region -- Livelli completati --

        saveString += "\n" + COMPLETED_LEVELS_TITLE + "\n";

        //Aggiunge il Count della lista
        saveString += "[" + save_SO.GetCompletedLevels().Count + "]" + "\n";

        //Aggiunge quali livelli sono stati completati
        foreach (bool isCompleted in save_SO.GetCompletedLevels())
        {
            saveString += isCompleted + "\n";
        }

        #endregion


        #region -- Scene completate --

        saveString += "\n" + COMPLETED_SCENES_TITLE + "\n";

        //Aggiunge il Count della lista
        saveString += "[" + save_SO.GetCompletedScenes().Count + "]" + "\n";

        //Aggiunge quali scene sono state completate
        foreach (bool isCompleted in save_SO.GetCompletedScenes())
        {
            saveString += isCompleted + "\n";
        }

        #endregion


        #region -- Collezionabili raccolti  --

        saveString += "\n" + COLLECTIBLES_TITLE + "\n";

        //Aggiunge quali collezionabili sono state sbloccati
        saveString += string.Join('\n', save_SO.GetUnlockedCollectibles()) + "\n";

        #endregion


        #region -- Opzioni --

        saveString += "\n" + OPTIONS_TITLE + "\n";

        //Aggiunge tutte le opzioni scelte
        saveString += opt_SO.GetMusicVolume_Percent() + "\n";
        saveString += opt_SO.GetSoundVolume_Percent() + "\n";
        saveString += opt_SO.GetSensitivity() + "\n";
        saveString += (int)opt_SO.GetRuneSelect() + "\n";

        #endregion


        if (save_SO.GetUseEncryption())
        {
            //Codifica il file
            //(Encryption)
            saveString = EncryptDecrypt(saveString);
        }


        //Sovrascrive il file
        //(se non esiste, ne crea uno nuovo e ci scrive)
        File.WriteAllText(file_path, saveString);


        #region Prodotto finale
        /*  0:  ### CURRENT LEVEL ###
         *  1:  Scena (attuale)
         *  2:  Livello (attuale)
         *  3:  
         *  4:  ### PLAYER ###
         *  5:  Checkpoint - Posizione X
         *  6:  Checkpoint - Posizione Y
         *  7:  Checkpoint - Posizione Z
         *  8:  Checkpoint - Direzione vista X
         *  9:  Checkpoint - Direzione vista Y
         * 10:  Checkpoint - Direzione vista Z
         * 11:  Vita Giocatore (attuale)
         * 12:  Vita Giocatore (max)
         * 13:  
         * 14:  ### RUNES ###
         * 15:  Nome (Elettrica)
         * 16:  - Munizioni attuali
         * 17:  - Munizioni max
         * 18:  Nome (Esplosiva)
         * 19:  - Munizioni attuali
         * 20:  - Munizioni max
         * 21:  Nome (Scudo)
         * 22:  - Munizioni attuali
         * 23:  - Munizioni max
         * 24:  Cadenza di fuoco runa Viola (Smaterializzatore)
         * 25:  
         * 26:  ### UNLOCKED RUNES ###
         * 27:  [Count lista]
         * 28:  Runa Gialla sbloccata
         * 29:  Runa Rossa sbloccata
         * 30:  Runa Blu sbloccata
         * 31:  Runa Viola sbloccata
         * 32:  
         *  …:  ### COMPLETED LEVELS ###
         * 42:  [Count lista]
         * 43:  Lista di livelli completati
         *  …:  
         * 53:  ### COMPLETED SCENES ###
         * 54:  [Count lista]
         *  …:  Lista di scene (di Unity) completate
         * 64:  
         * 65:  ### COLLECTIBLES ###
         * 66:  Collezionabili raccolti (Chiavi & Valori)
         * 67:  
         * 68:  ### OPTIONS ###
         * 69:  Volume musica
         * 70:  Volume effetti sonori
         * 71:  Sensibilita'
         * 72:  Modalita' Selezione Rune
         * 73:  
         * 74:  
         * 75:  
         */
        #endregion
    }


    public void LoadGame()
    {
        string[] fileReading;

        int i_currentLevel = 0,
            i_player = 0,
            i_runes = 0,
            i_unlockedRunes = 0,
            i_completedLevels = 0,
            i_completedScenes = 0,
            i_collectibles = 0,
            i_options = 0;


        //Se il file esiste...
        if (File.Exists(file_path))
        {
            //Legge il file di salvataggio
            string encriptedText = File.ReadAllText(file_path);


            if (save_SO.GetUseEncryption())
            {
                //Decodifica ogni stringa dell'array
                //(Decryption)
                encriptedText = EncryptDecrypt(encriptedText);
            }


            //Lo divide ogni "a capo"
            //e lo sposta nell'array da leggere
            fileReading = encriptedText.Split('\n');
        }
        else
        {
            string errorPath = $"<i>({file_path})</i>";

            Debug.LogError($"[!] Il File non esiste" + "\n"
                           + errorPath);
            return;
        }


        #region Trovare i punti di inizio

        //Cerca nell'array i punti di inizio delle varie "regioni"
        for (int i = 0; i < fileReading.Length; i++)
        {
            switch (fileReading[i])
            {
                case CURRENT_LEVEL_TITLE:
                    i_currentLevel = i;
                    break;

                case PLAYER_TITLE:
                    i_player = i;
                    break;

                case RUNES_TITLE:
                    i_runes = i;
                    break;

                case UNLOCKED_RUNES_TITLE:
                    i_unlockedRunes = i;
                    break;

                case COMPLETED_LEVELS_TITLE:
                    i_completedLevels = i;
                    break;

                case COMPLETED_SCENES_TITLE:
                    i_completedScenes = i;
                    break;

                case COLLECTIBLES_TITLE:
                    i_collectibles = i;
                    break;

                case OPTIONS_TITLE:
                    i_options = i;
                    break;
            }
        }

        #endregion



        #region -- Livello e Scena attuale --

        //Trasforma da string a int
        int level_load = int.Parse(fileReading[i_currentLevel + 1]),
            scene_load = int.Parse(fileReading[i_currentLevel + 2]);

        //Carica l'ultimo livello e scena del giocatore
        save_SO.LoadLevel(level_load);
        save_SO.LoadScene(scene_load);
        //save_SO.OpenChosenScene(level_load);

        #endregion


        #region -- Stats Giocatore --

        //Trasforma da string a float
        float posX_load = float.Parse(fileReading[i_player + 1]),
              posY_load = float.Parse(fileReading[i_player + 2]),
              posZ_load = float.Parse(fileReading[i_player + 3]),
              dirX_load = float.Parse(fileReading[i_player + 4]),
              dirY_load = float.Parse(fileReading[i_player + 5]),
              dirZ_load = float.Parse(fileReading[i_player + 6]),
              currentHealth_load = float.Parse(fileReading[i_player + 7]),
              maxHealth_load = float.Parse(fileReading[i_player + 8]);

        //Carica l'ultima posizione e direzione del giocatore
        //(l'ultimo checkpoint raggiunto)
        save_SO.LoadCheckpointPos(new Vector3(posX_load, posY_load, posZ_load));
        save_SO.LoadCheckpointDir(new Vector3(posX_load, posY_load, posZ_load));

        //Carica la vita del giocatore
        //(sia la massima, sia quella rimanente)
        save_SO.LoadCurrentHealth(currentHealth_load);
        save_SO.LoadMaxHealth(maxHealth_load);

        #endregion


        #region -- Rune (stats) --

        //Il punto d'inizio per leggere la Runa Viola 
        int i_purpleRune_fireRate = 0;

        //Carica le munizioni massime e quelle rimaste
        //delle tre Rune (Elettrica, Esplosiva e Scudo)
        for (int i = 0; i < 3; i++)
        {
            //Turns from string to int for each power-up's name
            int i_runeName = i_runes + (3 * i) + 1;
            
            /* 
             * 3 ---> quante linee deve saltare (per ogni sezione delle muniz. di runa)
             */

            //Prende i numeri corrispondenti
            int currentAmmo_load = int.Parse(fileReading[i_runeName + 1]),
                maxAmmo_load = int.Parse(fileReading[i_runeName + 2]);
            
            save_SO.LoadCurrentAmmo(i, currentAmmo_load);
            save_SO.LoadMaxAmmo(i, maxAmmo_load);


            //Prende la posizione dall'ultimo ciclo
            i_purpleRune_fireRate = i_runes + (i + 1) * 3 + 1;
        }


        //Trasforma da string a int
        float purple_fireRate_load = float.Parse(fileReading[i_purpleRune_fireRate]);

        //Carica la cadenza di tiro della Runa Viola
        save_SO.LoadFireRate(purple_fireRate_load);

        #endregion


        #region -- Rune (raccolte) --

        //Trova la quantita' della lista delle rune
        //e la prima posizione delle rune sbloccate
        int unlockedRune_count = ReadCountFromFile(fileReading[i_unlockedRunes + 1]),
            i_firstUnlockedRune = i_unlockedRunes + 2;

        save_SO.ClearUnlockedRunes();    //Cancella la vecchia lista

        //Carica le rune sbloccate
        for (int i = 0; i < unlockedRune_count; i++)
        {
            //Trova la string nella posizione
            //definita dal ciclo e la trasforma in bool
            bool isUnlocked_load = bool.Parse(fileReading[i_firstUnlockedRune + i]);

            save_SO.LoadUnlockedRune(isUnlocked_load);
        }

        #endregion


        #region -- Livelli completati --

        //Legge la quantita' dei livelli completati
        int complLevels_Count = ReadCountFromFile(fileReading[i_completedLevels + 1]);

        save_SO.ClearCompletedLevels();    //Cancella la vecchia lista

        //Carica quali livelli sono stati completati
        for (int i = 0; i < complLevels_Count; i++)
        {
            //Trasforma da string a bool
            bool isCompleted_load = bool.Parse(fileReading[i_completedLevels + i + 2]);
            
            /* 
             * +2 perche' salta la linea del Count
             */

            //Carica quali livelli sono stati completati
            save_SO.LoadCompletedLevel(isCompleted_load);
        }

        #endregion


        #region -- Scene completate --

        //Legge la quantita' delle scene completate
        int complScenes_Count = ReadCountFromFile(fileReading[i_completedScenes + 1]);

        save_SO.ClearCompletedScenes();    //Cancella la vecchia lista

        //Carica quali scene sono state completate
        for (int i = 0; i < complScenes_Count; i++)
        {
            //Trasforma da string a bool
            bool isCompleted_load = bool.Parse(fileReading[i_completedScenes + i + 2]);
            
            /* 
             * +2 perche' salta la linea del Count
             */

            //Carica quali scene sono state completate
            save_SO.LoadCompletedScene(isCompleted_load);
        }

        #endregion


        #region -- Collezionabili raccolti --

        List<string> allKeysValues = new List<string>();
                
        //Prende ogni valore del Dictionary
        //e lo aggiunge come elemento nella lista
        //for (int i = 0; i < i_options-2; i++)                      //--Prima della prox sezione----//
        for (int i = 0; i < save_SO.GetUnlockedCollectibles().Count; i++)  //--Quanti sono i [key, value]--//
        {
            //Trasforma da string a float
            allKeysValues.Add(fileReading[i_collectibles + i + 1]);
        }

        //Carica quali livelli sono stati completati
        save_SO.LoadUnlockedCollectibles(allKeysValues.ToArray());

        #endregion


        #region -- Opzioni --

        //Trasforma da string a float/bool
        float musicVol_load = float.Parse(fileReading[i_options + 1]),
              soundVol_load = float.Parse(fileReading[i_options + 2]),
              sensitivity_load = float.Parse(fileReading[i_options + 3]);
        int runeSelect_load = int.Parse(fileReading[i_options + 4]);

        //Load di tutte le opzioni
        opt_SO.ChangeMusicVolume(musicVol_load);
        opt_SO.ChangeSoundVolume(soundVol_load);
        opt_SO.ChangeSensitivity(sensitivity_load);
        opt_SO.ChangeRuneSelect(runeSelect_load);

        #endregion
    }


    public void GenerateNewGame()
    {
        //Cancella il file precedente
        DeleteSaveFile();

        //Salva in un nuovo file
        SaveGame();
    }

    public void DeleteSaveFile()
    {
        //Se già esiste, lo elimina
        if (File.Exists(file_path))
            File.Delete(file_path);
    }


    string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        int keyLenght = encryptionKey.Length;

        //Per ogni carattere della stringa...
        for (int i = 0; i < data.Length; i++)
        {
            //Cambia con un'operazione XOR, scambiando ogni carattere con un'altro
            //(operazione reversibile se si usa la stessa chiave/key)
            modifiedData += (char)(data[i] ^ encryptionKey[i % keyLenght]);

            /* 
             * Questo è il video in cui spiega come funziona l'operazione XOR
             * https://youtu.be/aUi9aijvpgs&t=1380s
             */
        }

        return modifiedData;
    }



    #region Funzioni utili

    /// <summary>
    /// Prende in lettura il Count ancora "sporco", scritto in: "[#]"        
    /// <br></br>("#" sarebbe il numero);
    /// togliendo "[" e "]"
    /// </summary>
    /// <returns>Il Count "pulito", come int</returns>
    int ReadCountFromFile(string dirtyCount)
    {
        //Prende in lettura il Count ancora "sporco", scritto in: "[#]"
        //<br></br>("#" sarebbe il numero);
        //togliendo "[" e "]"
        dirtyCount = dirtyCount.Trim('[').Trim(']');

        //Ritorna Il Count "pulito", come int
        return int.Parse(dirtyCount);
    }

    #endregion
}
