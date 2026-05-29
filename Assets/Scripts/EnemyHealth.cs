using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public Animator animator;

    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("DEATH TRIGGERED");

        // STOP EVERYTHING
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null)
            ai.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false; // FULL physics stop (no falling)
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // FORCE animation (bypasses Animator logic completely)
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Death", 0, 0f);
        }

        // delay destroy so you can SEE it
        Invoke(nameof(RemoveEnemy), 2f);
    }

    void RemoveEnemy()
    {
        Destroy(gameObject);
    }
}