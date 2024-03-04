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
    private bool canShootObj ;
    
    private GameObject hiddenObject;
    private float objSafeDistance;

    [SerializeField] GameObject phantomObj;
    [SerializeField] Material phantomMat;
    bool matMessi_DASISTEMARE = false;
    
    public GameObject ImageObjectCollected;
    public Image mirino;
    public AudioSource errore;
    public AudioSource colpo;

    private void Start()
    {
        canShootObj = true;
        isObjectInSlot = false;
        ImageObjectCollected.SetActive(false);
    }

    void Update()
    {
        // Memorizza l'oggetto colpito
        GameObject hitObject = null;

        // Lanciare un raycast in avanti solo se l'oggetto nascosto � null
        RaycastHit hit;
        // Aggiunto maxRaycastDistance al raycast
        if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, maxRaycastDistance) && hiddenObject == null)
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
        else
        {
            mirino.color = Color.white;
        }

        // Input per sparare
        if (GameManager.inst.inputManager.Player.Fire.triggered)
        {
            // Se c'� un oggetto nascosto, spara senza dover colpire nulla con il raycast
            if (hiddenObject != null && canShootObj)
            {
                ShootObject(shootForce);
                StartCoroutine(ActivateCooldown());
            }
            // Se non c'� un oggetto nascosto, spara solo se il raycast ha colpito qualcosa
            else if (hitObject != null)
            {
                ShootObject(shootForce);
            }

            if (hiddenObject == null)
            {
                HideObject(hitObject);
            }
            else
            {
                errore.Play();
            }
        }


        // Input per far vedere dove piazzare l'oggetto
        if (GameManager.inst.inputManager.Player.Aim.ReadValue<float>()>0)
        {
            if (hiddenObject != null)
            {
                canShootObj = false;
                ShowObject();
            }
        }
        else if (!canShootObj)
        {
            // Input per piazzare l'oggetto
            PlaceObject(placeForce);
            StartCoroutine(ActivateCooldown());

            // Toglie l'oggetto fantasma
            phantomObj.SetActive(false);
            matMessi_DASISTEMARE = false;
            phantomObj.GetComponent<MeshFilter>().mesh = null;
        }
    }




    IEnumerator ActivateCooldown()
    {
        canShootObj = false;
        yield return new WaitForSeconds(cooldown);
        canShootObj = true;
    }

    public void ShowObject()
    {
        if (GameManager.inst.inputManager.Player.Aim.triggered)
        {
            phantomObj.GetComponent<MeshFilter>().mesh = hiddenObject.GetComponent<MeshFilter>().mesh;            
        }
        
        phantomObj.SetActive(true);

        RaycastHit hit;

        if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, maxRaycastDistance))
        {
            phantomObj.transform.position = hit.point
                                            - (raycastStartPoint.forward * objSafeDistance)
                                            + new Vector3(0, 0.5f, 0);
        }
        else
        {
            phantomObj.transform.position = raycastStartPoint.position
                                            + raycastStartPoint.forward * maxRaycastDistance;

        }

        phantomObj.transform.rotation = hiddenObject.transform.rotation;
        phantomObj.transform.localScale = hiddenObject.transform.localScale;
    }

    public void PlaceObject(float PlaceForce)
    {      
        // Lanciare un raycast in avanti solo se l'oggetto nascosto � diverso null
        if (hiddenObject != null)
        {
            RaycastHit hit;

            // Aggiunto maxRaycastDistance al raycast
            if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, maxRaycastDistance))
            {
                GameObject goHit = hit.transform.gameObject;
                
                // Verifica se l'oggetto colpito � diverso dal giocatore
                if (goHit != null && !goHit.CompareTag("Player"))
                {
                    Vector3 safeDistanceCalculated = (raycastStartPoint.forward * objSafeDistance);
                                   
                    Destroy(hiddenObject.GetComponent<Smaterializzatore>());
                    hiddenObject.SetActive(true);
                    hiddenObject.transform.position = hit.point - safeDistanceCalculated + new Vector3(0, 0.5f, 0);
                    colpo.Play();
                    // Assegna null a hiddenObject per indicare che non c'� pi� un oggetto nascosto
                    hiddenObject = null;
                    isObjectInSlot = false;
                    // Assegna a false per far scomparire l'immagine
                    ImageObjectCollected.SetActive(false);
                }
               
            }
            else
            {
                Destroy(hiddenObject.GetComponent<Smaterializzatore>());
                hiddenObject.transform.position = raycastStartPoint.position + raycastStartPoint.forward * maxRaycastDistance ;
                hiddenObject.SetActive(true);
                colpo.Play();
                // Assegna null a hiddenObject per indicare che non c'� pi� un oggetto nascosto
                hiddenObject = null;
                isObjectInSlot = false;
                // Assegna a false per far scomparire l'immagine
                ImageObjectCollected.SetActive(false);
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

            // Assegna null a hiddenObject per indicare che non c'� pi� un oggetto nascosto
            hiddenObject = null;
            isObjectInSlot = false;
            print(isObjectInSlot);
            colpo.Play();
            //assegna a false per far scomparire l'immagine
            ImageObjectCollected.SetActive(false);
        }
    }

    void HideObject(GameObject objToHide)
    {
        if (objToHide != null && objToHide.GetComponent<PickUp>() && canShootObj)
        {     
            if(objToHide==GameObject.Find("Barile"))
            {       
                if(objToHide.GetComponent<PickUp>().canPickUp)
                {
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

            //attiva il suono
            colpo.Play();

            // Disattiva l'oggetto colpito
           objToHide.SetActive(false);

           // Memorizza l'oggetto nascosto e la distanza
           hiddenObject = objToHide;
            objSafeDistance = objToHide.GetComponent<PickUp>().safeDistance;
            
           // Attiva lo sprite a schermo
           ImageObjectCollected.SetActive(true);            
           
        }
        isObjectInSlot = true;
        
    }
}
