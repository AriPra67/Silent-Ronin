using UnityEngine;

public class BossWave : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float lifetime = 3f;

    [Header("Damage")]
    public int damage = 1;
    private bool hasHitPlayer;

    [Header("Visual Trail")]
    public bool useTrail = true;
    public float trailSpawnRate = 0.06f;
    public float trailLifetime = 0.35f;
    public float trailScaleShrink = 0.85f;
    public float trailAlpha = 0.45f;

    private int direction = 1;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float trailTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(int newDirection)
    {
        direction = newDirection;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }
        else
        {
            transform.position += Vector3.right * direction * speed * Time.fixedDeltaTime;
        }
    }

    void Update()
    {
        if (!useTrail)
            return;

        trailTimer += Time.deltaTime;

        if (trailTimer >= trailSpawnRate)
        {
            trailTimer = 0f;
            CreateTrailSprite();
        }
    }

    void CreateTrailSprite()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        GameObject trail = new GameObject("Wave Trail");

        trail.transform.position = transform.position;
        trail.transform.rotation = transform.rotation;
        trail.transform.localScale = transform.localScale * trailScaleShrink;

        SpriteRenderer trailRenderer = trail.AddComponent<SpriteRenderer>();
        trailRenderer.sprite = spriteRenderer.sprite;
        trailRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
        trailRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;

        Color color = spriteRenderer.color;
        color.a = trailAlpha;
        trailRenderer.color = color;

        Destroy(trail, trailLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDamagePlayer(other);
        TryDestroyOnWall(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamagePlayer(other);
    }

    void TryDamagePlayer(Collider2D other)
    {
        if (hasHitPlayer)
            return;

        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            hasHitPlayer = true;
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void TryDestroyOnWall(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            Destroy(gameObject);
        }
    }
}