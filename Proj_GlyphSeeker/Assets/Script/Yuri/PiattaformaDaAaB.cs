using UnityEngine;

public class PiattaformaDaAaB : MonoBehaviour
{
    [SerializeField] private float velocitaMovimento = 5f;
    private GameObject playerOnPlatform;
    private CharacterController playerController;
    [SerializeField] private Transform[] percorso; // Array di punti nel percorso
    private int indicePuntoCorrente = 0;

    void Update()
    {
        MuoviPiattaforma();

        if (playerOnPlatform != null)
        {
            MuoviGiocatoreConPiattaforma();
        }
    }

    void MuoviPiattaforma()
    {
        if (percorso.Length == 0)
        {
            Debug.LogError("Il percorso non contiene punti.");
            return;
        }

        // Muovi verso il punto corrente nel percorso
        transform.position = Vector3.MoveTowards(transform.position, percorso[indicePuntoCorrente].position, velocitaMovimento * Time.deltaTime);

        if (Vector3.Distance(transform.position, percorso[indicePuntoCorrente].position) < 0.01f)
        {
            // Se la piattaforma ha raggiunto il punto corrente, passa al successivo
            indicePuntoCorrente = (indicePuntoCorrente + 1) % percorso.Length;
        }
    }

    void MuoviGiocatoreConPiattaforma()
    {
        if (playerController != null)
        {
            // Calcola il movimento relativo della piattaforma
            Vector3 deltaMovement = percorso[indicePuntoCorrente].position - transform.position;

            // Muovi il giocatore con la piattaforma
            playerController.Move(deltaMovement * velocitaMovimento * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = other.gameObject;
            Rigidbody playerController = playerOnPlatform.GetComponent<Rigidbody>();

            if (playerController != null)
            {
                // Rendi il giocatore un figlio della piattaforma
                playerOnPlatform.transform.parent = transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Rimuovi il giocatore come figlio della piattaforma
            playerOnPlatform.transform.parent = null;

            playerOnPlatform = null;
            playerController = null;
        }
    }
}

