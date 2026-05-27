using System.Collections;
using UnityEngine;

public class Enemy_Pursuit : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 3f;
    public float lostPlayerIdleTime = 2f;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform visual;

    private Enemy_Patrol patrol;

    private bool isChasing;
    private Coroutine lostRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        patrol = GetComponent<Enemy_Patrol>();

        anim = GetComponentInChildren<Animator>();
        visual = anim.transform;
    }

    void Update()
    {
        if (!isChasing)
            return;

        if (player == null)
            return;

        patrol.isOverridden = true;

        float dir = Mathf.Sign(player.position.x - transform.position.x);

        // Flip sprite
        if (dir < 0 && visual.localScale.x > 0 ||
            dir > 0 && visual.localScale.x < 0)
        {
            visual.localScale = new Vector3(
                visual.localScale.x * -1,
                visual.localScale.y,
                visual.localScale.z
            );
        }

        rb.linearVelocity = new Vector2(dir * chaseSpeed, 0f);

        anim.Play("Walk");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        player = collision.transform;

        isChasing = true;

        if (lostRoutine != null)
            StopCoroutine(lostRoutine);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        isChasing = false;

        rb.linearVelocity = Vector2.zero;

        lostRoutine = StartCoroutine(LosePlayerRoutine());
    }

    IEnumerator LosePlayerRoutine()
    {
        anim.Play("Idle");

        yield return new WaitForSeconds(lostPlayerIdleTime);

        patrol.isOverridden = false;
    }
}