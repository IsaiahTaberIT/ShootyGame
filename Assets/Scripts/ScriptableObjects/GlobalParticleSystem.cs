using System;
using UnityEngine;
using static Logic;

[CreateAssetMenu(fileName = "GlobalParticleSystem", menuName = "Scriptable Objects/GlobalParticleSystem")]
public class GlobalParticleSystem : ScriptableObject
{
    public static implicit operator ParticleSystem(GlobalParticleSystem p) => p.SceneInstance;


    public ParticleSystem PrefabInstance;
    [SerializeField] private ParticleSystem _SceneInstance = null;


    public ParticleSystem SceneInstance
    {
        get
        {
            if (_SceneInstance == null)
            {
                _SceneInstance = GameObject.Instantiate(PrefabInstance) as ParticleSystem;
                return _SceneInstance;
            }
            else
            {
                return _SceneInstance;
            }
        }

        set
        {
            _SceneInstance = value;
        }
    }






}
