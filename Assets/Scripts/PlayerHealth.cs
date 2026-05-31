using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //changed to float - Xi
    public float maxHealth = 3f;
    private float currentHealth;

    public HealthUI healthUI;
    public Animator animator;
    public PlayerMovement movement;

    private bool isDead;
    private bool isInvincible;

    private Vector3 startPosition;

    public float invincibleTime = 0.2f;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (movement == null)
            movement = GetComponent<PlayerMovement>();

        if (healthUI == null)
            healthUI = FindObjectOfType<HealthUI>();
    }

    void Start()
    {
        startPosition = transform.position;

        currentHealth = maxHealth;

        if (healthUI != null)
        {
            healthUI.SetMaxHearts(maxHealth);
            healthUI.UpdateHearts(currentHealth);
        }
        else
        {
            Debug.LogWarning("HealthUI NOT FOUND");
        }
    }

    void Update()
    {
        // 🔥 PRESS H TO TAKE DAMAGE
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    //changed to float not int, for precise enemy damage (won't effect hearts though) - Xi
    public void TakeDamage(float damage)
    {
        if (isDead || isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log("Player HP: " + currentHealth);

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(Invincibility());
    }

    public void Heal(float heal)
    {
        if (isDead || isInvincible) return;

        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log("Player HP: " + currentHealth);

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("PLAYER DEAD");

        if (movement != null)
            movement.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }

        if (animator != null)
        {
            animator.ResetTrigger("Hurt");
            animator.Play("Samurai die");
        }
    }

    public void Respawn()
    {
        isDead = false;

        currentHealth = maxHealth;

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        Vector3 respawnPosition =
            GameManager.Instance.GetRespawnPosition(startPosition);

        transform.position = respawnPosition;

        if (movement != null)
            movement.enabled = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;

            if (movement != null)
                rb.gravityScale = movement.baseGravity;
        }

        if (animator != null)
            animator.Play("Idle");
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}