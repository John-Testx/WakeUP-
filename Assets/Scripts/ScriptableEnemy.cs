using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ScriptableEnemy : ScriptableObject
{
    public enum EnemyType
    {
        None,
        Melee,
        Shooting,
        Flying
    }

    public int health;
    public float attackRange;
    public float attackSpeed;
    public EnemyType enemyType;
    public float hoverHeight;
    public float flySpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
