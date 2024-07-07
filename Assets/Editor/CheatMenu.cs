using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;

public static class CheatMenu 
{
    [MenuItem("Cheats/PreviousScene")]
    public static void PreviousScene()
    {
        ChangeScene(0);
    }

    [MenuItem("Cheats/NextScene")]
    public static void NextScene()
    {
        ChangeScene(1);
    }
    
    private static void ChangeScene(int index)
    {
        if (index == 0)
        {
            int currentSceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
            int previousSceneIndex = currentSceneIndex - 1;

            if (previousSceneIndex >= 0 && previousSceneIndex < EditorSceneManager.sceneCountInBuildSettings)
            {
                EditorSceneManager.LoadScene(previousSceneIndex);
            }
            else
            {
                Debug.LogWarning("No previous scene available.");
            }
        }
        else if (index == 1) 
        {
            int currentSceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < EditorSceneManager.sceneCountInBuildSettings)
            {
                EditorSceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("No next scene available.");
            }
        }

    }
    
}
