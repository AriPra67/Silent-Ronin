using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 1;

    private EnemyAI2 ai;
    private bool playerInside;

    void Awake()
    {
        ai = GetComponentInParent<EnemyAI2>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            playerInside = true;

            ai?.SetPlayerInHitbox(true);

            player.TakeDamage(damage);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            if (!playerInside)
            {
                playerInside = true;
                ai?.SetPlayerInHitbox(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            playerInside = false;

            ai?.SetPlayerInHitbox(false);
        }
    }
}