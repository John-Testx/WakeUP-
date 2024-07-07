using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private LevelManager instance;
    public GameObject parentLevelObject;
    public GameObject levelObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public void LoadStartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void FillLevels()
    {

        List<string> options = new List<string>();

        Debug.Log(SceneManager.sceneCountInBuildSettings);

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            if (i > 0)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                GameObject buttonGO = Instantiate(levelObject, parentLevelObject.transform);
                buttonGO.name = sceneName;

                LevelButton levelButton = buttonGO.GetComponent<LevelButton>();
                levelButton.transform.localScale = Vector3.one;
                levelButton.SetSceneName(sceneName);

                TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = sceneName;

                Debug.Log($"Scene : {sceneName}");
                options.Add(sceneName);
            }
        }
    }

    private void Start()
    {
        FillLevels();
    }

    void OnEnable()
    {
        LevelButton.OnSceneLoadRequested += LoadSceneByName;
    }

    void OnDisable()
    {
        LevelButton.OnSceneLoadRequested -= LoadSceneByName;
    }

    public void LoadSceneByName(string sceneName)
    {
        string name = $"{SceneManager.GetSceneByName(sceneName)} ";
        Debug.Log($"Running: {name}");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
