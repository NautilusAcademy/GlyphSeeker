using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHeal : ObjectToDestroy
{
    [Range(0, 1)]
    [SerializeField] float dropPercent_small,
                           dropPercent_medium,
                           dropPercent_large;

    [Space(10)]
    [SerializeField] GameObject dropPrefab_small;
    [SerializeField] GameObject dropPrefab_medium;
    [SerializeField] GameObject dropPrefab_large;
    


    public void Drop()
    {
        float randomPerc = Random.value;    //Prende una percentuale a caso

    
        //Controllo cura Small
        CheckPercentage(dropPercent_small, dropPrefab_small);
    
        //Controllo cura Medium
        CheckPercentage(dropPercent_medium, dropPrefab_small);

        //Controllo cura Large
        CheckPercentage(dropPercent_large, dropPrefab_large);
     


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
}
