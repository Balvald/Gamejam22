using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject creditCanvas;
    public GameObject storyCanvas;
    public GameObject playButton;
    public GameObject storyButton;
    public GameObject creditsButton;
    public GameObject exitButton;
    public GameObject title;

    void Start()
    {
        creditCanvas.SetActive(false);
        storyCanvas.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("We're done here");
    }

    public void CreditsButton()
    {
        creditCanvas.SetActive(true);
        playButton.SetActive(false);
        creditsButton.SetActive(false);
        exitButton.SetActive(false);
        storyButton.SetActive(false);
    }

    public void storyScreenButton()
    {
        title.SetActive(false);
        playButton.SetActive(false);
        creditsButton.SetActive(false);
        exitButton.SetActive(false);
        storyButton.SetActive(false);
        storyCanvas.SetActive(true);
    }

    public void ReturnFromCreditsButton()
    {
        title.SetActive(true);
        storyCanvas.SetActive(false);
        creditCanvas.SetActive(false);
        playButton.SetActive(true);
        creditsButton.SetActive(true);
        exitButton.SetActive(true);
        storyButton.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
