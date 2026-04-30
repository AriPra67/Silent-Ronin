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
    public float invincibleTime = 1f;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthUI != null)
            healthUI.SetMaxHearts(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible) return;

        currentHealth -= damage;

        Debug.Log("Player HP: " + currentHealth);

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invincibility());
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

    System.Collections.IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}