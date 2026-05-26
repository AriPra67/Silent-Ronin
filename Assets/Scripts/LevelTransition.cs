using UnityEngine;
using UnityEngine.SceneManagement; // Crucial for loading scenes!

public class LevelTransition : MonoBehaviour
{
    public string nextSceneName; // Type the exact name of Level 2 here

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset the checkpoint memory so the player doesn't spawn in mid-air in Level 2!
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetCheckpointForNewLevel();
            }

            // Load the next level
            SceneManager.LoadScene(nextSceneName);
        }
    }
}