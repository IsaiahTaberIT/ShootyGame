using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "WaveSpawnPattern", menuName = "Scriptable Objects/WaveSpawnPattern")]
public class WaveSpawnPattern : ScriptableObject
{
    public int EnemyCount;
    /// <summary>
    /// SpawnPositions are in a range 0-1 on the top of the screen
    /// </summary>
    public List<float> SpawnPositions;
    public float SpawnMinBounds;
    public float SpawnMaxBounds;








    private void OnValidate()
    {
        GenerateSpawnPositions();
    }
    void GenerateSpawnPositions()
    {
        SpawnPositions.Clear();
        for (int i = 0; i < EnemyCount; i++)
        {
            SpawnPositions.Add(Mathf.Lerp(SpawnMinBounds, SpawnMaxBounds, i / (float)(EnemyCount - 1)));
        }



    }
}
