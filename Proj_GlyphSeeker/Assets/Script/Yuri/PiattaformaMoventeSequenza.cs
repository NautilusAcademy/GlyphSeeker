using UnityEngine;

public class PiattaformaMoventeSequenza : PiattaformaBase
{
    [SerializeField] private float velocitaMovimento = 5f;
    private GameObject playerOnPlatform;
    private CharacterController playerController;
    [SerializeField] private Transform startPoint, destinationPoint;

    void Update()
    {
        MuoviPiattaforma();
    }

    void MuoviPiattaforma()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationPoint.position, velocitaMovimento * Time.deltaTime);

        if (Vector3.Distance(transform.position, destinationPoint.position) < 0.01f)
        {
            transform.position = startPoint.position;
        }
    }
}
