using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public HealthUI healthUI;

    public GameOverManager gameOverManager; 

    void Start()
    {
        currentHealth = maxHealth;
        
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

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthUI != null)
        {
            healthUI.UpdateHearts(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player Dead");
            
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOver();
            }
            else
            {
                Debug.LogWarning("GameOverManager is missing! Check the slot on the Player object.");
            }
        }
    }
}