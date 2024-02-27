using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AudioSource caduta;
    public static int playerIndex = 0;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float normalGravity = -30f;
    [SerializeField] private float increasedGravity = -60f;
    [SerializeField] private float currentGravity;
    Vector3 inputDirection;
    Vector3 velocity;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    bool isGrounded;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimer = 0f;

    private bool hasJumped = false;

    private bool isJumping = false;
    private float jumpTime = 0f;
    public float maxJumpDuration = 1.0f;

    void ManageDeath()
    {
        caduta.Play();
        rb.velocity = Vector3.zero;
        transform.position = Checkpoint.GetActiveCheckPointPosition();
    }

    void Start()
    {
        caduta = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        currentGravity = normalGravity;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.5f;
            currentGravity = normalGravity;
            coyoteTimer = coyoteTime;
            hasJumped = false;
        }
        else
        {
            currentGravity = increasedGravity;
            coyoteTimer -= Time.deltaTime;
        }

        float x = GameManager.inst.inputManager.Player.Movement.ReadValue<Vector2>().x;
        float z = GameManager.inst.inputManager.Player.Movement.ReadValue<Vector2>().y;

        inputDirection = new Vector3(x, 0f, z).normalized;

        Vector3 move = transform.right * inputDirection.x + transform.forward * inputDirection.z;

        rb.MovePosition(rb.position + move * speed * Time.deltaTime);

        velocity.y += currentGravity * Time.deltaTime;

        if (GameManager.inst.inputManager.Player.Jump.WasPressedThisFrame() && (isGrounded || coyoteTimer > 0f) && !hasJumped)
        {
            isJumping = true;
            jumpTime = 0f;
        }

        if (GameManager.inst.inputManager.Player.Jump.WasReleasedThisFrame() && isJumping)
        {
            isJumping = false;
        }

        if (isJumping)
        {
            if (jumpTime < maxJumpDuration)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * currentGravity);
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            ManageDeath();
        }
    }
}

