using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{

    public bool IsGamePaused = false;

    public GameObject PauseMenuUI;
    public GameObject SaveMenuUI;
    public GameObject LoadMenuUI;
    public GameObject SirHandel;

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
            Time.timeScale = 0f;
        }
        else
        {
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
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
        Time.timeScale = 1f;
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

    // Looks like crap but works. 4 Slots are more than enought for this simple game.
    public void SaveButton1()
    {
        string slot = "slot1.json";
        SirHandel.GetComponent<GameDataHandler>().switchSaveSlot(slot);
        SirHandel.GetComponent<GameDataHandler>().writeFile();
    }

    public void SaveButton2()
    {
        string slot = "slot2.json";
        SirHandel.GetComponent<GameDataHandler>().switchSaveSlot(slot);
        SirHandel.GetComponent<GameDataHandler>().writeFile();
    }

    public void SaveButton3()
    {
        string slot = "slot3.json";
        SirHandel.GetComponent<GameDataHandler>().switchSaveSlot(slot);
        SirHandel.GetComponent<GameDataHandler>().writeFile();
    }

    public void SaveButton4()
    {
        string slot = "slot4.json";
        SirHandel.GetComponent<GameDataHandler>().switchSaveSlot(slot);
        SirHandel.GetComponent<GameDataHandler>().writeFile();
    }

    public void LoadButton1()
    {
        string slot = "slot1.json";
        SirHandel.GetComponent<GameDataHandler>().switchSaveSlot(slot);
        SirHandel.GetComponent<GameDataHandler>().readFile();
    }

    public void LoadButton2()
    {
        string slot = "slot2.json";
        SirHandel.GetComponent<GameDataHandler>().switchSaveSlot(slot);
        SirHandel.GetComponent<GameDataHandler>().readFile();
    }

    public void LoadButton3()
    {
        string slot = "slot3.json";
        SirHandel.GetComponent<GameDataHandler>().switchSaveSlot(slot);
        SirHandel.GetComponent<GameDataHandler>().readFile();
    }

    public void LoadButton4()
    {
        string slot = "slot4.json";
        SirHandel.GetComponent<GameDataHandler>().switchSaveSlot(slot);
        SirHandel.GetComponent<GameDataHandler>().readFile();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
