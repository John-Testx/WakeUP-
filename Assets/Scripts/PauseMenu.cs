using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public Image backgroundImage;
    bool onPause = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerCamera.SetMoveMouse(onPause);
            onPause = !onPause; // Toggle onPause state
            backgroundImage.enabled = onPause; // Set background image enabled state based on onPause
            pauseMenu.SetActive(onPause); // Set menu active state based on onPause
            Cursor.lockState = onPause ? CursorLockMode.None : CursorLockMode.Locked; // Set cursor lock state based on onPause
            Cursor.visible = onPause; // Set cursor visibility based on onPause
        }

        if (optionsMenu.activeSelf == true && !onPause)
        {
            optionsMenu.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        PlayerCamera.SetMoveMouse(true);
    }
    
}
