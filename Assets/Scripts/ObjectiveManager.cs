using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    [SerializeField] private int chapter;
    [SerializeField] private UIText UIText;
    [SerializeField] private int currentObjectiveIndex = 0;
    [SerializeField] private Objective currObjective;
    [SerializeField] private List<string> objectivesText = new();

    [SerializeField] private List<Objective> objectives = new();

    public static event Action ChangeObjective;
    public static event Action NextObjectiveEvent;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        CheckObjectives();

        if (objectives != null )
        {
            foreach(Objective obj in objectives) 
            {
                objectivesText.Add(obj.objectiveInfo);
            }

            currObjective = objectives[currentObjectiveIndex];
        }
    }

    void CheckObjectives()
    {

        foreach (Objective obj in GetComponentsInChildren<Objective>())
        {
            objectives.Add(obj);
            obj.manager = this;
        }
    }

    void Start()
    {
        UIText.ChangeObjectiveMessage(objectivesText[currentObjectiveIndex]);
        NextObjectiveEvent += NextObjective;
    }

    private void Update()
    {

    }

    public static void InvokeNextObjectiveEvent()
    {
        NextObjectiveEvent?.Invoke();
    }

    public void NextObjective()
    {
        currentObjectiveIndex++;
        if (currentObjectiveIndex < objectives.Count && currObjective.completed)
        {
            currObjective = objectives[currentObjectiveIndex];
        }

        UIText.ChangeObjectiveMessage(objectivesText[currentObjectiveIndex]);
        //Debug.Log($"Current objective {objectivesText[currentObjective]}");
    }

}
