using UnityEngine;
using static UnityEngine.Mathf;
public class WorldBounds : MonoBehaviour
{
    public float GizmoSize = 1f;
    public Vector3 NW;
    public Vector3 NE;
    public Vector3 SE;
    public Vector3 SW;

    public Vector2 TestpointScreen;
    public Vector2 TestpointWorld;

    public Vector2 StartPoint;
    public Vector2 EndPoint;
    public Color StartColor;
    public Color EndColor;

    [Range(0,1)] public float Progress;


    public void MoveTestPoint()
    {
        TestpointScreen = Logic.LerpVector(StartPoint, EndPoint, Progress);

    }

    public void DrawPoint(Vector2 point, Color color)
    {
        float halfWidth = Lerp(Abs(NW.x) + Abs(NE.x), Abs(SW.x) + Abs(SE.x), point.y) / 2f;
        float x = Lerp(halfWidth, -halfWidth, point.x);

        float y = Lerp(NW.y, SW.y, point.y);

    
        Gizmos.color = color;

        Gizmos.DrawSphere(new(x , y), GizmoSize);



    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ExecuteAlways]
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(NW, GizmoSize);

        Gizmos.color = Color.orange;

        Gizmos.DrawWireSphere(NE, GizmoSize);

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(SE, GizmoSize);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(SW, GizmoSize);


        DrawPoint(StartPoint, StartColor);

        DrawPoint(EndPoint, EndColor);

        MoveTestPoint();



        DrawPoint(TestpointScreen, (Color.yellow + Logic.LerpColor(StartColor,EndColor,Progress)) / 2f);





    }
}
