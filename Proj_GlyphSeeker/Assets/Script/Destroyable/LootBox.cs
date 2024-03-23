using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LootBox : ObjectToDestroy
{
    [Header("—— Percentuali ——")]
    [Range(0, 1), SerializeField] float healPercent_S = 0.15f;
    [Range(0, 1), SerializeField] float healPercent_M = 0.35f;
    [Range(0, 1), SerializeField] float healPercent_L = 0.45f;

    [Space(10)]
    [Range(0, 1), SerializeField] float coinPercent = 0.75f;
    [Space(10)]
    [Range(0, 1), SerializeField] float enemyPercent = 0.85f;


    [Space(20), Header("—— Prefab ——")]
    [SerializeField] GameObject healPrefab_S;
    [SerializeField] GameObject healPrefab_M;
    [SerializeField] GameObject healPrefab_L;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject enemyPrefab;




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

        GameObject loot;


        #region Check per ogni Drop
        
        //---Cura S---//
        if (randomPerc <= healPercent_S)
        {
            loot = healPrefab_S;
        }
        //---Cura M---//
        else if (randomPerc <= healPercent_M)
        {
            loot = healPrefab_M;
        }
        //---Cura L---//
        else if (randomPerc <= healPercent_L)
        {
            loot = healPrefab_L;
        }
        //---Moneta---//
        else if (randomPerc <= coinPercent)
        {
            loot = coinPrefab;
        }
        //---Nemico---//
        else if (randomPerc <= enemyPercent)
        {
            loot = enemyPrefab;
        }
        else
        {
            //Se non e' uscito nulla...
            loot = null;
        }

        #endregion


        //Rilascia il loot
        Instantiate(loot, transform.position, Quaternion.identity);


        //Toglie l'oggetto distrutto dal giocatore dalla scena
        gameObject.SetActive(false);
    }

    #endregion



    #region EXTRA - Cambiare l'Inspector

    private void OnValidate()
    {
        //Limita il range di ogni percentuale per non farle sovrapporre
        enemyPercent = Mathf.Clamp(enemyPercent, coinPercent, 1);
        coinPercent = Mathf.Clamp(coinPercent, healPercent_L, enemyPercent);
        healPercent_L = Mathf.Clamp(healPercent_L, healPercent_M, coinPercent);
        healPercent_M = Mathf.Clamp(healPercent_M, healPercent_S, healPercent_L);
        healPercent_S = Mathf.Clamp(healPercent_S, 0, healPercent_M);
    }

    [ContextMenu("–Reset solo le percentuali–")]
    void ResetPercentOnly()
    {
        healPercent_S = 0.15f;
        healPercent_M = 0.35f;
        healPercent_L = 0.45f;

        coinPercent = 0.75f;

        enemyPercent = 0.85f;
    }

    #endregion


    #region EXTRA - Editor

    public List<float> Editor_GetAllPercents()
    {
        return new List<float>()
        {
            healPercent_S,
            healPercent_M,
            healPercent_L,
            coinPercent,
            enemyPercent
        };
    }

    #endregion
}
