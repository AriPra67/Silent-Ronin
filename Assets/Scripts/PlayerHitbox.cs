using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("HIT: " + other.name);

        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            Debug.Log("DAMAGE DEALT");
            enemy.TakeDamage(damage);
        }
        else
        {
            Debug.Log("NO EnemyHealth FOUND");
        }
    }
}