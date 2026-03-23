using System;
using System.Collections.Generic;
using UnityEngine;
using static Logic;
public class Spawner : MonoBehaviour
{

    public Timer SpawnTimer = new(0.5f,0,false);
    public int SpawnIndex;


    [SerializeReference] public List<GameObject> Enemies = new List<GameObject>();

    private void Awake()
    {
        GameController.Controller.Spawner_Ref = this;

    }


    void OnEnable()
    {
        SpawnTimer.OnLoop += Spawn;
    }





    private void OnDisable()
    {
        SpawnTimer.OnLoop -= Spawn;

    }




    private void Update()
    {
        SpawnTimer.Step();
    }
    [ContextMenu("Spawn")]

    public void Spawn()
    {
        SpawnIndex = UnityEngine.Random.Range(0, Enemies.Count);





        Spawn(SpawnIndex);
    }

    public void Spawn(int index)
    {
        if (index >= Enemies.Count || index < 0)
        {
            Debug.Log("Index is outside of bounds of collection");
            return;
        }
        else if (Enemies[0] == null)
        {
            Debug.Log("reference at Index is null");

        }


        float xSpawnPos = UnityEngine.Random.Range(0f, 1f) + UnityEngine.Random.Range(0f, 1f);

        xSpawnPos /= 2f;




         Vector3 SpawnPos = GameController.Controller.Bounds.PlayArea.NormalToSurface(new Vector3(xSpawnPos, 0, 1));





        GameObject.Instantiate(Enemies[index], SpawnPos, Quaternion.identity);
    }

}
