using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaRetrattile : MonoBehaviour
{
    [SerializeField] private float velocitaMovimento = 5f;
    [SerializeField] private float pausa = 3f;
    [SerializeField] private float intervalloInversioneDirezione = 2f;
    bool stop;
    private float timerAttesa;
    private float timerInversioneDirezione;

    private GameObject playerOnPlatform;  // Riferimento al giocatore sulla piattaforma

    void Start()
    {
        timerAttesa = pausa;
        timerInversioneDirezione = intervalloInversioneDirezione;
    }

    void Update()
    {
        if (!stop)
        {
            MuoviPiattaforma();

            timerInversioneDirezione -= Time.deltaTime;
        }
        

        if (timerInversioneDirezione <= 0f)
        {
            stop = true;
            timerAttesa -= Time.deltaTime;
            if (timerAttesa <= 0f)
            {
                stop = false;
                InvertiDirezione();
                timerInversioneDirezione = intervalloInversioneDirezione;
            }
            
        }

        if (playerOnPlatform != null)
        {
            MuoviGiocatoreConPiattaforma();
        }
    }

    void MuoviPiattaforma()
    {
        float movimento = velocitaMovimento * Time.deltaTime;
        transform.Translate(new Vector3(movimento, 0f, 0f));
    }

    void InvertiDirezione()
    {
        velocitaMovimento = -velocitaMovimento;
        timerAttesa = pausa;
    }

    void MuoviGiocatoreConPiattaforma()
    {
        Rigidbody playerRigidbody = playerOnPlatform.GetComponent<Rigidbody>();

        if (playerRigidbody != null)
        {
            playerRigidbody.MovePosition(playerRigidbody.position + new Vector3(velocitaMovimento * Time.deltaTime, 0f, 0f));
        }
        else
        {
            CharacterController playerController = playerOnPlatform.GetComponent<CharacterController>();

            if (playerController != null)
            {
                playerController.Move(new Vector3(velocitaMovimento * Time.deltaTime, 0f, 0f));
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = other.gameObject;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = null;
        }
    }
}
