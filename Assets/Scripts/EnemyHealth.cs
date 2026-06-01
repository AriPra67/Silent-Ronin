using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public Animator animator;

[Header("Death Position Fix")]
public float deathYOffset = 0f;

    [Header("Boss UI")]
    public BossHealthUI bossHealthUI;

    [Header("Drops")]
    public GameObject heartDropPrefab;

    [Range(0f, 1f)]
    public float heartDropChance = 0.35f;

    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (bossHealthUI != null)
        {
            bossHealthUI.SetMaxHearts(maxHealth);
            bossHealthUI.UpdateHearts(currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Enemy HP: " + currentHealth);

        if (bossHealthUI != null)
        {
            bossHealthUI.UpdateHearts(currentHealth);
        }

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
if (deathYOffset != 0f)
{
    transform.position += new Vector3(0f, deathYOffset, 0f);
}

        Debug.Log("DEATH TRIGGERED");

        Enemy_Patrol patrol = GetComponent<Enemy_Patrol>();

        if (patrol != null)
            patrol.enabled = false;

        BossAI bossAI = GetComponent<BossAI>();

        if (bossAI != null)
            bossAI.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

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
            animator.SetTrigger("Die");
        }

        Invoke(nameof(RemoveEnemy), 2f);
    }

    void TryDropHeart()
    {
        if (heartDropPrefab == null)
            return;

        float roll = Random.value;

        if (roll <= heartDropChance)
        {
            Instantiate(heartDropPrefab, transform.position, Quaternion.identity);
        }
    }

    void RemoveEnemy()
    {
        TryDropHeart();
        Destroy(gameObject);
    }
}