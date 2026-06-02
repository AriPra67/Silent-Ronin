using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject gameOverPanel;
    public PlayerHealth player;

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void PressRespawn()
    {
        player.Respawn();
        gameOverPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            PressRespawn();
        }
    }
}