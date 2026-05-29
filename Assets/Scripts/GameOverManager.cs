using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth player;

    public void PressRespawn()
    {
        player.Respawn();     
        gameObject.SetActive(false);
    }
}