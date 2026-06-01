using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioSource jumpSource;

    public AudioClip runningGrass;
    public AudioClip jumpSound;

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
    }
}