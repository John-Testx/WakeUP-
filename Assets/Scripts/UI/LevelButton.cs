using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public delegate void SceneLoadRequest(string sceneName);
    public static event SceneLoadRequest OnSceneLoadRequested;

    private string sceneName;

    public void SetSceneName(string name)
    {
        sceneName = name;
    }

    public void LoadScene()
    {
        if (OnSceneLoadRequested != null)
        {
            OnSceneLoadRequested?.Invoke(sceneName);
        }
        else
        {
            Debug.LogError("No listeners for scene load request!");
        }
    }

}
