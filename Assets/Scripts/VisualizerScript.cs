using UnityEngine;
using static Logic;
[ExecuteAlways]





public class VisualizerScript : MonoBehaviour
{
    public float Output1;
    public Vector2 Output2;

    public Vector2 V1;
    public Vector2 V2;
    public Vector2 VTest;
    public float Size;
    public float TestFloat1;
    public float TestFloat2;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(V1, V2);

        Output1 = DistanceToRay(VTest, V1, V2);
        Output2 = NearestPointOnInfiniteLine(V1,V2, VTest);
        Gizmos.DrawSphere(V1,Size);

        Gizmos.DrawSphere(V2, Size);
        Gizmos.DrawSphere(VTest, Size);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(Output2, Size);


    }


    private void OnValidate()
    {
        Debug.Log(TestFloat1 % TestFloat2);


    }

}
