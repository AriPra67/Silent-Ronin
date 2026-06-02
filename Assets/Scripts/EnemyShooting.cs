using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletpos;
    public Transform player;

    public float attackRange = 3f;
    public float shootCooldown = 2.5f;
    public float attackAnimationTime = 2f;

    [Header("Facing")]
    public bool startsFacingRight = true;

    private float timer;
    private Animator animator;
    private bool isAttacking;
    private bool hasFiredThisAttack;
    private bool shootingStopped;

    private int facingDirection = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = shootCooldown;

        facingDirection = startsFacingRight ? 1 : -1;

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
                player = foundPlayer.transform;
        }
    }

    void Update()
    {
        if (shootingStopped)
            return;

        if (player == null)
            return;

        FacePlayer();

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

    void FacePlayer()
    {
        if (shootingStopped || player == null)
            return;

        float xDifference = player.position.x - transform.position.x;

        if (xDifference > 0f)
        {
            SetFacingDirection(1);
        }
        else if (xDifference < 0f)
        {
            SetFacingDirection(-1);
        }
    }

    void SetFacingDirection(int direction)
    {
        if (direction == facingDirection)
            return;

        facingDirection = direction;

        Vector3 scale = transform.localScale;

        if (startsFacingRight)
            scale.x = Mathf.Abs(scale.x) * facingDirection;
        else
            scale.x = Mathf.Abs(scale.x) * -facingDirection;

        transform.localScale = scale;
    }

    void StartAttack()
    {
        if (shootingStopped)
            return;

        if (bullet == null || bulletpos == null)
            return;

        FacePlayer();

        timer = 0f;
        isAttacking = true;
        hasFiredThisAttack = false;

        if (animator != null)
            animator.SetBool("attack", true);

        CancelInvoke(nameof(StopAttack));
        Invoke(nameof(StopAttack), attackAnimationTime);
    }

    public void EnemyFireBullet()
    {
        if (shootingStopped)
            return;

        if (!isAttacking)
            return;

        if (hasFiredThisAttack)
            return;

        if (bullet == null || bulletpos == null)
            return;

        hasFiredThisAttack = true;

        Instantiate(bullet, bulletpos.position, Quaternion.identity);
    }

    void StopAttack()
    {
        if (shootingStopped)
            return;

        isAttacking = false;
        hasFiredThisAttack = false;

        if (animator != null)
            animator.SetBool("attack", false);
    }

    public void StopShooting()
    {
        shootingStopped = true;
        isAttacking = false;
        hasFiredThisAttack = true;

        CancelInvoke(nameof(StopAttack));

        if (animator != null)
        {
            animator.SetBool("attack", false);
        }
    }

    void OnDisable()
    {
        CancelInvoke(nameof(StopAttack));
        shootingStopped = true;
    }
}