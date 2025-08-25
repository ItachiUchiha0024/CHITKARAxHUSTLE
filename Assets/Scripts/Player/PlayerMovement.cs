using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Optional Settings")]
    public bool useRawInput = true;

    private Rigidbody2D rb;
    private float horizontalInput;
    private float verticalInput;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the player GameObject!");
            enabled = false;
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player GameObject!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // Capture movement input
        horizontalInput = useRawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        verticalInput = useRawInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        // Set animation parameters
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("IsMoving", movement != Vector2.zero);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;
        rb.linearVelocity = velocity;
    }
}
