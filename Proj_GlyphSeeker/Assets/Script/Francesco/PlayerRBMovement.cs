using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRBMovement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float playerSpeed = 7.5f;
    [SerializeField] float jumpPower = 8.5f;
    float x_movem, z_movem;

    Vector3 moveVector;

    [Space(20)]
    [SerializeField] float treshold_groundCheck = 0.25f;
    float playerHalfHeight;
    float spherecastRadius = 0.5f;

    bool isOnGround = false;

    bool hasJumped = false;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerHalfHeight = GetComponent<CapsuleCollider>().height / 2;
    }

    private void Update()
    {
        //Prende gli assi dall'input di movimento
        x_movem = GameManager.inst.inputManager.Player.Movement.ReadValue<Vector2>().x;
        z_movem = GameManager.inst.inputManager.Player.Movement.ReadValue<Vector2>().y;

        moveVector = transform.forward * z_movem + transform.right * x_movem;      //Vettore movimento orizzontale


        //Prende l'input di salto
        hasJumped = GameManager.inst.inputManager.Player.Jump.ReadValue<float>() > 0;
    }

    RaycastHit hitBase;
    void FixedUpdate()
    {
        //Calcolo se si trova a terra
        //(non colpisce i Trigger e "~0" significa che collide con tutti i layer)
        isOnGround = Physics.SphereCast(transform.position,
                                           spherecastRadius,
                                           -transform.up,
                                           out hitBase,
                                           playerHalfHeight + treshold_groundCheck - spherecastRadius,
                                           ~0,
                                           QueryTriggerInteraction.Ignore);


        float airVelMult = !isOnGround ? 0.65f : 1;   //Diminuisce la velocita' orizz. se si trova in aria


        //Salta se premi Spazio e si trova a terra
        if (hasJumped && isOnGround)
        {
            //Resetta la velocita' Y e applica la forza d'impulso verso l'alto
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        }


        //Movimento orizzontale (semplice) del giocatore
        rb.AddForce(moveVector.normalized * playerSpeed * 10f, ForceMode.Force);

        //Applica l'attrito dell'aria al giocatore
        //(Riduce la velocita' se il giocatore e' in aria e si sta muovendo)
        if (!isOnGround
            &&
            (rb.velocity.x >= 0.05f || rb.velocity.z >= 0.05f))
        {
            rb.AddForce(new Vector3(-rb.velocity.x * 0.1f, 0, -rb.velocity.z * 0.1f), ForceMode.Force);
        }


        #region Limitazione della velocita'

        //Prende la velocita' orizzontale del giocatore
        Vector3 horizVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        //Controllo se si accelera troppo, cioe' si supera la velocita'
        if (horizVel.magnitude >= playerSpeed)
        {
            //Limita la velocita' a quella prestabilita, riportandola al RigidBody
            Vector3 limit = horizVel.normalized * playerSpeed * airVelMult;
            rb.velocity = new Vector3(limit.x, rb.velocity.y, limit.z);
        }

        #endregion
    }


    #region EXTRA - Gizmo

    private void OnDrawGizmos()
    {
        //Disegna lo SphereCast per capire se e' a terra o meno (togliendo l'altezza del giocatore)
        float _halfPlayerHeight = Application.isPlaying
                                    ? playerHalfHeight
                                    : GetComponent<CapsuleCollider>().height / 2;

        Gizmos.color = new Color(0.85f, 0.85f, 0.85f, 1);
        Gizmos.DrawWireSphere(transform.position + (-transform.up * _halfPlayerHeight)
                               + (-transform.up * treshold_groundCheck)
                               - (-transform.up * spherecastRadius),
                              spherecastRadius);

        //Disegna dove ha colpito se e' a terra e se ha colpito un'oggetto solido (no trigger)
        Gizmos.color = Color.green;
        if (isOnGround && hitBase.collider)
        {
            Gizmos.DrawLine(hitBase.point + (transform.up * hitBase.distance), hitBase.point);
            Gizmos.DrawCube(hitBase.point, Vector3.one * 0.1f);
        }
    }

    #endregion
}
