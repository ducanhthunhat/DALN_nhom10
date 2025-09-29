using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float lives;
    public float damage;
    public float minSpeed;
    public float maxSpeed;
    public float resourceReward;
}
