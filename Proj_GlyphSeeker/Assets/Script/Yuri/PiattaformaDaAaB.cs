using UnityEngine;

public class PiattaformaDaAaB : PiattaformaBase
{
    [SerializeField] private float velocitaMovimento = 5f;
    private GameObject playerOnPlatform;
    private CharacterController playerController;
    [SerializeField] private Transform[] percorso; // Array di punti nel percorso
    private int indicePuntoCorrente = 0;

    void Update()
    {
        MuoviPiattaforma();
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

    
}

