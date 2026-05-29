using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector]
    public Vector3 lastCheckpointPosition;
    private bool hasReachedCheckpoint = false;

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
    }

    public Vector3 GetRespawnPosition(Vector3 defaultStartPosition)
    {
        // If the player hasn't hit a checkpoint yet, use the level's default start
        return hasReachedCheckpoint ? lastCheckpointPosition : defaultStartPosition;
    }

    public void ResetCheckpointForNewLevel()
    {
        hasReachedCheckpoint = false;
    }
}