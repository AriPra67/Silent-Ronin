using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private float horizontalMovement;
    private bool facingRight = true;
    private bool isAttacking;

    [Header("Jumping")]
    public float jumpPower = 10f;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.7f, 0.2f);
    public LayerMask groundLayer;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleFlip();
    }

    // ---------------- MOVEMENT ----------------
    void HandleMovement()
    {
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
    }

    // ---------------- FLIP ----------------
    void HandleFlip()
    {
        if (horizontalMovement > 0 && !facingRight) Flip();
        else if (horizontalMovement < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ---------------- INPUT ----------------
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;

        if (context.canceled)
            horizontalMovement = 0f;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
    }

    // ---------------- ATTACK ----------------
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    // ---------------- GROUND CHECK ----------------
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(
            groundCheckPos.position,
            groundCheckSize,
            0f,
            groundLayer
        ) != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPos == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}