using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public bool completed;
    public ObjectiveManager manager;
    public string objectiveInfo;
    public Objective parentObjective;
    public List<Objective> subObjectives;
    private Collider collider;
    public int currentStage;
    public int amountOfStages;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();

        if (transform.parent.GetComponent<Objective>() != null )
        {
            parentObjective = transform.parent.GetComponent<Objective>();
        }

        foreach (Transform child in transform)
        {
            Objective childObjective = child.GetComponent<Objective>();

            if (childObjective != null)
            {
                subObjectives.Add(childObjective);
                amountOfStages++;
            }
        }

        ObjectiveManager.ChangeObjective += CheckObjectiveState;
    }

    void CheckObjectiveState()
    {
        if (parentObjective != null) { }
        
        else
        {
            
            //Debug.Log($"is Completed? : {completed}");
            ObjectiveManager.InvokeNextObjectiveEvent();
        }
    }

    private void OnDestroy()
    {
        ObjectiveManager.ChangeObjective -= CheckObjectiveState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!completed && other.CompareTag("Player"))
        {
            Debug.Log("Objective completed");
            completed = true;
            collider.enabled = false;
            CheckObjectiveState();
        }
        
    }
}
