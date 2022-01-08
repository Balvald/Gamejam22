using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject creditCanvas;
    public GameObject playButton;
    public GameObject creditsButton;
    public GameObject exitButton;

    void Start()
    {
        creditCanvas.SetActive(false);
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
    }

    public void ReturnFromCreditsButton()
    {
        creditCanvas.SetActive(false);
        playButton.SetActive(true);
        creditsButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
