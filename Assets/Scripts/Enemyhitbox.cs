using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 1;

    [Header("Only melee enemies should use this")]
    public bool canDamagePlayer = true;

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

            if (canDamagePlayer)
            {
                player.TakeDamage(damage);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            playerInside = true;
            ai?.SetPlayerInHitbox(true);
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