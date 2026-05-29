using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public int damage = 1;
    public bool attackMode = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("HITBOX TOUCHED: " + other.name);

        if (!attackMode) return;

        EnemyHealth enemy = other.transform.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            Debug.Log("ENEMY HIT!");
            enemy.TakeDamage(damage);
            return;
        }

        PuzzleGiveHealth puzzle = other.transform.GetComponentInParent<PuzzleGiveHealth>();
        if (puzzle != null)
        {
            Debug.Log("PUZZLE OBJECT HIT!");
            puzzle.TakeDamage(damage);
            return;
        }
    }

    public void StartAttack()
    {
        attackMode = true;
        Debug.Log("ATTACK ON");
    }

    public void EndAttack()
    {
        attackMode = false;
        Debug.Log("ATTACK OFF");
    }
}