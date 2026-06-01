using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public GameObject hitbox;
    public float attackDuration = 0.2f;
    public float attackCooldown = 0.45f;

    [Header("Dash")]
    public float dashForce = 12f;
    public float dashDuration = 0.12f;

    [Header("Audio")]
    public AudioSource attackAudio;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerHitbox playerHitbox;

    private bool canAttack = true;
    private float originalGravityScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb != null)
            originalGravityScale = rb.gravityScale;

        if (hitbox != null)
        {
            playerHitbox = hitbox.GetComponent<PlayerHitbox>();
            hitbox.SetActive(false);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
        {
            StartCoroutine(DashAttack());
        }
    }

    IEnumerator DashAttack()
    {
        canAttack = false;

        if (animator != null)
            animator.SetTrigger("Attack");

        if (attackAudio != null)
            attackAudio.Play();

        if (hitbox != null)
            hitbox.SetActive(true);

        if (playerHitbox != null)
            playerHitbox.StartAttack();

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        // Make dash rigid/flat
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        float dashTimer = 0f;

        while (dashTimer < dashDuration)
        {
            rb.linearVelocity = new Vector2(direction * dashForce, 0f);

            dashTimer += Time.deltaTime;
            yield return null;
        }

        // Stop dash momentum immediately
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravityScale;

        yield return new WaitForSeconds(attackDuration);

        if (playerHitbox != null)
            playerHitbox.EndAttack();

        if (hitbox != null)
            hitbox.SetActive(false);

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }
}