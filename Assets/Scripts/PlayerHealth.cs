using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public HealthUI healthUI;
    public Animator animator;
    public PlayerMovement movement;

    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthUI != null)
            healthUI.SetMaxHearts(maxHealth);
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Player Dead");

        if (movement != null)
            movement.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (animator != null)
            animator.SetTrigger("Die");
    }
}