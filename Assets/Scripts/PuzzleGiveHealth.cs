using UnityEngine;

public class PuzzleGiveHealth : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public int maxUses = 2;
    public float healAmount = 1f;

    private int currentUses;
    private bool isDestroyed = false;

    void Start()
    {
        currentUses = maxUses;
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;

        currentUses -= damage;

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Debug.Log($"Puzzle object healed player for {healAmount} HP!");
        }
        else
        {
            Debug.LogWarning("PlayerHealth not found in scene to heal!");
        }

        Debug.Log("Puzzle remaining uses: " + currentUses);

        if (currentUses <= 0)
        {
            Vanish();
        }
    }

    void Vanish()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        Debug.Log("Puzzle object used up! Vanishing.");

        Collider2D col = GetComponentInChildren<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject);
    }
}