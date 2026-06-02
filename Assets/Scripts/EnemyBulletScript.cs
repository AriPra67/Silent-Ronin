using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public float force = 5f;
    public int damage = 1;

    [Header("Destroy On Hit")]
    public LayerMask destroyOnLayers;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && rb != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;

            rb.linearVelocity = direction * force;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (((1 << other.gameObject.layer) & destroyOnLayers) != 0)
        {
            Destroy(gameObject);
        }
    }
}