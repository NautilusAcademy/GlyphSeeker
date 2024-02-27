using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHeal : ObjectToDestroy
{
    [Header("—— Percentuali ——")]
    [Range(0, 1)]
    [SerializeField] float dropHealPercent;

    [Space(5)]
    [Range(0, 1)]
    [SerializeField] float dropPercent_small;
    [Range(0, 1)]
    [SerializeField] float dropPercent_medium;
    [Range(0, 1)]
    [SerializeField] float dropPercent_large;

    [Space(20)]
    [SerializeField] GameObject dropPrefab_small;
    [SerializeField] GameObject dropPrefab_medium;
    [SerializeField] GameObject dropPrefab_large;



    public override void DestroyObject(bool a)
    {
        base.DestroyObject(a);

        Drop();
    }

    public void Drop()
    {
        float randomPerc = Random.value;    //Prende una percentuale a caso

        if (randomPerc <= dropHealPercent)
        {
            randomPerc = Random.value;    //Prende una percentuale a caso

            //Controllo cura Small
            CheckPercentage(dropPercent_small, dropPrefab_small);
    
            //Controllo cura Medium
            CheckPercentage(dropPercent_medium, dropPrefab_small);

            //Controllo cura Large
            CheckPercentage(dropPercent_large, dropPrefab_large);

        }
        else
        {
            return;
        }


        void CheckPercentage(float percentMin, GameObject objToDrop)
        {
            //Check se la percentuale uscita rientra
            //tra quella minima passata
            if(randomPerc <= percentMin)
            {
                //Rilascia la cura
                Instantiate(objToDrop, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnValidate()
    {
        dropPercent_small = Mathf.Clamp(dropPercent_small, 0, dropPercent_medium);
        dropPercent_medium = Mathf.Clamp(dropPercent_medium, dropPercent_small, dropPercent_large);
        dropPercent_large = Mathf.Clamp(dropPercent_large, dropPercent_medium, 1);
    }
}
