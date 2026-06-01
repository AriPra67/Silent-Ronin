using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public int damage = 1;
    public bool attackMode = false;

    private HashSet<EnemyHealth> enemiesHitThisAttack = new HashSet<EnemyHealth>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryHit(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryHit(other);
    }

    private void TryHit(Collider2D other)
    {
        if (!attackMode)
            return;

        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy == null)
            return;

        if (enemiesHitThisAttack.Contains(enemy))
            return;

        enemiesHitThisAttack.Add(enemy);
        enemy.TakeDamage(damage);
    }

    public void StartAttack()
    {
        attackMode = true;
        enemiesHitThisAttack.Clear();
    }

    public void EndAttack()
    {
        attackMode = false;
        enemiesHitThisAttack.Clear();
    }
}