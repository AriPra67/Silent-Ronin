using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public HealthUI healthUI;
    public Animator animator;
    public PlayerMovement movement;

    private bool isDead;
    private bool isInvincible;

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

    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player HP: " + currentHealth);

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (animator != null)
            animator.SetTrigger("Hurt");

        StartCoroutine(Invincibility());
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

    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}