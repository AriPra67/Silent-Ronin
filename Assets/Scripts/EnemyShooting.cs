using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletpos;
    public Transform player;

    public float attackRange = 3f;
    public float shootCooldown = 2f;
    public float attackAnimationTime = 0.6f;

    private float timer;
    private Animator animator;
    private bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();

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
                timer = 0f;
                StartAttack();
            }
        }
        else
        {
            timer = 0f;

            if (isAttacking)
                StopAttack();
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

        isAttacking = true;

        if (animator != null)
            animator.SetBool("attack", true);

        CancelInvoke(nameof(StopAttack));
        Invoke(nameof(StopAttack), attackAnimationTime);
    }

    // Animation Event calls this
    public void EnemyFireBullet()
    {
        Debug.Log("EnemyFireBullet event happened");

        if (bullet == null || bulletpos == null) return;

        Instantiate(bullet, bulletpos.position, Quaternion.identity);
    }

    void StopAttack()
    {
        isAttacking = false;

        if (animator != null)
            animator.SetBool("attack", false);
    }
}