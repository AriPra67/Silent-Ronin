using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public PlayerHealth player;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        Debug.Log("SHOW GAME OVER CALLED");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            Debug.LogWarning("GameOverPanel not assigned");
    }

    public void PressRespawn()
    {
        if (player != null)
            player.Respawn();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
}