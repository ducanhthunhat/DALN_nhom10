using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerData")]
public class TowerData : ScriptableObject
{
    public float range;
    public float shootInterval;
    public float projectileSpeed;
    public float projectileDuration;
    public float projectileSize;
    public float damage;
    public int cost;
    public Sprite sprite;
    public GameObject prefab;
    public AudioClip attackSound;
}
