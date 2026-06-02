using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    public HealthUI healthUI;
    public Animator animator;
    public PlayerMovement movement;
    public PlayerAttack playerAttack;
    public PlayerInput playerInput;

    private bool isDead;
    private bool isInvincible;

    private Vector3 startPosition;

    public float invincibleTime = 0.2f;
    public float gameOverDelay = 1.2f;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (movement == null)
            movement = GetComponent<PlayerMovement>();

        if (playerAttack == null)
            playerAttack = GetComponent<PlayerAttack>();

        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage(1);
    }

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

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("PLAYER DEAD");

        if (playerInput != null)
            playerInput.enabled = false;

        if (movement != null)
            movement.enabled = false;

        if (playerAttack != null)
            playerAttack.enabled = false;

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

        StartCoroutine(ShowGameOverAfterDelay());
    }

    IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(gameOverDelay);

        GameOverManager gameOver = FindFirstObjectByType<GameOverManager>();

        if (gameOver != null)
            gameOver.ShowGameOver();
        else
            Debug.LogWarning("GameOverManager not found");
    }

    public void Respawn()
    {
        StopAllCoroutines();

        isDead = false;
        isInvincible = false;

        currentHealth = maxHealth;

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        Vector3 respawnPosition = GameManager.Instance.GetRespawnPosition(startPosition);
        transform.position = respawnPosition;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;

            if (movement != null)
                rb.gravityScale = movement.baseGravity;
        }

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
            animator.Play("Idle");
        }

        if (movement != null)
            movement.enabled = true;

        if (playerAttack != null)
            playerAttack.enabled = true;

        if (playerInput != null)
            playerInput.enabled = true;
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}