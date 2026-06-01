using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 moveOffset = new Vector3(3f, 0f, 0f);
    public float speed = 2f;

    [Header("Carry Settings")]
    public float topCheckTolerance = 0.15f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;

    private Rigidbody2D platformRb;
    private Collider2D platformCollider;

    private Rigidbody2D playerRb;
    private Collider2D playerCollider;
    private bool playerOnTop;

    private Vector2 lastPlatformPosition;

    void Awake()
    {
        platformRb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + moveOffset;
        targetPosition = endPosition;

        lastPlatformPosition = platformRb.position;
    }

    void FixedUpdate()
    {
        Vector2 newPosition = Vector2.MoveTowards(
            platformRb.position,
            targetPosition,
            speed * Time.fixedDeltaTime
        );

        platformRb.MovePosition(newPosition);

        Vector2 platformMovement = newPosition - lastPlatformPosition;

        if (playerOnTop && playerRb != null)
        {
            // Only carry the player while they are not jumping upward
            if (playerRb.linearVelocity.y <= 0.1f)
            {
                playerRb.position += platformMovement;
            }
        }

        lastPlatformPosition = newPosition;

        if (Vector2.Distance(platformRb.position, targetPosition) < 0.05f)
        {
            targetPosition = targetPosition == endPosition ? startPosition : endPosition;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponentInParent<PlayerMovement>();

        if (player == null)
            return;

        Rigidbody2D foundPlayerRb = player.GetComponent<Rigidbody2D>();
        Collider2D foundPlayerCollider = player.GetComponent<Collider2D>();

        if (foundPlayerRb == null || foundPlayerCollider == null)
            return;

        float playerBottom = foundPlayerCollider.bounds.min.y;
        float platformTop = platformCollider.bounds.max.y;

        bool isStandingOnTop = playerBottom >= platformTop - topCheckTolerance;

        if (isStandingOnTop)
        {
            playerRb = foundPlayerRb;
            playerCollider = foundPlayerCollider;
            playerOnTop = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponentInParent<PlayerMovement>();

        if (player != null)
        {
            playerRb = null;
            playerCollider = null;
            playerOnTop = false;
        }
    }
}