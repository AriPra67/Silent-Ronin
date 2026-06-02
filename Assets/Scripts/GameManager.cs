using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector]
    public Vector3 lastCheckpointPosition;

    private bool hasReachedCheckpoint = false;
    private string checkpointSceneName = "";

    void Awake()
    {
        // Ensure there is only ever one GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it alive across scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCheckpoint(Vector3 newPosition)
    {
        lastCheckpointPosition = newPosition;
        hasReachedCheckpoint = true;

        // Save which scene this checkpoint belongs to
        checkpointSceneName = SceneManager.GetActiveScene().name;

        Debug.Log("Checkpoint saved in scene: " + checkpointSceneName);
    }

    public Vector3 GetRespawnPosition(Vector3 defaultStartPosition)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Only use the checkpoint if it belongs to the current scene
        if (hasReachedCheckpoint && checkpointSceneName == currentSceneName)
        {
            return lastCheckpointPosition;
        }

        // Otherwise use the player's starting position in this scene
        return defaultStartPosition;
    }

    public void ResetCheckpointForNewLevel()
    {
        hasReachedCheckpoint = false;
        checkpointSceneName = "";
    }
}