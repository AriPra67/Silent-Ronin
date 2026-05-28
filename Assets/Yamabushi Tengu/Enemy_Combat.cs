using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public float detectionRange = 3f;
    public float attackRange = 1f;
    public float damage = 1f;
    public float attackRate = 1f;
    public LayerMask targetLayer;

    private Transform currentTarget;
    private float lastAttackTime;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform visual;
    private Enemy_Patrol patrol;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        visual = anim.transform;
        patrol = GetComponent<Enemy_Patrol>();
    }

    void Update()
    {
        DetectTarget();

        if (currentTarget != null)
        {
            patrol.isOverridden = true;
            HandleChaseAndAttack();
        }
        else
        {
            patrol.isOverridden = false;
        }
    }

    void DetectTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRange, targetLayer);

        if (hit != null)
            currentTarget = hit.transform;
        else
            currentTarget = null;
    }

    void HandleChaseAndAttack()
    {
        float distance = Vector2.Distance(transform.position, currentTarget.position);
        Vector2 direction = (currentTarget.position - transform.position).normalized;

        if (direction.x < 0 && visual.localScale.x > 0 || direction.x > 0 && visual.localScale.x < 0)
        {
            visual.localScale = new Vector3(visual.localScale.x * -1, visual.localScale.y, visual.localScale.z);
        }

        if (distance > attackRange)
        {
            rb.linearVelocity = direction * patrol.speed;
            anim.Play("Walk");
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (Time.time >= lastAttackTime + attackRate)
            {
                anim.Play("attack1");
                lastAttackTime = Time.time;

                var health = currentTarget.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}