using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 3;
    public float invincibleTime = 0.2f;

    [Header("References")]
    public HealthUI healthUI;
    public Animator animator;
    public PlayerMovement movement;
    public GameObject gameOverUI;

    private int currentHealth;
    private bool isDead;
    private bool isInvincible;
    private Vector3 startPosition;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

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
        startPosition = transform.position;

        if (healthUI != null)
        {
            healthUI.SetMaxHearts(maxHealth);
            healthUI.UpdateHearts(currentHealth);
        }
    }

    void Update()
    {

        if (Time.timeScale == 0) return;

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

        if (movement != null)
            movement.enabled = false;

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

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        transform.position = startPosition;

        if (rb != null)
        {
            rb.simulated = true;
            rb.gravityScale = (movement != null) ? movement.baseGravity : 1f;
            rb.linearVelocity = Vector2.zero;
        }

        if (movement != null)
        {
            movement.enabled = true;
            movement.ResetMovement();
        }

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
            animator.Play("Idle");
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}