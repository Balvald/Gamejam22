using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{

    public bool IsGamePaused = false;

    public GameObject PauseMenuUI;
    public GameObject SaveMenuUI;
    public GameObject LoadMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenuUI.SetActive(false);
        SaveMenuUI.SetActive(false);
        LoadMenuUI.SetActive(false);
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

    public void SaveMenuButton()
    {
        // Going to Save Menu
        PauseMenuUI.SetActive(false);
        SaveMenuUI.SetActive(true);
    }

    public void LoadMenuButton()
    {
        // Going to Load Menu
        PauseMenuUI.SetActive(false);
        LoadMenuUI.SetActive(true);
    }

    public void SaveLoadReturnButton()
    {
        //returning either from Save or Load Menu
        SaveMenuUI.SetActive(false);
        LoadMenuUI.SetActive(false);
        PauseMenuUI.SetActive(true);
    }

    public void SaveButton()
    {
        //Save current state of game with name in associated input field
    }

    public void LoadButton()
    {
        //Attempt to load gamefile with name in associated input field

    }

    // Update is called once per frame
    void Update()
    {

    }
}
