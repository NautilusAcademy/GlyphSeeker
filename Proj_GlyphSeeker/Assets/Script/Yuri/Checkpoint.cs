using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpoints;
    public bool Activated = false;

    public static List<Checkpoint> CheckPointsList = new List<Checkpoint>();

    public static Vector3 GetActiveCheckPointPosition()
    {
        Vector3 result = new Vector3(0, 1, 0);

        // Cerchiamo il checkpoint attivato per ottenerne la posizione
        foreach (Checkpoint cp in CheckPointsList)
        {
            if (cp.Activated)
            {
                result = cp.transform.position;
                break;
            }
        }

        return result;
    }

    private void ActivateCheckPoint()
    {
        // Disattiviamo tutti i checkpoint nella lista
        foreach (Checkpoint cp in CheckPointsList)
        {
            cp.Activated = false;
        }

        // Attiviamo l'attuale checkpoint
        Activated = true;
    }

    void Start()
    {
        CheckPointsList.Add(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckPoint();
            checkpoints.Play();
        }
    }
}
