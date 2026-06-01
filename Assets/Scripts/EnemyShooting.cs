using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletpos;
    public Transform player;

    public float attackRange = 3f;
    public float shootCooldown = 2.5f;
    public float attackAnimationTime = 2f;

    private float timer;
    private Animator animator;
    private bool isAttacking;
    private bool hasFiredThisAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = shootCooldown;

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
                player = foundPlayer.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            timer += Time.deltaTime;

            if (timer >= shootCooldown && !isAttacking)
            {
                StartAttack();
            }
        }
        else
        {
            timer = shootCooldown;
        }
    }

    void StartAttack()
    {
        if (bullet == null)
        {
            Debug.LogError("Bullet missing");
            return;
        }

        if (bulletpos == null)
        {
            Debug.LogError("BulletPos missing");
            return;
        }

        timer = 0f;
        isAttacking = true;
        hasFiredThisAttack = false;

        if (animator != null)
            animator.SetBool("attack", true);

        CancelInvoke(nameof(StopAttack));
        Invoke(nameof(StopAttack), attackAnimationTime);
    }

    // Animation Event calls this at the exact release frame
    public void EnemyFireBullet()
    {
        if (!isAttacking) return;
        if (hasFiredThisAttack) return;

        if (bullet == null || bulletpos == null) return;

        hasFiredThisAttack = true;

        Instantiate(bullet, bulletpos.position, Quaternion.identity);

        Debug.Log("Enemy fired bullet from animation event");
    }

    void StopAttack()
    {
        isAttacking = false;
        hasFiredThisAttack = false;

        if (animator != null)
            animator.SetBool("attack", false);
    }
}