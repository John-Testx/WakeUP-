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
        Shooting
    }

    public int health;
    public float attackRange;
    public float attackSpeed;
    public EnemyType enemyType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
