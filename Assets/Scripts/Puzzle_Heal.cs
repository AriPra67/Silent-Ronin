using System.Collections;
using UnityEngine;

public class Puzzle_Heal: MonoBehaviour
{
    public float heal = 0.2f;
    public float healInterval = 10f;
    public float healPauseDuration = 5f;

    private Rigidbody2D enemyRb;
    private Animator anim;

    private Transform playerTarget;
    private Coroutine healRoutine;
    private bool isDead;

    void Start()
    {
        enemyRb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerTarget = other.transform;

        if (healRoutine == null)
            healRoutine = StartCoroutine(HealLoop());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerTarget = null;

        if (healRoutine != null)
        {
            StopCoroutine(healRoutine);
            healRoutine = null;
        }

    }

    IEnumerator HealLoop()
    {
        while (playerTarget != null)
        {

            PlayerHealth playerHealth =
                playerTarget.GetComponent<PlayerHealth>();

            if (playerHealth != null && !isDead)
            {
                playerHealth.Heal(heal);
                Debug.Log("Healing!");
            }

            yield return new WaitForSeconds(healInterval);
        }

        healRoutine = null;
    }

}