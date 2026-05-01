using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ENEMY HITBOX TOUCHED: " + other.name);

        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            Debug.Log("PLAYER FOUND - DAMAGE");
            player.TakeDamage(damage);
        }
        else
        {
            Debug.Log("NO PlayerHealth found on " + other.name);
        }
    }
}