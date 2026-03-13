using System;
using System.Collections.Generic;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    public int Index;

    [SerializeReference] public List<GameObject> Enemies = new List<GameObject>();
    void Start()
    {
        GameController.Controller.Spawner_Ref = this;
    }

    [ContextMenu("Spawn")]

    public void Spawn()
    {
        Spawn(Index);
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


        GameObject.Instantiate(Enemies[index]);
    }

}
