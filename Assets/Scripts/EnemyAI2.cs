using UnityEngine;

public class EnemyAI2 : MonoBehaviour
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

    private bool playerInHitbox;

    private Enemy_Patrol patrol;
    private Transform visual;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        if (player == null)
        {
            Debug.Log("PLAYER IS NULL - AI CANNOT TARGET");
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) player = found.transform;
            
        }

        patrol = GetComponent<Enemy_Patrol>();
        if (animator != null) visual = animator.transform;

        if (hitbox != null)
            hitbox.SetActive(false);
    }

    void Update()
    {
        Debug.Log("EnemyAI2 running");
        if (rb == null) return;

        CheckGround();

        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        bool shouldChase = dist <= chaseDistance || playerInHitbox;

        if (patrol != null)
            patrol.isOverridden = shouldChase;

        if (!shouldChase)
            return;

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
                ResetAttack();

            return;
        }

        // PRIORITY: hitbox OR close range
        if (playerInHitbox || dist <= attackDistance)
        {
            Attack();
        }
        else
        {
            Chase();
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

    void Chase()
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

        float heightDiff = player.position.y - transform.position.y;

        if (isGrounded && (heightDiff > 1f || touchingWall))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void Flip(float dir)
    {
        if (dir == 0 || visual == null) return;

        if ((dir < 0 && visual.localScale.x > 0) ||
            (dir > 0 && visual.localScale.x < 0))
        {
            visual.localScale = new Vector3(
                visual.localScale.x * -1,
                visual.localScale.y,
                visual.localScale.z
            );
        }
    }

    void Attack()
    {
        isAttacking = true;
        attackTimer = attackDuration;

        rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.Play("Attack");

        if (hitbox != null)
            hitbox.SetActive(true);
    }

    void ResetAttack()
    {
        isAttacking = false;

        if (hitbox != null)
            hitbox.SetActive(false);
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetFloat("magnitude", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("isGrounded", isGrounded);
    }

    // Called by hitbox
    public void SetPlayerInHitbox(bool state)
    {
        playerInHitbox = state;
    }
}