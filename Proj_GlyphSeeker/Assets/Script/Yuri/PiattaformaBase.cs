using UnityEngine;

public class PiattaformaBase : MonoBehaviour
{
    private GameObject playerRigidbody;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRigidbody = other.gameObject;
            Rigidbody playerController = playerRigidbody.GetComponent<Rigidbody>();

            if (playerController != null)
            {
                // Rendi il giocatore un figlio della piattaforma
                playerRigidbody.transform.parent = transform;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Interrompe la parentela del giocatore con la piattaforma
            playerRigidbody.transform.parent = null;
        }
    }
}
