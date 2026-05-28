using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class DoorToLevel : MonoBehaviour
{
    public string sceneToLoad = "Level 2";

    public Image fadeImage;
    public float fadeTime = 1f;

    public GameObject doorPrompt;

    private bool playerNearby;
    private bool isTransitioning;

    void Start()
    {
        if (doorPrompt != null)
            doorPrompt.SetActive(false);

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    void Update()
    {
        if (playerNearby && !isTransitioning && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
    {
        isTransitioning = true;

        if (doorPrompt != null)
            doorPrompt.SetActive(false);

        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(timer / fadeTime);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerMovement>() != null)
        {
            playerNearby = true;

            if (doorPrompt != null)
                doorPrompt.SetActive(true);

            Debug.Log("Player entered door trigger");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerMovement>() != null)
        {
            playerNearby = false;

            if (doorPrompt != null)
                doorPrompt.SetActive(false);

            Debug.Log("Player left door trigger");
        }
    }
}