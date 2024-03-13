using UnityEngine;

public class PiattaformaSemovente : MonoBehaviour
{
    [SerializeField] private float velocitaMovimento = 5f;
    [SerializeField] private float intervalloInversioneDirezione = 2f;

    private float timerInversioneDirezione;

    private GameObject playerOnPlatform;  // Riferimento al giocatore sulla piattaforma

    void Start()
    {
        timerInversioneDirezione = intervalloInversioneDirezione;
    }

    void Update()
    {
        MuoviPiattaforma();

        timerInversioneDirezione -= Time.deltaTime;

        if (timerInversioneDirezione <= 0f)
        {
            InvertiDirezione();
            timerInversioneDirezione = intervalloInversioneDirezione;
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
