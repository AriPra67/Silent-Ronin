using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;

    [Header("UI Feedback")]
    public TextMeshProUGUI notificationText;
    public float displayDuration = 1.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            GameManager.Instance.UpdateCheckpoint(transform.position);

            if (notificationText != null)
            {
                notificationText.gameObject.SetActive(true);

                Invoke("HideNotification", displayDuration);
            }

            Debug.Log("Checkpoint Activated!");
        }
    }

    void HideNotification()
    {
        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false);
        }
    }
}