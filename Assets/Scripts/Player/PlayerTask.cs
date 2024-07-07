using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTask : MonoBehaviour
{
    public int tasksToBeDone;
    private List<Task> tasks = new List<Task>();
    public Dictionary<string, List<Task>> taskListsByType;

    // Start is called before the first frame update
    void Start()
    {        
        tasks = FindObjectsOfType<Task>().ToList();
        taskListsByType = new Dictionary<string, List<Task>>();
        Task.OnTaskCreated += AddTaskToListByType;

        foreach(Task task in tasks)
        {
            AddTaskToListByType(task);
        }
        
        //GetTotalAmountOfTasks();
    }

    // Update is called once per frame
    void Update()
    {
        //GetTasksDone();
        tasksToBeDone = GetTotalAmountOfTasks();
        
        
        CheckTasks();
        
    }

    void CheckTasks()
    {
        //Iterate over specific types or all types of tasks
        foreach (var taskList in taskListsByType.Values)
        {
            for (int i = taskList.Count - 1; i >= 0; i--)
            {
                IITask task = taskList[i].GetComponent<IITask>();

                if (task != null && task.IsDone())
                {
                    taskList.RemoveAt(i);
                }
            }
        }
    }

    public Dictionary<string, int> GetTasksDone()
    {
        Dictionary<string, int> tasks = new Dictionary<string, int>();

        if (taskListsByType != null) { 
        // Iterate over each task type
            foreach (var taskList in taskListsByType)
            {
                int counter = 0; // Reset counter for each task type

                // Iterate over tasks of current task type
                foreach (var task in taskList.Value)
                {
                    if (!task.taskDone)
                    {
                        counter++; // Increment counter for completed task
                    }
                }

                // Add task type and count to the dictionary
                tasks.Add(taskList.Key, counter);
            }
            
        }
        return tasks;
    }

    public int GetTotalAmountOfTasks()
    {
        
        int counter = 0;

        foreach (var taskList in taskListsByType.Values)
        {
            
            for (int i = taskList.Count - 1; i >= 0; i--)
            {
                counter++;
            }
        }

        return counter;
    }

    // Method to add task to the list based on type
    void AddTaskToListByType(Task newTask)
    {
        
        if (!taskListsByType.ContainsKey(newTask.taskType))
        {
            taskListsByType[newTask.taskType] = new List<Task>();
        }

        if (!taskListsByType[newTask.taskType].Contains(newTask))
        {
            taskListsByType[newTask.taskType].Add(newTask);
        }
    }

    private void OnDestroy()
    {
        Task.OnTaskCreated -= AddTaskToListByType;
    }

}
