using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Smaterializzatore : PlayerShoot
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
    private float placeDist = 0.5f;
    [SerializeField]
    private float cooldown = 3f;
    private bool isObjectInSlot;

    RaycastHit purpleHit;

    private GameObject hiddenObject;
    private float objSafeDistance;

    [SerializeField] GameObject phantomObj;
    [SerializeField] Material phantomMat;
    MeshFilter phantomObj_mf;
    MeshRenderer phantomObj_mr;
    
    public GameObject ImageObjectCollected;
    public Image mirino;
    public AudioSource errore;
    public AudioSource colpo;

    private void Start()
    {
        canShoot = true;
        isObjectInSlot = false;

        phantomObj_mf = phantomObj.GetComponent<MeshFilter>();
        phantomObj_mr = phantomObj.GetComponent<MeshRenderer>();

        ImageObjectCollected.SetActive(false);
    }

    void Update()
    {
        // Memorizza l'oggetto colpito
        GameObject hitObject = null;

        // Lanciare un raycast in avanti solo se l'oggetto nascosto è null
        bool hasHit = CastRaycast(out purpleHit);

        // Aggiunto maxRaycastDistance al raycast
        if (hasHit && !isObjectInSlot)
        {
            hitObject = purpleHit.transform.gameObject;
        }



        // E' vero solo se ha un oggetto immagazzinato
        isObjectInSlot = hiddenObject != null;


        // ---Da sistemare in PlayerUIManager
        if (isObjectInSlot)
        {
            mirino.color = Color.green;
        } 
        else if(purpleHit.collider!=null)
        {                
            if (purpleHit.transform.GetComponent<PickUp>())
            {
                if(canShoot)
                    mirino.color = Color.magenta;
                else
                    mirino.color = Color.gray;
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
            // Se c'è un oggetto nascosto, spara senza dover colpire nulla con il raycast
            if (isObjectInSlot && canShoot)
            {
                ShootObject(shootForce);
                StartCoroutine(ActivateCooldown());
            }
            // Se non c'è un oggetto nascosto, spara solo se il raycast ha colpito qualcosa
            else if (hitObject != null)
            {
                ShootObject(shootForce);
            }

            if (!isObjectInSlot)
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
            if (isObjectInSlot)
            {
                canShoot = false;
                ShowObject();
            }
        }
        else if (isObjectInSlot && !canShoot)
        {
            // Input per piazzare l'oggetto
            PlaceObject(placeForce);
            StartCoroutine(ActivateCooldown());

            // Toglie l'oggetto fantasma
            phantomObj.SetActive(false);
            phantomObj_mf.mesh = null;
        }
    }


    bool CastRaycast(out RaycastHit hit)
    {
        return Physics.Raycast(raycastStartPoint.position,
                               raycastStartPoint.forward,
                               out hit,
                               maxRaycastDistance,
                               ~0,
                               QueryTriggerInteraction.Ignore);
    }


    IEnumerator ActivateCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    public void ShowObject()
    {
        // Cambia la mesh e il materiale dell'oggetto fantasma
        if (GameManager.inst.inputManager.Player.Aim.triggered)
        {
            phantomObj_mf.mesh = hiddenObject.GetComponent<MeshFilter>().mesh;
            phantomObj_mr.material = phantomMat;
        }
        
        // Lo attiva
        phantomObj.SetActive(true);

        RaycastHit hit;

        if (CastRaycast(out hit))
        {
            // Mette l'oggetto fantasma nel punto dove ha colpito
            phantomObj.transform.position = hit.point
                                            - (raycastStartPoint.forward * objSafeDistance)
                                            + Vector3.up * placeDist;
        }
        else
        {
            // Mette l'oggetto fantasma alla distanza massima
            phantomObj.transform.position = raycastStartPoint.position
                                            + raycastStartPoint.forward * maxRaycastDistance;

        }

        phantomObj.transform.rotation = hiddenObject.transform.rotation;
        phantomObj.transform.localScale = hiddenObject.transform.localScale;
    }

    public void PlaceObject(float PlaceForce)
    {      
        // Lanciare un raycast in avanti solo se l'oggetto nascosto è diverso null
        if (isObjectInSlot)
        {
            RaycastHit hit;

            // Aggiunto maxRaycastDistance al raycast
            if (CastRaycast(out hit))
            {
                GameObject goHit = hit.transform.gameObject;
                
                // Verifica se l'oggetto colpito è diverso dal giocatore
                if (goHit != null && !goHit.CompareTag("Player"))
                {
                    Vector3 safeDistanceCalculated = (raycastStartPoint.forward * objSafeDistance);
                    
                    hiddenObject.transform.position = hit.point
                                                       - safeDistanceCalculated
                                                       + Vector3.up * placeDist;
                }
            }
            else
            {
                hiddenObject.transform.position = raycastStartPoint.position
                                                   + raycastStartPoint.forward * maxRaycastDistance;
            }

            //Destroy(hiddenObject.GetComponent<Smaterializzatore>());
            
            // Attiva l'oggetto
            hiddenObject.SetActive(true);

            // Assegna null a hiddenObject per indicare che non c'è più un oggetto nascosto
            hiddenObject = null;
            
            //Feedback
            colpo.Play();   //SFX
            ImageObjectCollected.SetActive(false);    //Nasconde l'immagine
        }

    }

    public void ShootObject(float ShootForce)
    {
        if (isObjectInSlot)
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
                projectileRb.AddForce(raycastStartPoint.transform.forward * ShootForce, ForceMode.Impulse);
            }

            // Assegna null a hiddenObject per indicare che non c'è più un oggetto nascosto
            hiddenObject = null;
        }
    }

    void HideObject(GameObject objToHide)
    {
        if (objToHide != null && objToHide.GetComponent<PickUp>() && canShoot)
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
            
            // Reset della velocita' del RigidBody
            ResetRBVelocity();
            
            // Attiva lo sprite a schermo
            ImageObjectCollected.SetActive(true);            
            
        }
    }


    public void ResetRBVelocity()
    {
        Rigidbody hiddenObjRb = hiddenObject.GetComponent<Rigidbody>();

        hiddenObjRb.velocity = Vector3.zero;
        hiddenObjRb.angularVelocity = Vector3.zero;
    }


    public bool GetIsObjectInSlot()
    {
        return isObjectInSlot;
    }


    #region EXTRA - Gizmo

    private void OnDrawGizmos()
    {
        if (isObjectInSlot && purpleHit.collider)
        {
            //Disegna la linea per indicare a quanto
            //alto da terra posiziona l'oggetto immagazzinato
            Gizmos.color = Color.gray;
            Gizmos.DrawRay(phantomObj.transform.position, Vector3.down * placeDist);
            Gizmos.DrawLine(phantomObj.transform.position + Vector3.down * placeDist,
                            purpleHit.point);
            

            //Disegna una piccola sfera dove colpisce il Raycast
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(purpleHit.point, 0.1f);
        }
    }

    #endregion
}
