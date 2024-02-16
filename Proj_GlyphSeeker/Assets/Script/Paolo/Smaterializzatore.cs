using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Smaterializzatore : MonoBehaviour
{
    [SerializeField]
    private Transform raycastStartPoint;
    [SerializeField]
    private float maxRaycastDistance = 10f;
    [SerializeField]
    private float placeForce = 0f;
    [SerializeField]
    private float shootForce = 20f;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private float cooldown = 3f;
    private bool isObjectInSlot;
    private bool isCooldown = false;
    private GameObject hiddenObject;
    
    public GameObject ImageObjectCollected;
    public Image mirino;  
    
    private void Start()
    {
        
        isObjectInSlot = false;
        ImageObjectCollected.SetActive(false);
    }

    void Update()
    {
        // Memorizza l'oggetto colpito
        GameObject hitObject = null;

        // Lanciare un raycast in avanti solo se l'oggetto nascosto è null
        RaycastHit hit;
        // Aggiunto maxRaycastDistance al raycast
        if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, maxRaycastDistance)
            &&
            hiddenObject == null)
        {
            hitObject = hit.transform.gameObject;
        }
        

        if (hiddenObject!=null)
        {
            mirino.color = Color.green;
        } 
        else if(hit.collider!=null)
        {                
            if (hit.transform.GetComponent<PickUp>())
            {
                mirino.color = Color.magenta;
            }
            else
            {
                mirino.color = Color.white;
            }
        } 

        // Input per sparare
        if (GameManager.inst.inputManager.Player.Fire.triggered)
        {
            // Se c'è un oggetto nascosto, spara senza dover colpire nulla con il raycast
            if (hiddenObject != null && !isCooldown)
            {
                ShootObject(shootForce);
                StartCoroutine(ActivateCooldown());
            }
            // Se non c'è un oggetto nascosto, spara solo se il raycast ha colpito qualcosa
            else if (hitObject != null)
            {
                ShootObject(shootForce);
            }

            if (hiddenObject == null)
            {
                HideObject(hitObject);
            }
        }

        // Input per far scomparire l'oggetto
        if (GameManager.inst.inputManager.Player.Aim.triggered)
        {
            if (hiddenObject != null && !isCooldown)
            {
                PlaceObject(placeForce);
                StartCoroutine(ActivateCooldown());
            }
        }
    }

    IEnumerator ActivateCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }

    public void PlaceObject(float PlaceForce)
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
                    Vector3 safeDistanceCalculated = (raycastStartPoint.forward * hiddenObject.GetComponent<PickUp>().safeDistance);
                    
                    hitObject = goHit;
                    Destroy(hiddenObject.GetComponent<Smaterializzatore>());
                    hiddenObject.SetActive(true);
                    hiddenObject.transform.position = hit.point - safeDistanceCalculated + new Vector3(0, 0.5f, 0);

                    // Assegna null a hiddenObject per indicare che non c'è più un oggetto nascosto
                    hiddenObject = null;
                    isObjectInSlot = false;
                    // Assegna a false per far scomparire l'immagine
                    ImageObjectCollected.SetActive(false);
                }
            }
        }
    }

    public void ShootObject(float ShootForce)
    {
        if (hiddenObject != null)
        {           
            // Rimuovi il componente PlayerShooting dal clone per evitare duplicati
            Destroy(hiddenObject.GetComponent<Smaterializzatore>());

             // Attiva l'oggetto clonato
                hiddenObject.SetActive(true);
            // Sposta l'oggetto clonato nella posizione e rotazione del punto di sparo
            hiddenObject.transform.position = shootPoint.position;
            hiddenObject.transform.rotation = shootPoint.rotation;

            // Ottenere il componente Rigidbody dell'oggetto clonato
            Rigidbody projectileRb = hiddenObject.GetComponent<Rigidbody>();

            // Aggiungi una forza in avanti all'oggetto clonato
            if (projectileRb != null)
            {
                projectileRb.angularVelocity = Vector3.zero;
                projectileRb.velocity = Vector3.zero;
                projectileRb.AddForce(raycastStartPoint.transform.forward * ShootForce, ForceMode.Impulse);
            }

            // Assegna null a hiddenObject per indicare che non c'è più un oggetto nascosto
            hiddenObject = null;
            isObjectInSlot = false;

            //assegna a false per far scomparire l'immagine
            ImageObjectCollected.SetActive(false);
        }
    }

    void HideObject(GameObject objToHide)
    {
        if (objToHide != null && objToHide.GetComponent<PickUp>() && !isCooldown)
        {     
            if(objToHide==GameObject.Find("Barile"))
            {       
                if(objToHide.GetComponent<PickUp>().canPickUp)
                {
                  objToHide.GetComponent<Barile>().enabled = false;
                  objToHide.GetComponent<SmBarile>().enabled = true;
                    // Attiva lo sprite a schermo
                    ImageObjectCollected.SetActive(true);

                    objToHide.SetActive(false);
                    // Memorizza l'oggetto nascosto
                     hiddenObject = objToHide;

                    if (!objToHide.GetComponent<Rigidbody>())
                    {
                        objToHide.AddComponent<Rigidbody>();
                    }

                }                    
                return;
            }

            if(!objToHide.GetComponent<Rigidbody>())
            {
                objToHide.AddComponent<Rigidbody>();
            }
            // Disattiva l'oggetto colpito
           objToHide.SetActive(false);

            // Memorizza l'oggetto nascosto
            hiddenObject = objToHide;  
            
            // Attiva lo sprite a schermo
           ImageObjectCollected.SetActive(true);               
           
        }
        isObjectInSlot = true;
        
    }
}
