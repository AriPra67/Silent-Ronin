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
        //Adding this for bug - Xi
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("DEATH TRIGGERED");


        Enemy_Patrol patrol = GetComponent<Enemy_Patrol>();

        if (patrol != null)
            patrol.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        //Adding this - Xi
        Enemy_Attack attack = GetComponentInChildren<Enemy_Attack>();

        if (attack != null)
        {
            attack.StopAttacking();
            attack.enabled = false;

        }

        Collider2D col = GetComponent<Collider2D>();

        if (col != null)
            col.enabled = false;

        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Death", 0, 0f);
        }

        Invoke(nameof(RemoveEnemy), 2f);
    }

    void RemoveEnemy()
    {
        Destroy(gameObject);
    }
}