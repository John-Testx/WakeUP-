using System.Collections;
using UnityEngine;


public class StartMenu : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject optionsPanel;
    public GameObject levelsPanel;
    public Camera levelsCamera;
    public GameObject startPanel;
    public Camera optionsCamera;
    public LevelManager levelManager;

    public void StartGame()
    {
        levelManager.LoadStartLevel();
    }

    public void ToggleLevelsMenu(bool show)
    {
        mainCamera.SetActive(!show);
        startPanel.SetActive(!show);
        levelsCamera.gameObject.SetActive(show);
        levelsPanel.SetActive(show);
    }

    public void ToggleOptionsMenu(bool show)
    {
        mainCamera.SetActive(!show);
        optionsPanel.SetActive(show);
        optionsCamera.gameObject.SetActive(show);
        startPanel.SetActive(!show);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
