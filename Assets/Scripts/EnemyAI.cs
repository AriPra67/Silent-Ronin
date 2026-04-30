using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;

    public GameObject hitbox;

    [Header("Movement")]
    public float speed = 2f;
    public float jumpForce = 8f;

    [Header("Detection")]
    public float chaseDistance = 8f;
    public float attackDistance = 1.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 groundSize = new Vector2(0.5f, 0.2f);

    [Header("Wall / Platform Check")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.6f;

    [Header("Attack")]
    public float attackDuration = 0.4f;
    private float attackTimer;

    private bool isGrounded;
    private bool isAttacking;

    void Update()
    {
        if (player == null || rb == null) return;

        // ground check
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundSize, 0f, groundLayer);

        // handle attack timer first
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
                ResetAttack();

            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);
        float heightDiff = player.position.y - transform.position.y;

        // attack priority
        if (dist <= attackDistance)
        {
            Attack();
            return;
        }

        // chase + jump logic
        if (dist <= chaseDistance)
        {
            Chase(heightDiff);
            return;
        }

        Idle();
    }

    void Chase(float heightDiff)
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);

        // move
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        // flip
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;

        // 🔥 PLATFORM / WALL DETECTION
        RaycastHit2D wallInfo = Physics2D.Raycast(
            wallCheck.position,
            Vector2.right * dir,
            wallCheckDistance,
            groundLayer
        );

        // jump if:
        // - player is above OR
        // - wall / platform edge in front
        if (isGrounded && (heightDiff > 1f || wallInfo.collider != null))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    void Attack()
    {
        isAttacking = true;
        attackTimer = attackDuration;

        rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Attack");

        if (hitbox != null)
            hitbox.SetActive(true);
    }

    void ResetAttack()
    {
        isAttacking = false;

        if (hitbox != null)
            hitbox.SetActive(false);
    }

    void Idle()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        if (animator != null)
            animator.SetFloat("Speed", 0);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(groundCheck.position, groundSize);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                wallCheck.position,
                wallCheck.position + Vector3.right * wallCheckDistance
            );
        }
    }
}