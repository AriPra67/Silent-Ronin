using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;
    public Animator animator; // Optional: if you have a flag-raising animation

    void OnTriggerEnter2D(Collider2D other)
    {
        // Make sure it's the player touching the checkpoint
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            // Tell the GameManager to save this exact position
            GameManager.Instance.UpdateCheckpoint(transform.position);

            // Optional visual feedback
            if (animator != null)
            {
                animator.SetTrigger("Activate");
            }

            Debug.Log("Checkpoint Activated at: " + transform.position);
        }
    }
}