using System.Collections;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    public float damage = 0.2f;
    public float attackInterval = 2f;
    public float attackPauseDuration = 0.5f;

    private Enemy_Patrol patrol;
    private Rigidbody2D enemyRb;
    private Animator anim;

    private Transform playerTarget;
    private Coroutine attackRoutine;
    private bool isDead;

    void Start()
    {
        patrol = GetComponentInParent<Enemy_Patrol>();
        enemyRb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();

        if (anim == null)
            anim = GetComponentInParent<Animator>(true);

        Debug.Log(anim);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerTarget = other.transform;

        if (attackRoutine == null)
            attackRoutine = StartCoroutine(AttackLoop());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerTarget = null;

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        patrol.isOverridden = false;

        anim.Play("Walk");
    }

    IEnumerator AttackLoop()
    {
        while (playerTarget != null)
        {
            patrol.isOverridden = true;

            enemyRb.linearVelocity = Vector2.zero;

            anim.Play("Idle");

            yield return new WaitForSeconds(attackPauseDuration);

            anim.Play("Attack");

            PlayerHealth playerHealth =
                playerTarget.GetComponent<PlayerHealth>();

            if (playerHealth != null && !isDead)
            {
                playerHealth.TakeDamage(damage);
            }

            yield return new WaitForSeconds(attackInterval);
        }

        attackRoutine = null;
    }

    public void StopAttacking()
    {
        isDead = true;

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }
}