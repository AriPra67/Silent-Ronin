using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true); // Shows the screen
        Time.timeScale = 0f; // Freezes the game movement
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Unfreezes time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}