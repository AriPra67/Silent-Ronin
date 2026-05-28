using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioSource jumpSource;
    public AudioSource attackSource;

    public AudioClip runningGrass;
    public AudioClip jumpSound;
    public AudioClip attackSound;

    void Update()
    {
        bool moving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        // Footsteps
        if (moving)
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
            footstepSource.Stop();
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpSource.PlayOneShot(jumpSound);
        }

        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            attackSource.PlayOneShot(attackSound);
        }
    }
}