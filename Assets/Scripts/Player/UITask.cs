using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITask : MonoBehaviour
{
    public TextMeshProUGUI tasks;
    Dictionary<string, int> tasksToBeDone;
    string text;
    PlayerTask player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerTask>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTasks();
        TaskInfo();
    }

    void TaskInfo()
    {
        if (player != null)
        {
            //Debug.Log(player.GetTotalAmountOfTasks());
            text = TasksAsString();
            tasks.SetText(text);
        }
    }
    void currentTasks()
    {
        if (player != null)
        {
            tasksToBeDone = player.GetTasksDone();

        }
    }

    public string TasksAsString()
    {

        // Log and return the dictionary
        string tasksAsString = "";
        foreach (var pair in tasksToBeDone)
        {
            tasksAsString += $"\n {pair.Key} {pair.Value}";
        }
        tasksAsString = tasksAsString.TrimEnd(',', ' '); // Remove trailing comma and space
        tasksAsString += "";

        //Debug.Log(tasksAsString);
        return tasksAsString;
    }

}
