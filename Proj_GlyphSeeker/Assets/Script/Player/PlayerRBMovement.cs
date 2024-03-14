using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRBMovement : MonoBehaviour, IPlayer
{
    Rigidbody rb;

    [SerializeField] float playerSpeed = 7.5f;
    [SerializeField] float jumpPower = 8.5f;
    [SerializeField] float increasedGravityMult = 3.5f;
    [Min(0)]
    [SerializeField] float addedJumpHeight = 1f;
    float x_movem, z_movem;

    Vector3 moveVector;

    [Space(10)]
    [SerializeField] float coyoteMaxTime = 0.8f;
    float coyoteTime_current = 0f;
    bool canCoyote;

    [Space(10)]
    [SerializeField] float dmgKnockbackPower = 10f;

    [Space(20)]
    [SerializeField] float groundCheck_treshold = 0.25f;
    float playerHalfHeight;
    float spherecastRadius = 0.5f;

    bool isOnGround = false;
    RaycastHit hitBase;

    bool isJumping = false;
    bool hasJumpedFromGround = false;
    bool hasJumped_doOnce = true;
    float jumpPower_divider = 1;

    [Header("\t—— Roll ——")]
    [SerializeField] /*HealthSystem*/MonoBehaviour plHealthScr;
    [Min(0)]
    [SerializeField] float maxRollTime = 2.5f;
    [SerializeField] float maxInvincibleTime = 0.25f;
    [Min(0.1f)]
    [SerializeField] float rollForce = 25;
    float currentRollTime;
    bool canMove,
         isMovingInput;
    bool canRoll,
         isRolling;




    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canMove = true;
        canRoll = true;

        playerHalfHeight = GetComponent<CapsuleCollider>().height / 2;
    }

    private void Update()
    {
        //Prende l'input della schaivata
        isRolling = GameManager.inst.inputManager.Player.Roll.ReadValue<float>() > 0;


        //Prende gli assi dall'input di movimento
        x_movem = GameManager.inst.inputManager.Player.Movement.ReadValue<Vector2>().x;
        z_movem = GameManager.inst.inputManager.Player.Movement.ReadValue<Vector2>().y;

        moveVector = (transform.forward * z_movem + transform.right * x_movem).normalized;      //Vettore movimento orizzontale

        isMovingInput = moveVector != Vector3.zero;


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
                                        playerHalfHeight + groundCheck_treshold - spherecastRadius,
                                        ~0,
                                        QueryTriggerInteraction.Ignore);


        #region Salto

        //Se tieni premuto il tasto di salto
        //e il divisore e' sotto il limite...
        if (isJumping && jumpPower_divider <= addedJumpHeight+1)
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


        float airResistance = !isOnGround ? 0.45f : 1;   //Diminuisce la velocita' orizz. se si trova in aria

        if (canMove)
        {
            //Movimento orizzontale (semplice) del giocatore
            rb.AddForce(moveVector.normalized * playerSpeed * 10f * airResistance, ForceMode.Force);
        }


        #region Limitazioni della velocita'

        //Prende la velocita' orizzontale e verticale
        //del giocatore, ma separate
        Vector3 horizVel = new Vector3(rb.velocity.x, 0, rb.velocity.z),
                vertVel = rb.velocity.y * Vector3.up;


        //Controllo se si accelera troppo, cioe' si supera la velocita'
        if (horizVel.magnitude >= playerSpeed)
        {
            //Limita la velocita' a quella prestabilita, riportandola al RigidBody
            Vector3 limit = horizVel.normalized * playerSpeed;
            rb.velocity = new Vector3(limit.x, rb.velocity.y, limit.z);
        }


        //Rallenta il giocatore se si trova in aria
        //e non si sta muovendo
        if(moveVector == Vector3.zero  &&  !isOnGround)
        {
            rb.AddForce(-horizVel / airResistance);
        }



        //Aumenta la gravita' quando il giocatore sta cadendo...
        if (rb.velocity.y < 0  &&  hasJumpedFromGround)
        {
            float _fallingVel = vertVel.y;

            _fallingVel *= increasedGravityMult;
            _fallingVel = Mathf.Clamp(_fallingVel, Physics.gravity.y * 2, 0);

            rb.velocity = new Vector3(rb.velocity.x, _fallingVel, rb.velocity.z);
        }

        //...o sta salendo in aria dopo un salto
        if (rb.velocity.y > 0  &&  hasJumpedFromGround)
        {
            float _partialFallingVel = increasedGravityMult * 0.2f;

            rb.AddForce(-transform.up * _partialFallingVel, ForceMode.VelocityChange);
        }

        #endregion


        #region Schivata / roll (timer)

        if (canRoll)
        {
            if (isRolling  &&  isOnGround)
            {
                //Toglie la possibilita' di muoversi al giocatore
                //(per dare il tempo di schivare)
                canMove = false;


                //Calcola la direzione dove schivare
                // (la stessa di dove sta andando,
                //  ma indietro se non sta premendo nulla)
                Vector3 dirToRoll = isMovingInput
                                     ? moveVector * 2
                                     : -transform.forward;

                //Resetta la velocita' XZ e applica la forza di schivata
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                rb.AddForce(dirToRoll * rollForce + Vector3.up * 0.2f, ForceMode.Impulse);



                //Toglie la possibilita' di schiavare un'altra volta
                canRoll = false;
            }
        }
        //Quando non puo' piu' schivare,
        //inizia il timer
        else
        {
            if (currentRollTime >= maxInvincibleTime)
            {
                //Toglie l'invincibilita' al giocatore
                //playerHalfHeight.SetInvincible(false);

                //Puo' tornare a muoversi
                canMove = true;
            }

            if (currentRollTime >= maxRollTime)
            {
                //Puo' tornare a schivare
                canRoll = true;


                //Reset del timer
                currentRollTime = 0;
            }
            else
            {
                //Aumento del timer
                currentRollTime += Time.deltaTime;
            }
        }

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


    /// Danno --> da sistemare in HealthSystem
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Knockback(-transform.forward, dmgKnockbackPower);
        }
    }



    #region EXTRA - Cambiare l'Inspector

    private void OnValidate()
    {
        //Limita i tempi massimi della schivata e per potersi muovere di nuovo
        maxInvincibleTime = Mathf.Clamp(maxInvincibleTime, 0, maxRollTime);
        maxRollTime = Mathf.Clamp(maxRollTime, maxInvincibleTime, maxRollTime);
    }

    #endregion


    #region EXTRA - Gizmo

    private void OnDrawGizmos()
    {
        //Disegna lo SphereCast per capire se e' a terra o meno (togliendo l'altezza del giocatore)
        float _halfPlayerHeight = Application.isPlaying
                                    ? playerHalfHeight
                                    : GetComponent<CapsuleCollider>().height / 2;

        Gizmos.color = new Color(0.85f, 0.85f, 0.85f, 1);
        Gizmos.DrawWireSphere(transform.position + (-transform.up * _halfPlayerHeight)
                               + (-transform.up * groundCheck_treshold)
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
