using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{   
    [Header("Movemment Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheakRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody2D rb;

    private float horizontalInput;
    private float facingDirection = 1f;

    private bool isGrounded;
    private bool isDashing;
    private bool canDash = true;

    private float originalGravityScale;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheakRadius, groundLayer);

        if (horizontalInput !=0) {
            facingDirection = Mathf.Sign(horizontalInput);
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isDashing)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash){
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        Move();
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private IEnumerator Dash() {
        isDashing = true;
        canDash = false;

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravityScale;
        rb.linearVelocity = new Vector2(0f,0f);

        isDashing = false;

        yield return new WaitForSeconds(dashDuration);

        canDash = true;

    }
    private void onDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheakRadius);
        }
    }
}

