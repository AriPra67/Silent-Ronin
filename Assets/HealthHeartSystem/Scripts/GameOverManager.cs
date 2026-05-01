using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public PlayerHealth playerHealth;

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; 
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RespawnButton()
    {
        // Hide the screen and unfreeze game
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;

        // Tell the player to reset themselves
        if (playerHealth != null)
        {
            playerHealth.Respawn();
        }

        // Re-hide the cursor 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}