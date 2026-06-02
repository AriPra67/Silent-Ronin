using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public PlayerHitbox playerHitbox;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private float horizontalMovement;

    private bool facingRight = true;
    private bool isAttacking;
    private bool controlsLocked;

    [Header("Jumping")]
    public float jumpPower = 10f;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallGravityMult = 2f;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.7f, 0.2f);
    public LayerMask groundLayer;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();

        if (playerHitbox == null)
            playerHitbox = GetComponentInChildren<PlayerHitbox>();

        if (playerHitbox != null)
            playerHitbox.EndAttack();
    }

    void Update()
    {
        if (controlsLocked)
            return;

        HandleMovement();
        HandleFlip();
        HandleGravity();
        HandleAnimations();
    }

    public void LockControls()
    {
        controlsLocked = true;
        horizontalMovement = 0f;
        isAttacking = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (playerHitbox != null)
            playerHitbox.EndAttack();
    }

    public void UnlockControls()
    {
        controlsLocked = false;
        horizontalMovement = 0f;
        isAttacking = false;
    }

    void HandleMovement()
    {
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
    }

    void HandleFlip()
    {
        if (horizontalMovement > 0 && !facingRight) Flip();
        else if (horizontalMovement < 0 && facingRight) Flip();
    }

    void HandleGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    void HandleAnimations()
    {
        animator.SetFloat("magnitude", Mathf.Abs(horizontalMovement));
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (controlsLocked)
        {
            horizontalMovement = 0f;
            return;
        }

        horizontalMovement = context.ReadValue<Vector2>().x;

        if (context.canceled)
            horizontalMovement = 0f;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (controlsLocked) return;

        if (context.performed && IsGrounded())
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

        if (context.canceled && rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (controlsLocked) return;

        if (context.performed && !isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            StartAttackHitbox();
        }
    }

    public void Attack2(InputAction.CallbackContext context)
    {
        if (controlsLocked) return;

        if (context.performed && !isAttacking)
        {
            animator.SetTrigger("Attack 2");
            isAttacking = true;
            StartAttackHitbox();
        }
    }

    public void Attack3(InputAction.CallbackContext context)
    {
        if (controlsLocked) return;

        if (context.performed && !isAttacking)
        {
            animator.SetTrigger("Attack 3");
            isAttacking = true;
            StartAttackHitbox();
        }
    }

    void StartAttackHitbox()
    {
        if (playerHitbox != null)
            playerHitbox.StartAttack();
    }

    public void EndAttack()
    {
        isAttacking = false;

        if (playerHitbox != null)
            playerHitbox.EndAttack();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool IsGrounded()
    {
        if (groundCheckPos == null) return false;

        return Physics2D.OverlapBox(
            groundCheckPos.position,
            groundCheckSize,
            0f,
            groundLayer
        );
    }

    public void ResetMovement()
    {
        horizontalMovement = 0f;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPos == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}