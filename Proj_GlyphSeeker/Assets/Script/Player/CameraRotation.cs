using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] Transform cameraMasterPivot;
    [SerializeField] Transform cameraPivotTilt;
    [SerializeField] Camera playerCam;
    Transform playerCam_Tr;
    Vector3 dirCamPlayer;

    [Space(10)]
    [SerializeField] float rotVel = 6.5f;
    [SerializeField] Vector2 vertRotRange = new Vector2(-15, 52.5f);
    [Space(5)]
    [SerializeField] Vector2 camDistRange = new Vector2(1, 5);
    float xRot = 0f;
    Vector2 _camDistRange;
    float camSafeRadius;
    RaycastHit hitWall;
    bool hasCamHitWall;

    [Header("——  Aiming  ——")]
    [SerializeField] float camDist_aiming = 1.5f;
    [SerializeField] float fieldOfView_aiming = 50f;
    [Space(10)]
    [SerializeField] float camDist_purpleRune = 4f;
    [SerializeField] float fieldOfView_purpleRune = 55f;
    float currentCamDist = 3;
    float fieldOfView_normal = 65f;
    float currentFOV;

    bool centerMouse = true;



    void Awake()
    {
        playerCam_Tr = playerCam.transform;
        _camDistRange = camDistRange;
        
        fieldOfView_normal = playerCam.fieldOfView;
        currentFOV = fieldOfView_normal;


        //Imposta la "distanza sicura" per la telecamera
        camSafeRadius = playerCam.nearClipPlane * 1.5f + 0.1f;


        //Imposta il mouse al centro dello schermo
        SetCenterMouse(centerMouse);
    }

    void FixedUpdate()
    {
        #region Rotazione telecamera

        //Prende la rotazione
        InputAction inputRotation = GameManager.inst.inputManager.Player.Look;


        //Prende le (X,Y) dell'input
        float mouseX = inputRotation.ReadValue<Vector2>().x * rotVel * Time.deltaTime;
        float mouseY = inputRotation.ReadValue<Vector2>().y * rotVel * Time.deltaTime;


        #region Gamepad

        if (GamepadCheck())   //Se c'e' almeno un Gamepad
        {
            string inpuName = inputRotation.activeControl.name,
                   gamepadNameCameraMov = Gamepad.current.rightStick.name;

            //Aumenta la sensibilita' se si usa un controller/gamepad
            if (inpuName == gamepadNameCameraMov)
            {
                mouseX *= 10;
                mouseY *= 10;
            }
        }
        #endregion


        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, vertRotRange.x, vertRotRange.y);         //Restrige la rotazione nei limiti

        cameraPivotTilt.localRotation = Quaternion.Euler(xRot, 0, 0f);    //La X come rotazione Y del pivot orizz
        cameraMasterPivot.Rotate(Vector3.up * mouseX);                      //...e la Y come rotazione X del pivot vert

        #endregion



        //Limita la distanza della cam per non farla entrare nei muri
        LimitCameraDistance();



        #region Cambio del "Field of View"

        //Calcola il nuovo campo visivo (FOV)
        playerCam.fieldOfView = Mathf.SmoothStep(playerCam.fieldOfView,
                                                 currentFOV,
                                                 Time.deltaTime * 10f);

        #endregion
    }


    public bool GamepadCheck()
    {
        return Gamepad.all.Count > 0;
    }


    #region Gestione distanza Telecamera-Giocatore

    void LimitCameraDistance()
    {
        //Calcolo della direzione della telecamera
        dirCamPlayer = -cameraPivotTilt.forward;//playerCam_Tr.position - cameraPivotTilt.position;


        //Calcolo se la telecamera ha colpito un muro
        //(non colpisce i Trigger e "~0" significa che collide con tutti i layer)
        hasCamHitWall = Physics.Raycast(cameraPivotTilt.position,
                                        dirCamPlayer,
                                        out hitWall,
                                        _camDistRange.y + camSafeRadius,
                                        ~0,
                                        QueryTriggerInteraction.Ignore);


        //Se ha colpito il muro (e NON ha colpito il giocatore)
        //avvicina la telecamera,
        //se no la mette alla massima distanza
        currentCamDist = hasCamHitWall && !hitWall.transform.CompareTag("Player")
                           ? hitWall.distance - camSafeRadius
                           : _camDistRange.y;

        //Limita la distanza nel range
        currentCamDist = Mathf.Clamp(currentCamDist, _camDistRange.x, _camDistRange.y);


        //Calcola la nuova posizione
        Vector3 _camPosDist = playerCam_Tr.localPosition;
        _camPosDist.z = -currentCamDist;
        playerCam_Tr.localPosition = Vector3.Slerp(playerCam_Tr.localPosition,
                                                   _camPosDist,
                                                   Time.deltaTime * 10f);
    }


    public void SwitchMaxDist(bool isAiming, bool isPurpleRuneSelected)
    {
        //Cambia la distanza max della camera
        //(Se sta mirando, usa quella più vicina,
        // se no usa quella più lontana)
        float distToUse_aiming = isPurpleRuneSelected
                                  ? camDist_purpleRune
                                  : camDist_aiming;

        _camDistRange.y = isAiming
                            ? distToUse_aiming
                            : camDistRange.y;


        //Cambia il campo visivo/"field of view"
        //(Se sta mirando, lo diminuisce,
        // se no lo fa tornare normale)
        float fovToUse_aiming = isPurpleRuneSelected
                                 ? fieldOfView_purpleRune
                                 : fieldOfView_aiming;

        currentFOV = isAiming
                       ? fovToUse_aiming
                       : fieldOfView_normal;
    }

    #endregion


    public void SetCenterMouse(bool value)
    {
        centerMouse = value;


        //Blocca (o no) + nasconde (o meno)
        //il mouse al centro
        Cursor.visible = !centerMouse;
        Cursor.lockState = centerMouse
                           ? CursorLockMode.Locked
                           : CursorLockMode.None;
    }



    #region EXTRA - Cambiare l'Inspector

    private void OnValidate()
    {
        //Limita il range della rotazione verticale della telecamera
        //(con un min di -90° e un max di 90°)
        vertRotRange.x = Mathf.Clamp(vertRotRange.x, -90, vertRotRange.y);
        vertRotRange.y = Mathf.Clamp(vertRotRange.y, vertRotRange.x, 90);

        //Limita il range della distanza tra la telecamera e il giocatore
        //(sempre positivo)
        camDistRange.x = Mathf.Clamp(camDistRange.x, 0, camDistRange.y);
        camDistRange.y = Mathf.Clamp(camDistRange.y, camDistRange.x, camDistRange.y);

        //Limita la distanza e il FOV di "mira" per non andare oltre il range
        camDist_aiming = Mathf.Clamp(camDist_aiming, camDistRange.x, camDistRange.y);
        fieldOfView_aiming = Mathf.Clamp(fieldOfView_aiming, 1, playerCam.fieldOfView);

        //Limita la distanza e il FOV per quando si seleziona la Runa Viola
        camDist_purpleRune = Mathf.Clamp(camDist_purpleRune, camDist_aiming, camDistRange.y);
        fieldOfView_purpleRune = Mathf.Clamp(fieldOfView_purpleRune, fieldOfView_aiming, playerCam.fieldOfView);
    }

    #endregion


    #region EXTRA - Gizmo

    private void OnDrawGizmos()
    {
        //Disegna una linea grigia che collega la telecamera al giocatore
        Vector3 camPos = playerCam.transform.position;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(cameraPivotTilt.position, camPos);


        //Disegna due sfere vuote grigie;
        //una quanto il nearClipPlane della telecamera,
        //l'altra per quanto può avvicinarsi al giocatore
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(playerCam.transform.position, camSafeRadius/2);
        Gizmos.DrawWireSphere(cameraMasterPivot.position, camDistRange.x);
    }

    private void OnDrawGizmosSelected()
    {
        //Disegna un cubetto verde che indica dove ha colpito il muro
        Gizmos.color = Color.green;
        if (hasCamHitWall)
        {
            Gizmos.DrawLine(cameraPivotTilt.position, hitWall.point);
            Gizmos.DrawCube(hitWall.point, Vector3.one * 0.1f);
        }


        //Disegna due linee blu nei limiti della rotazione della telecamera
        Vector3 camPos = playerCam.transform.position;

        Vector3 camPos2D = new Vector3(camPos.x,
                                       cameraPivotTilt.position.y,
                                       camPos.z),
                dir = (camPos2D - cameraPivotTilt.position).normalized;

        Quaternion minRot = Quaternion.AngleAxis(vertRotRange.x, cameraPivotTilt.right),
                   maxRot = Quaternion.AngleAxis(vertRotRange.y, cameraPivotTilt.right);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(cameraPivotTilt.position, minRot * dir * 1.5f);
        Gizmos.DrawRay(cameraPivotTilt.position, maxRot * dir * 1.5f);
    }

    #endregion
}
