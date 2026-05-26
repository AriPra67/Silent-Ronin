using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Image fadeImage;
    public float fadeTime = 1f;
    public string sceneToLoad = "Level 1";

    private bool isStarting;

    void Start()
    {
        Time.timeScale = 1f;

        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    public void PlayGame()
    {
        if (!isStarting)
        {
            StartCoroutine(FadeAndStart());
        }
    }

    IEnumerator FadeAndStart()
    {
        isStarting = true;

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
}