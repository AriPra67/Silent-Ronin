using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (pauseUI != null)
            pauseUI.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        PlayerMovement movement = FindObjectOfType<PlayerMovement>();

        if (movement != null)
        {
            movement.ResetMovement();
        }
    }

    void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // RETURN TO MAIN MENU
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameMenu");
    }
}