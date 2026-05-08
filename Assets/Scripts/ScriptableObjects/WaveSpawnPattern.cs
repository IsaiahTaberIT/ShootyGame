using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
[CreateAssetMenu(fileName = "WaveSpawnPattern", menuName = "Scriptable Objects/WaveSpawnPattern")]
public class WaveSpawnPattern : ScriptableObject
{
    public SpawnPatternRenderer Renderer;
  




    public int EnemyCount;
    /// <summary>
    /// SpawnPositions are in a range 0-1 on the top of the screen
    /// </summary>
    public List<float> SpawnPositions;
    public float Duration = 1f;
    public float TotalDurationWeights;

    public List<SubWave> SubWaves;
    [System.Serializable]
    public class SubWave
    {
        public bool IsNonDeterministic => HasRandomLayer();
        public float SubWaveDurationWeight = 1;
        public List<WavePatternLayer> Layers;
        [SerializeField]
        public List<float> SpawnPositions;

        bool HasRandomLayer()
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                if (Layers[i].PatternType == WavePatternLayer.PatternTypes.RandomSubset)
                {
                    return true;
                }
            }

            return false;
        }


        public void GenerateSpawnPositions( )
        {

            List<float> tempSpawnPositions = new();

            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].Apply(ref tempSpawnPositions);
            }

            SpawnPositions = tempSpawnPositions;
        }
    }


    [ContextMenu("gen")]

    public void Gen()
    {
        TotalDurationWeights = 0;
        EnemyCount = 0;

        foreach (SubWave subWave in SubWaves)
        {
            subWave.GenerateSpawnPositions();


            TotalDurationWeights += subWave.SubWaveDurationWeight;
            EnemyCount += subWave.SpawnPositions.Count;
        }
    }

    [System.Serializable]
    public class WavePatternLayer
    {
        [SerializeField]
        [Range(0,1)]
        private float StartOffset = 0.5f;
        public float SpawnWidth = 1;
        public int Points = 5;

        public enum PatternTypes
        {
            Manual = 0,
            EvenlySpaced = 1,
            RandomSubset = 2,

        }

        public PatternTypes PatternType;


        void ApplyEvenlySpaced(ref List<float> s)
        {
            List<float> Temp = new List<float>();

            for (int i = 0; i < s.Count; i++)
            {
                float startoffsetMinBounds = Mathf.Clamp01(-SpawnWidth / 2f + s[i] + StartOffset - 0.5f);
                float startoffsetMaxBounds = Mathf.Clamp01(SpawnWidth / 2f + s[i] + StartOffset - 0.5f);

                for (int j = 0; j < Points; j++)
                {
                    Temp.Add(Mathf.Lerp(startoffsetMinBounds, startoffsetMaxBounds, j / (float)(Points - 1)));
                }
            }

            s = Temp;
        }


        void ApplyRandomSubset(ref List<float> s)
        {

            Points = Mathf.Clamp(Points, 0, s.Count);

            int[] indices = new int[s.Count];

            for (int i = 0; i < s.Count; i++)
            {
                indices[i] = i;  
            }

            Logic.Shuffle<int>(ref indices);

            for (int i = 0; i < s.Count - Points; i++)
            {
                s[indices[i]] = -1;

            }

            for (int i = 0; i < s.Count; i++)
            {
                if (s[i] == -1)
                {
                    s.RemoveAt(i);
                    i--;
                }
            }
        }


        public void Apply(ref List<float> s)
        {
            if (s.Count == 0)
            {

                if (Points == 1)
                {
                    s.Add(StartOffset);
                    return;
                }

                float startoffsetMinBounds = Mathf.Clamp01(-SpawnWidth /2f + StartOffset);
                float startoffsetMaxBounds = Mathf.Clamp01(SpawnWidth / 2f + StartOffset);

                for (int i = 0; i < Points; i++)
                {
                    s.Add(Mathf.Lerp(startoffsetMinBounds, startoffsetMaxBounds, i / (float)(Points - 1)));
                }
            }
            else
            {
                switch (PatternType)
                {
                    case PatternTypes.Manual:

                        break;
                    case PatternTypes.EvenlySpaced:
                        ApplyEvenlySpaced(ref s);
                        break;
                    case PatternTypes.RandomSubset:
                        ApplyRandomSubset(ref s);
                        break;
                    default:
                        break;
                }

            }

        }



    }

   




    private void OnValidate()
    {
        if (!Renderer)
        {
            Renderer = FindFirstObjectByType<SpawnPatternRenderer>();
        }

        if (Renderer != null)
        {
            Renderer.pattern = this;
        }

        Gen();
    }


    public void Draw(Vector3 start, Vector3 end,Color basecolor, float weight, Color sColor, Color eColor, float size, float subwaveOffset)
    {

        for (int i = 0; i < SubWaves.Count; i++)
        {
            for (int j = 0; j < SubWaves[i].SpawnPositions.Count; j++)
            {

                if (SubWaves[i].SpawnPositions.Count == 1)
                {
                    Gizmos.color = Logic.LerpColor(Logic.LerpColor(sColor, eColor, 0.5f), basecolor, weight);

                }
                else
                {
                    Gizmos.color = Logic.LerpColor(Logic.LerpColor(sColor, eColor, j / (float)(SubWaves[i].SpawnPositions.Count - 1)), basecolor, weight);

                }

                Vector3 SpawnPos = Logic.LerpVector(start, end, SubWaves[i].SpawnPositions[j]);
                Gizmos.DrawSphere(SpawnPos, size);

            }

            start.y -= subwaveOffset;
            end.y -= subwaveOffset;

        }
      


    }




}
