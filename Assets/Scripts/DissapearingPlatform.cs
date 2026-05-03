using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    public float delayBeforeDisappear = 1f;
    public float respawnTime = 3f;

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DisappearRoutine());
        }
    }

    IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(delayBeforeDisappear);

        spriteRenderer.enabled = false;
        platformCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        spriteRenderer.enabled = true;
        platformCollider.enabled = true;
    }
}