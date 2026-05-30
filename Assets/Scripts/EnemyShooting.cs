using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletpos;
    public Transform player;

    public float attackRange = 3f;
    public float shootCooldown = 2f;

    public float bulletDelay = 0.3f;
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
            StopAttack();
        }
    }

    void StartAttack()
    {
        if (bullet == null)
        {
            Debug.LogError("EnemyShooting: Bullet prefab is missing.");
            return;
        }

        if (bulletpos == null)
        {
            Debug.LogError("EnemyShooting: BulletPos is missing.");
            return;
        }

        isAttacking = true;

        if (animator != null)
            animator.SetBool("attack", true);

        Invoke(nameof(FireBullet), bulletDelay);
        Invoke(nameof(StopAttack), attackAnimationTime);
    }

    void FireBullet()
    {
        if (bullet != null && bulletpos != null)
            Instantiate(bullet, bulletpos.position, Quaternion.identity);
    }

    void StopAttack()
    {
        isAttacking = false;

        if (animator != null)
            animator.SetBool("attack", false);
    }
}