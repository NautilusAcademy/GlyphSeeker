using UnityEngine;

public class PiattaformaMoventeSequenza : MonoBehaviour
{
    [SerializeField] private float velocitaMovimento = 5f;
    private GameObject playerOnPlatform;
    private CharacterController playerController;
    [SerializeField] private Transform startPoint, destinationPoint;

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
        transform.position = Vector3.MoveTowards(transform.position, destinationPoint.position, velocitaMovimento * Time.deltaTime);

        if (Vector3.Distance(transform.position, destinationPoint.position) < 0.01f)
        {
            transform.position = startPoint.position;
        }
    }

    void MuoviGiocatoreConPiattaforma()
    {
        if (playerController != null)
        {
            // Calcola il movimento relativo della piattaforma
            Vector3 deltaMovement = destinationPoint.position - startPoint.position;

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
