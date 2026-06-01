using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    public float delayBeforeDisappear = 1f;
    public float respawnTime = 3f;

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    private bool isRunning;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRunning) return;

        if (collision.gameObject.GetComponentInParent<PlayerMovement>() != null)
        {
            StartCoroutine(DisappearRoutine());
        }
    }

    IEnumerator DisappearRoutine()
    {
        isRunning = true;

        yield return new WaitForSeconds(delayBeforeDisappear);

        spriteRenderer.enabled = false;
        platformCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        spriteRenderer.enabled = true;
        platformCollider.enabled = true;

        isRunning = false;
    }
}