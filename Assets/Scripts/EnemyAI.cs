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

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        if (hitbox != null)
            hitbox.SetActive(false);
    }

    void Update()
    {
        if (rb == null) return;

        CheckGround();

        if (player == null)
        {
            Idle();
            UpdateAnimation();
            return;
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
                ResetAttack();

            UpdateAnimation();
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);
        float heightDiff = player.position.y - transform.position.y;

        if (dist <= attackDistance)
        {
            Attack();
        }
        else if (dist <= chaseDistance)
        {
            Chase(heightDiff);
        }
        else
        {
            Idle();
        }

        UpdateAnimation();
    }

    void CheckGround()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapBox(
            groundCheck.position,
            groundSize,
            0f,
            groundLayer
        );
    }

    void Chase(float heightDiff)
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        Flip(dir);

        bool touchingWall = false;

        if (wallCheck != null)
        {
            RaycastHit2D wallInfo = Physics2D.Raycast(
                wallCheck.position,
                Vector2.right * dir,
                wallCheckDistance,
                groundLayer
            );

            touchingWall = wallInfo.collider != null;
        }

        if (isGrounded && (heightDiff > 1f || touchingWall))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void Flip(float dir)
    {
        if (dir == 0) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
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
    }

    void UpdateAnimation()
    {
        if (animator == null || rb == null) return;

        animator.SetFloat("magnitude", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("isGrounded", isGrounded);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundSize);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.red;

            Vector3 dir = transform.localScale.x >= 0 ? Vector3.right : Vector3.left;

            Gizmos.DrawLine(
                wallCheck.position,
                wallCheck.position + dir * wallCheckDistance
            );
        }
    }
}