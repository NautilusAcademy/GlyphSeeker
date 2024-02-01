using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Smaterializzatore : MonoBehaviour
{
    public Transform raycastStartPoint;
    public float maxRaycastDistance = 10f;
    public float minProjectileForce = 0f;
    public float projectileForce = 20f;
    public Transform spawnPoint;
    private GameObject hiddenObject;
    public GameObject ImageObjectCollected;
    private GameObject hitObject;

    private bool isCooldown = false;
    private float cooldownDuration = 3f;

    private void Start()
    {
        ImageObjectCollected.SetActive(false);
    }

    void Update()
    {
        // Memorizza l'oggetto colpito
        GameObject hitObject = null;

        // Lanciare un raycast in avanti solo se l'oggetto nascosto è null
        if (hiddenObject == null)
        {
            RaycastHit hit;
            // Aggiunto maxRaycastDistance al raycast
            if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, maxRaycastDistance))
            {
                hitObject = hit.transform.gameObject;
            }
        }

        // Input per sparare
        if (Input.GetMouseButtonDown(0))
        {
            // Se c'è un oggetto nascosto, spara senza dover colpire nulla con il raycast
            if (hiddenObject != null && !isCooldown)
            {
                Shoot(projectileForce);
                StartCoroutine(Cooldown());
            }
            // Se non c'è un oggetto nascosto, spara solo se il raycast ha colpito qualcosa
            else if (hitObject != null)
            {
                Shoot(projectileForce);
            }

            if (hiddenObject == null)
            {
                HideObject(hitObject);
            }
        }

        // Input per far scomparire l'oggetto
        if (Input.GetMouseButtonDown(1))
        {
            if (hiddenObject != null && !isCooldown)
            {
                PlaceObject(minProjectileForce);
                StartCoroutine(Cooldown());
            }
        }
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
    }

    void PlaceObject(float minProjectileForce)
    {
        // Memorizza l'oggetto colpito
        GameObject hitObject = null;

        // Lanciare un raycast in avanti solo se l'oggetto nascosto è null
        if (hiddenObject != null)
        {
            RaycastHit hit;

            // Aggiunto maxRaycastDistance al raycast
            if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, maxRaycastDistance))
            {
                GameObject goHit = hit.transform.gameObject;
                Debug.Log(hit.transform.name);

                // Verifica se l'oggetto colpito è diverso dal giocatore
                if (goHit != null && !goHit.CompareTag("Player"))
                {
                    hitObject = goHit;
                    Destroy(hiddenObject.GetComponent<Smaterializzatore>());
                    hiddenObject.SetActive(true);
                    hiddenObject.transform.position = hit.point + new Vector3(0, 0.5f, 0);

                    // Assegna null a hiddenObject per indicare che non c'è più un oggetto nascosto
                    hiddenObject = null;

                    // Assegna a false per far scomparire l'immagine
                    ImageObjectCollected.SetActive(false);
                }
            }
        }
    }

    void Shoot(float projectileForce)
    {
        if (hiddenObject != null)
        {
            // Rimuovi il componente PlayerShooting dal clone per evitare duplicati
            Destroy(hiddenObject.GetComponent<Smaterializzatore>());

            // Attiva l'oggetto clonato
            hiddenObject.SetActive(true);

            // Sposta l'oggetto clonato nella posizione e rotazione del punto di sparo
            hiddenObject.transform.position = spawnPoint.position;
            hiddenObject.transform.rotation = spawnPoint.rotation;

            // Ottenere il componente Rigidbody dell'oggetto clonato
            Rigidbody projectileRb = hiddenObject.GetComponent<Rigidbody>();

            // Aggiungi una forza in avanti all'oggetto clonato
            if (projectileRb != null)
            {
                projectileRb.angularVelocity = Vector3.zero;
                projectileRb.velocity = Vector3.zero;
                projectileRb.AddForce(raycastStartPoint.transform.forward * projectileForce, ForceMode.Impulse);
            }

            // Assegna null a hiddenObject per indicare che non c'è più un oggetto nascosto
            hiddenObject = null;

            //assegna a false per far scomparire l'immagine
            ImageObjectCollected.SetActive(false);
        }
    }

    void HideObject(GameObject objToHide)
    {
        if (objToHide != null && objToHide.CompareTag("toHide") && !isCooldown)
        {
            // Disattiva l'oggetto colpito
            objToHide.SetActive(false);

            // Attiva lo sprite a schermo
            ImageObjectCollected.SetActive(true);

            // Memorizza l'oggetto nascosto
            hiddenObject = objToHide;
        }
    }
}
