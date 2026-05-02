using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static Logic;
public class Spawner : MonoBehaviour
{

    public Timer SpawnTimer = new(0.5f,0,false);
    public int SpawnIndex;
    public WaveSpawnPattern SpawnPattern;

    public float LastSpawnTime = 0;

    public int SubWaveIndex = 0;
    public Timer SubWaveTimer = new(0.5f, 0, false);



    void SpawnSpawnPattern(WaveSpawnPattern w)
    {

        if (!GameController.Controller.CanHandleMoreEmemies(w.EnemyCount))
        {
            SpawnTimer.Time = 0;
            return;
        }

        if (Time.time - LastSpawnTime < (w.SubWaves[SubWaveIndex].SubWaveDurationWeight / w.TotalDurationWeights) * w.Duration)
        {
            SpawnTimer.Time = (w.SubWaves[SubWaveIndex].SubWaveDurationWeight / w.TotalDurationWeights) *w.Duration;
            Debug.Log("BelowMin");
            Debug.Log(Time.time - LastSpawnTime);

            return;
        }



        foreach (float pos in w.SubWaves[SubWaveIndex].SpawnPositions)
        {
            LastSpawnTime = Time.time;
            Spawn(SpawnIndex, pos);
        }

        SubWaveIndex++;

        SubWaveIndex %= w.SubWaves.Count;


    }





    [SerializeReference] public List<GameObject> EnemyTypes = new List<GameObject>();

    private void Awake()
    {
        GameController.Controller.Spawner_Ref = this;

    }

    [ContextMenu("re")]

    public void Re()
    {

    }




    void OnEnable()
    {
        GameController.OnFixedUpdateUnPaused += OnFixedUpdate;

        SpawnTimer.OnLoop += Spawn;
    }





    private void OnDisable()
    {
        GameController.OnFixedUpdateUnPaused -= OnFixedUpdate;

        SpawnTimer.OnLoop -= Spawn;

    }




    private void OnFixedUpdate()
    {
        SpawnTimer.Step(Time.fixedDeltaTime);
    }

    [ContextMenu("Spawn")]

    public void Spawn()
    {
        SpawnIndex = UnityEngine.Random.Range(0, EnemyTypes.Count);
        SpawnSpawnPattern(SpawnPattern);
    }


    public void Spawn(int index, float pos)
    {
        if (index >= EnemyTypes.Count || index < 0)
        {
            Debug.Log("Index is outside of bounds of collection");
            return;
        }
        else if (EnemyTypes[0] == null)
        {
            Debug.Log("reference at Index is null");
            return;
        }

        if (GameController.Controller.EnemyArrayFull)
        {
            Debug.Log("full");
            return;
        }



        Vector3 SpawnPos = GameController.Controller.Bounds.PlayArea.NormalToSurface(new Vector3(pos, 0, 1));

        GameController.Controller.TryAddEnemy(GameObject.Instantiate(EnemyTypes[index], SpawnPos, Quaternion.identity).GetComponent<Enemy>());
    }




    public void Spawn(int index)
    {
        float xSpawnPos = UnityEngine.Random.Range(0f, 1f) + UnityEngine.Random.Range(0f, 1f);

        xSpawnPos /= 2f;

        Spawn(index, xSpawnPos);
    }

}
