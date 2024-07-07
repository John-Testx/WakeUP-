using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public string taskId;
    public bool taskDone;
    public string taskType;
    public string taskName;

    public static event Action<Task> OnTaskCreated;

    // Start is called before the first frame update
    void Start()
    {
        // Trigger the event when a task is created
        OnTaskCreated?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {
        IITask task = GetComponent<IITask>();
        if (task != null)
        {
            if (task.IsDone()) taskDone = true;
        }
    }
}
