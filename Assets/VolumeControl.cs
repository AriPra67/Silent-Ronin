using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    public AudioSource audioSource;

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        Debug.Log("Volume: " + volume);
    }
}
