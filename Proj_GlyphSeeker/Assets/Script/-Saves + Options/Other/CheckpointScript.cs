using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckpointScript : MonoBehaviour
{
    [SerializeField] SaveSO_Script save_SO;
    [Space(10)]
    [SerializeField] bool isSingleUse = true;
    [SerializeField] Transform spawnPosition;
    Collider coll;



    private void Awake()
    {
        coll = GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        //IPlayer playerCheck = other.GetComponent<IPlayer>();

        if (other.CompareTag("Player"))//playerCheck != null)
        {
            SaveTempCheckpoint();
        }
    }

    void SaveTempCheckpoint()
    {
        //Imposta il nuovo checkpoint
        save_SO.SetTemp_CheckpointPos(spawnPosition.position);
        save_SO.SetTemp_CheckpointDir(spawnPosition.eulerAngles);


        //Se e' monouso, toglie il poter prenderlo di nuovo
        if (isSingleUse)
        {
            coll.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }



    #region EXTRA - Gizmo

    private void OnDrawGizmos()
    {
        //Disegna la posizione e direzione di spawn
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(spawnPosition.position, 0.15f);
        Gizmos.DrawRay(spawnPosition.position, spawnPosition.forward);
    }

    #endregion
}
