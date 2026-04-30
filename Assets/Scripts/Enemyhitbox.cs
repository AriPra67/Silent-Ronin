using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // safer check (doesn't rely only on tag order)
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            Debug.Log("Player hit for " + damage);

            player.TakeDamage(damage);
        }
    }
}