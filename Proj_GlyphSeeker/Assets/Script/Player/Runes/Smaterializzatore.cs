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
    private float shootForceForward;
    [SerializeField]
    private float shootForceUp;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private float placeDist = 0.5f;
    [SerializeField]
    private float cooldown = 3f;
    private bool isObjectInSlot;
    private bool shootableObj;
    private bool placebleObj;

    RaycastHit purpleHit;

    [SerializeField]
    private GameObject batteryBullet;
    [SerializeField]
    private GameObject enemyToBullet;
    [SerializeField]
    private GameObject kamikazeBullet;

    private GameObject objToShoot;
    private GameObject hiddenObject;

    private float objSafeDistance;

    [SerializeField] GameObject phantomObj;
    [SerializeField] Material phantomMat;
    MeshFilter phantomObj_mf;
    MeshRenderer phantomObj_mr;
    
    [SerializeField]
    private Image imageObjectCollected;
    [SerializeField]
    private Image currentImageObjectCollected;
    [SerializeField]
    private Sprite batterySprite,
                  kamikazeSprite,
                  rockSprite,
                  objectSprite;
                    
    public Image mirino;
    public AudioSource errore;
    public AudioSource colpo;

    private void Start()
    {
        canShoot = true;
        isObjectInSlot = false;
        placebleObj = false;
        shootableObj = false;

        phantomObj_mf = phantomObj.GetComponent<MeshFilter>();
        phantomObj_mr = phantomObj.GetComponent<MeshRenderer>();

        imageObjectCollected.gameObject.SetActive(false);
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
                ShootObject(shootForceForward);
                StartCoroutine(ActivateCooldown());
            }
            // Se non c'è un oggetto nascosto, spara solo se il raycast ha colpito qualcosa
            else if (hitObject != null)
            {
                ShootObject(shootForceForward);
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
            if (isObjectInSlot && placebleObj == true)
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
        return Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, maxRaycastDistance, 
                               ~0, QueryTriggerInteraction.Ignore);
    }

    IEnumerator ActivateCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    public void ShowObject()
    {
        if(placebleObj == true)
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
    }

    public void PlaceObject(float PlaceForce)
    {      
        // Lanciare un raycast in avanti solo se l'oggetto nascosto è diverso null
        if (isObjectInSlot && placebleObj == true)
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
            
            // Attiva l'oggetto
            hiddenObject.SetActive(true);

            // Assegna null a hiddenObject per indicare che non c'è più un oggetto nascosto
            hiddenObject = null;
            
            //Feedback
            colpo.Play();   //SFX
            imageObjectCollected.gameObject.SetActive(false);    //Nasconde l'immagine
        }

    }

    public void ShootObject(float ShootForce)
    {
        if (isObjectInSlot && shootableObj == true)
        {
            if (hiddenObject == batteryBullet)
            {
                objToShoot = Instantiate(hiddenObject, shootPoint.position, shootPoint.rotation);
            }
            else if(hiddenObject == enemyToBullet)
            {
                objToShoot = Instantiate(hiddenObject, shootPoint.position, shootPoint.rotation);
            }
            else if(hiddenObject == kamikazeBullet)
            {
                objToShoot = Instantiate(hiddenObject, shootPoint.position, shootPoint.rotation);
            }
            else
            {
                hiddenObject.SetActive(true);
                objToShoot = Instantiate(hiddenObject, shootPoint.position, shootPoint.rotation);
                hiddenObject.SetActive(false);
            }

            // Rimuovi il componente PlayerShooting dal clone per evitare duplicati
            Destroy(objToShoot.GetComponent<Smaterializzatore>());

            // Ottenere il componente Rigidbody dell'oggetto clonato
            Rigidbody projectileRb = objToShoot.GetComponent<Rigidbody>();

            // Aggiungi una forza in avanti all'oggetto clonato
            if (projectileRb != null)
            {
                projectileRb.AddForce(raycastStartPoint.transform.forward * ShootForce + raycastStartPoint.transform.up * shootForceUp, ForceMode.Impulse);
            }

            imageObjectCollected.gameObject.SetActive(false);

            // Assegna null a hiddenObject per indicare che non c'è più un oggetto nascosto
            hiddenObject = null;
        }
    }

    void HideObject(GameObject objToHide)
    {
        if (objToHide != null && objToHide.GetComponent<PickUp>() && canShoot)
        {       
            PickUp newObjToHide = objToHide.GetComponent<PickUp>();

            if (newObjToHide.canPickUp == true)
            {
                if (objToHide.GetComponent<BatteryToCharge>() != null)
                {
                    currentImageObjectCollected.sprite = batterySprite;

                    // Attiva lo sprite a schermo
                    imageObjectCollected.gameObject.SetActive(true);

                    // Disattiva l'oggetto colpito
                    objToHide.SetActive(false);

                    // Memorizza l'oggetto nascosto
                    hiddenObject = batteryBullet;

                    shootableObj = true;
                    placebleObj = false;

                    if (!objToHide.GetComponent<Rigidbody>())
                    {
                        objToHide.AddComponent<Rigidbody>();
                    }
                }
                else if (objToHide.GetComponent<IEnemy>() != null)
                {
                    if(objToHide.GetComponent<KamikazeEnemy>() != null)
                    {
                        currentImageObjectCollected.sprite = kamikazeSprite;

                        // Attiva lo sprite a schermo
                        imageObjectCollected.gameObject.SetActive(true);

                        // Disattiva l'oggetto colpito
                        objToHide.SetActive(false);

                        // Memorizza l'oggetto nascosto
                        hiddenObject = kamikazeBullet;
                    }
                    else
                    {
                        currentImageObjectCollected.sprite = rockSprite;

                        // Attiva lo sprite a schermo
                        imageObjectCollected.gameObject.SetActive(true);

                        // Disattiva l'oggetto colpito
                        objToHide.SetActive(false);

                        // Memorizza l'oggetto nascosto
                        hiddenObject = enemyToBullet;
                    }

                    shootableObj = true;
                    placebleObj = false;
                }
                else
                {
                    currentImageObjectCollected.sprite = objectSprite;

                    // Attiva lo sprite a schermo
                    imageObjectCollected.gameObject.SetActive(true);

                    // Disattiva l'oggetto colpito
                    objToHide.SetActive(false);

                    // Memorizza l'oggetto nascosto e la distanza
                    hiddenObject = objToHide;
                    objSafeDistance = objToHide.GetComponent<PickUp>().safeDistance;

                    shootableObj = true;
                    placebleObj = true;

                    if (!objToHide.GetComponent<Rigidbody>())
                    {
                        objToHide.AddComponent<Rigidbody>();
                    }
                }

                //attiva il suono
                colpo.Play();

                // Reset della velocita' del RigidBody
                ResetRBVelocity();

                // Aggiorna lo sprite dell'icona collezionabile
                UpdateSprite();
            }
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

    private void UpdateSprite()
    {
        imageObjectCollected.sprite = currentImageObjectCollected.sprite;
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