using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRBMovement2 : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float playerSpeed = 7.5f;
    [SerializeField] float jumpPower = 8.5f;
    [SerializeField] float increasedGravity = 3.5f;
    float x_movem, z_movem;

    Vector3 moveVector;

    [Space(10)]
    [SerializeField] float coyoteMaxTime = 0.8f;
    float coyoteTime_current = 0f;
    bool canCoyote;

    [Space(10)]
    [SerializeField] float knockbackPower = 10f;

    [Space(20)]
    [SerializeField] float treshold_groundCheck = 0.25f;
    float playerHalfHeight;
    float spherecastRadius = 0.5f;

    bool isOnGround = false;
    RaycastHit hitBase;

    bool isJumping = false;
    bool hasJumpedFromGround = false;
    bool hasJumped_doOnce = true;
    float jumpPower_divider = 1;



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

        moveVector = (transform.forward * z_movem + transform.right * x_movem).normalized;      //Vettore movimento orizzontale

        
        //Prende l'input di salto
        isJumping = GameManager.inst.inputManager.Player.Jump.ReadValue<float>() > 0;

    }

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
        
        if(moveVector.x == 0 && moveVector.y == 0 && isOnGround)
        {
            //rb.velocity = new Vector3(0f, 0f, 0f);
            moveVector = (transform.forward * z_movem - transform.right * x_movem).normalized;
        }


        #region Salto

        //Se tieni premuto il tasto di salto
        //e il divisore e' sotto il limite...
        if (isJumping && jumpPower_divider <= 2.5f)
        {
            if (isOnGround || canCoyote)
            {
                hasJumpedFromGround = true;        //Attiva il "salto da terra"
                hasJumped_doOnce = true;   //Permette di saltare un'unica volta
                jumpPower_divider = 1;           //Reset del divis. della potenza
            }

            //Se posso saltare
            //(una sola volta dal terreno OPPURE dal Coyote Time)
            if (hasJumped_doOnce
               &&
               (hasJumpedFromGround || canCoyote))
            {
                //...Allora aumenta il divisore che verra' diviso...
                jumpPower_divider += Time.deltaTime * 2;

                //...E fa saltare il giocatore
                Jump(jumpPower_divider <= 1.1f
                       ? jumpPower
                       : jumpPower / jumpPower_divider);


                /* Per il salto, più si tiene premuto,
                 * meno forza avra' il salto,
                 * risultando in una salita che rallenta;
                 *
                 * Inoltre, il salto controlla se il divisore
                 * e' troppo vicino ad 1: se lo e' gli passa
                 * il valore della potenza inalterato
                 */
            }
        }
        else
        {
            hasJumped_doOnce = false;    //Toglie la possibilita' di saltare in aria


            if (isOnGround)
            {
                jumpPower_divider = 1;       //Reset del divis. della potenza di salto
                hasJumpedFromGround = false;   //Reset dell'aver saltato a terra
            }
        }

        #endregion


        #region Coyote Time

        //La var. "possoCoyote" indica quando
        //posso fare il salto col Coyote Time,
        //ovvero: quando il timer e' dentro il limite,
        //        se si trova in aria
        //        e se non ho saltato prima
        canCoyote = coyoteTime_current > 0f
                    &&
                    !isOnGround
                    &&
                    !hasJumpedFromGround;


        //Se si trova in aria e non ha gia' saltato prima
        if (!isOnGround && !hasJumpedFromGround)
        {
            //Controlla se posso fare
            //il salto col Coyote Time
            if (canCoyote)
                coyoteTime_current -= Time.deltaTime;    //...Diminuisce il timer
        }
        else
        {
            coyoteTime_current = coyoteMaxTime;    //Se no, fa un reset del timer
        }

        #endregion


        //Movimento orizzontale (semplice) del giocatore
        rb.AddForce(moveVector.normalized * playerSpeed * 10f, ForceMode.Force);


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


        //Applica l'attrito dell'aria al giocatore
        //(Riduce la velocita' se il giocatore e' in aria e si sta muovendo)
        if (!isOnGround
            &&
            (rb.velocity.x >= 0.05f || rb.velocity.z >= 0.05f))
        {
            rb.AddForce(new Vector3(-rb.velocity.x * 0.1f, increasedGravity, -rb.velocity.z * 0.1f), ForceMode.Force);
        }


        #region OLD_UNUSED
        //if (rb.velocity.y < 0)
        //{
        //    //Aumenta la gravità mentre scende
        //    rb.velocity += Vector3.up * Physics.gravity.y * (increasedGravity - 1) * Time.deltaTime;
        //}
        //else
        //{
        //    //sistema la grav in salita
        //    rb.velocity += Vector3.up * Physics.gravity.y * Time.deltaTime;
        //} 
        #endregion

        #endregion
    }


    void Jump(float power)
    {
        //Resetta la velocita' Y e applica la forza d'impulso verso l'alto
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * power, ForceMode.Impulse);
    }

    public void Knockback(Vector3 direction, float power)
    {
        rb.AddForce(direction * power, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Knockback(-transform.forward, knockbackPower);
        }
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
