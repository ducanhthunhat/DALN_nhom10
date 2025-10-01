using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int wavesToWin;
    public int startingResources;
    public int startingLives;

    public Vector2 initialSpawnPosition;
    public WaveData[] waves;
}
