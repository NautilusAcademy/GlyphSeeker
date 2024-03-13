using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : ObjectToDestroy
{
    [Header("—— Percentuali ——")]
    [Range(0, 1)]
    [SerializeField] float healPercent_S;
    [Range(0, 1)]
    [SerializeField] float healPercent_M;
    [Range(0, 1)]
    [SerializeField] float healPercent_L;

    [Space(10), Range(0, 1)]
    [SerializeField] float coinPercent;
    [Space(10), Range(0, 1)]
    [SerializeField] float enemyPercent = 0.01f;


    [Header("—— Prefab ——")]
    [SerializeField] GameObject healPrefab_S;
    [SerializeField] GameObject healPrefab_M;
    [SerializeField] GameObject healPrefab_L;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject enemyPrefab;

    List<GameObject> healsDropped = new List<GameObject>(),
                     spawnList = new List<GameObject>();




    public override void DestroyObject(bool a)
    {
        base.DestroyObject(a);

        Drop();
    }


    #region Drop-are gli oggetti

    [ContextMenu("–Droppa oggetto–")]
    public void Drop()
    {
        float randomPerc = Random.value;    //Prende una percentuale a caso

        spawnList.Clear();


        #region Drop - Cure

        //Controllo per le cure (in ordine: S, M, L)
        CheckHeal(randomPerc, healPercent_S, healPrefab_S);
        CheckHeal(randomPerc, healPercent_M, healPrefab_M);
        CheckHeal(randomPerc, healPercent_L, healPrefab_L);

        if (healsDropped.Count > 0)
        {
            //Aggiunge casualmente una delle cure uscite
            //(solo se ne sono uscite)
            AddDropToSpawnList(healsDropped[Random.Range(0, healsDropped.Count)]);
        }

        #endregion


        #region Drop Restante

        //Rilascia una moneta se è uscita nella percentuale
        if (randomPerc <= coinPercent)
        {
            AddDropToSpawnList(coinPrefab);
        }

        //Toglie tutti i drop
        //e rilascia un nemico se è uscito nella percentuale
        if (randomPerc <= enemyPercent)
        {
            spawnList.Clear();

            AddDropToSpawnList(enemyPrefab);
        }

        #endregion


        //Rilascia tutto il loot
        foreach (GameObject drop in spawnList)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }


        //Toglie l'oggetto distrutto dalla scena
        gameObject.SetActive(false);
    }



    void CheckHeal(float perc, float dropPercent, GameObject objToDrop)
    {
        if (perc <= dropPercent)
        {
            //Aggiunge la cura solo se risulta uscita
            healsDropped.Add(objToDrop);
        }
    }

    void AddDropToSpawnList(GameObject objToDrop)
    {
        //Aggiunge l'oggetto passato alla lista da creare
        spawnList.Add(objToDrop);
    }

    #endregion
}
