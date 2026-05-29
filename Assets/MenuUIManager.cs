using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public GameObject usernamePanel;
    public TMP_InputField usernameInput;


    public void OpenUsernamePanel()
    {
        Debug.Log("Opening panel now");
        usernamePanel.SetActive(true);

        usernamePanel.transform.SetAsLastSibling();
        usernamePanel.transform.localScale = Vector3.one;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ConfirmUsername()
    {
        string username = usernameInput.text;

        if (!string.IsNullOrEmpty(username))
        {
            PlayerPrefs.SetString("Username", username);
            SceneManager.LoadScene("Tutorial");
        }
    }

    // Update is called once per frame
    public void CloseUsernamePanel()
    {
        usernamePanel.SetActive(false);
    }

    //Settings panel
    public GameObject settingsPanel;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    //When clicking on 'Exit', the game stops
    public void QuitGame()
    {
        Application.Quit();
    }
}
