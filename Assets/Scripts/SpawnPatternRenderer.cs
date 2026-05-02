using UnityEngine;
[ExecuteAlways]
public class SpawnPatternRenderer : MonoBehaviour
{
    public WaveSpawnPattern pattern;
    public Vector3 Start;
    public Vector3 End;

    public Color StartColor;
    public Color EndColor;

    public float SpawnPointSize;
    public float EdgeSize;
    public float SubWaveOffset = 1;


    public float BaseColorWeight;
    public Color BaseColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = StartColor;
        Gizmos.DrawSphere(Start + transform.position, EdgeSize);
        Gizmos.color = EndColor;
        Gizmos.DrawSphere(End + transform.position, EdgeSize);



        pattern.Draw(Start + transform.position,End + transform.position, BaseColor,BaseColorWeight,StartColor,EndColor, SpawnPointSize, SubWaveOffset);
    }

}
