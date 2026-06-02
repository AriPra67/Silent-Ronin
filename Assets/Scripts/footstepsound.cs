using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepSource;

    public AudioClip runningGrass;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.2f);

    void Update()
    {
        bool moving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        bool grounded = IsGrounded();

        // Footsteps only play while moving AND grounded
        if (moving && grounded)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.clip = runningGrass;
                footstepSource.loop = true;
                footstepSource.Play();
            }
        }
        else
        {
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
    }

    bool IsGrounded()
    {
        if (groundCheck == null)
            return false;

        return Physics2D.OverlapBox(
            groundCheck.position,
            groundCheckSize,
            0f,
            groundLayer
        );
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}