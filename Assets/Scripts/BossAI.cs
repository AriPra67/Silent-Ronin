using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject hitboxVisual;

    [Header("Movement")]
    public float speed = 2.2f;

    [Header("Turning")]
    public float turnDelay = 0.4f;
    private float turnTimer;
    private int facingDirection = 1;

    [Header("Attack Range")]
    public float attackRangeX = 5f;
    public float attackRangeY = 3f;

    [Header("Attack Timing")]
    public float attackCooldown = 2.5f;
    public float attackWindup = 1.2f;
    public float attackActiveTime = 0.35f;
    public float attackRecovery = 1.2f;

    [Header("Attack Movement")]
    public float attackLungeForce = 4f;

    [Header("Damage")]
    public int attackDamage = 1;

    [Header("Debug")]
    public bool showAttackDebugBox = true;

    private float cooldownTimer;
    private bool isAttacking;
    private bool hasDamagedThisAttack;

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        if (hitboxVisual != null)
            hitboxVisual.SetActive(false);

        facingDirection = transform.localScale.x >= 0 ? 1 : -1;
    }

    void Update()
    {
        if (rb == null || player == null)
            return;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (isAttacking)
            return;

        ChasePlayer();

        if (PlayerIsInAttackRange() && cooldownTimer <= 0f)
        {
            StartCoroutine(AttackRoutine());
            return;
        }

        UpdateAnimator();
    }

    void ChasePlayer()
    {
        float xDifference = player.position.x - transform.position.x;
        int desiredDirection = xDifference >= 0 ? 1 : -1;

        if (desiredDirection != facingDirection)
        {
            turnTimer += Time.deltaTime;

            if (turnTimer >= turnDelay)
            {
                facingDirection = desiredDirection;
                Flip(facingDirection);
                turnTimer = 0f;
            }
        }
        else
        {
            turnTimer = 0f;
        }

        rb.linearVelocity = new Vector2(facingDirection * speed, rb.linearVelocity.y);
    }

    bool PlayerIsInAttackRange()
    {
        float xDistance = Mathf.Abs(player.position.x - transform.position.x);
        float yDistance = Mathf.Abs(player.position.y - transform.position.y);

        return xDistance <= attackRangeX && yDistance <= attackRangeY;
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        hasDamagedThisAttack = false;
        cooldownTimer = attackCooldown;

        rb.linearVelocity = Vector2.zero;

        if (animator != null)
        {
            animator.SetFloat("magnitude", 0f);
            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
        }

        // WINDUP: animation plays first, but boss cannot damage yet
        yield return new WaitForSeconds(attackWindup);

        if (hitboxVisual != null)
            hitboxVisual.SetActive(true);

        float activeTimer = 0f;

        // ACTIVE: boss lunges and can damage the player once
        while (activeTimer < attackActiveTime)
        {
            rb.linearVelocity = new Vector2(facingDirection * attackLungeForce, rb.linearVelocity.y);

            if (!hasDamagedThisAttack && PlayerIsInAttackRange())
            {
                DamagePlayer();
                hasDamagedThisAttack = true;
            }

            activeTimer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        if (hitboxVisual != null)
            hitboxVisual.SetActive(false);

        // RECOVERY: lets the attack animation finish before chasing again
        yield return new WaitForSeconds(attackRecovery);

        isAttacking = false;
    }

    void DamagePlayer()
    {
        PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("Boss damaged player");
        }
    }

    void UpdateAnimator()
    {
        if (animator == null)
            return;

        animator.SetFloat("magnitude", Mathf.Abs(rb.linearVelocity.x));
    }

    void Flip(int direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        if (showAttackDebugBox)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireCube(
                transform.position,
                new Vector3(attackRangeX * 2f, attackRangeY * 2f, 0f)
            );
        }
    }
}