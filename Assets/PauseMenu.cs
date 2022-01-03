using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public bool IsGamePaused = false;

    public GameObject PauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenuUI.SetActive(false);
    }

    void OnEscapeKey()
    {
        IsGamePaused = !IsGamePaused;
        if (IsGamePaused)
        {
            PauseMenuUI.SetActive(true);
        }
        else
        {
            PauseMenuUI.SetActive(false);
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ResumeButton()
    {
        // IsGamePaused should be true here!
        IsGamePaused = !IsGamePaused;
        PauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
