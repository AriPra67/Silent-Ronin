using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;
    public GameOverManager gameOverManager; 

    private Vector2 spawnPoint;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Record the starting position at the very beginning
        spawnPoint = transform.position;

        if (healthUI != null)
        {
            healthUI.SetMaxHearts(maxHealth);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            TakeDamage(1);
        }
    }

    public void Respawn()
    {
        // Move player back to start
        transform.position = spawnPoint;
        
        // Reset health logic
        currentHealth = maxHealth;
        
        // Update the UI hearts
        if (healthUI != null)
        {
            healthUI.UpdateHearts(currentHealth);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthUI != null)
        {
            healthUI.UpdateHearts(currentHealth);
        }

        if (currentHealth <= 0)
        {
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOver();
            }
        }
    }
}