using System.Collections;
using UnityEngine;

public class Enemy_Patrol : MonoBehaviour
{
    private int currentPatrolIndex;
    private Vector2 target;
    private bool isPaused;

    public Vector2[] patrolPoints;
    public float speed = 2f;
    public float pauseDuration = 1.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform visual;

    private bool isSettingPoint;

    [HideInInspector] public bool isOverridden;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        visual = anim.transform;

        if (patrolPoints.Length > 0)
            target = patrolPoints[0];

        StartCoroutine(SetPatrolPoint());
    }

    void Update()
    {
        if (isOverridden)
            return;

        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float dir = Mathf.Sign(target.x - transform.position.x);

        if (dir < 0 && visual.localScale.x > 0 ||
            dir > 0 && visual.localScale.x < 0)
        {
            visual.localScale = new Vector3(
                visual.localScale.x * -1,
                visual.localScale.y,
                visual.localScale.z
            );
        }

        rb.linearVelocity = new Vector2(dir * speed, 0f);

        if (Mathf.Abs(transform.position.x - target.x) < 0.1f &&
            !isSettingPoint)
        {
            StartCoroutine(SetPatrolPoint());
        }
    }

    IEnumerator SetPatrolPoint()
    {
        isSettingPoint = true;

        isPaused = true;

        rb.linearVelocity = Vector2.zero; 

        anim.Play("Idle");

        yield return new WaitForSeconds(pauseDuration);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        target = patrolPoints[currentPatrolIndex];

        isPaused = false;

        anim.Play("Walk");

        isSettingPoint = false;
    }
}