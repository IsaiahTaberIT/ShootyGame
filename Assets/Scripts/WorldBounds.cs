using UnityEngine;
using static UnityEngine.Mathf;

public class WorldBounds : MonoBehaviour
{
    public float GizmoSize = 1f;
    public Vector2 TestpointScreen;
    public Vector2 TestpointWorld;

    public Vector2 StartPoint;
    public Vector2 EndPoint;
    public Color StartColor;
    public Color EndColor;

    [Range(0,1)] public float Progress;

    public Surface PlayArea;
    public Surface ScreenSurface;

    public LayerMask PlaySurfaceLayer;

    [System.Serializable]
    public class Surface
    {
        public Vector3[] Corners = new Vector3[4];
        public Vector3 NW
        {
            get
            {
                return Corners[0];
            }
            set
            {
                Corners[0] = value;
            }
        }

        public Vector3 NE
        {
            get
            {
                return Corners[1];
            }
            set
            {
                Corners[1] = value;
            }
        }
        public Vector3 SE
        {
            get
            {
                return Corners[2];
            }
            set
            {
                Corners[2] = value;
            }
        }
        public Vector3 SW
        {
            get
            {
                return Corners[3];
            }
            set
            {
                Corners[3] = value;
            }
        }

 


        public Vector3 NormalizedPos(Vector3 Pos)
        {

            float yNormal = InverseLerp(NW.y, SW.y, Pos.y);



            float halfWidth = Lerp(Abs(NW.x) + Abs(NE.x), Abs(SW.x) + Abs(SE.x), yNormal) / 2f;
            float x = InverseLerp(halfWidth, -halfWidth, Pos.x);


            return new Vector3(x, yNormal, Pos.z);
        }


        public Vector3 NormalToSurface(Vector3 Pos)
        {
            float halfWidth = Lerp(Abs(NW.x) + Abs(NE.x), Abs(SW.x) + Abs(SE.x), Pos.y) / 2f;
            float x = Lerp(halfWidth, -halfWidth, Pos.x);

            float y = Lerp(NW.y, SW.y, Pos.y);

            return new Vector3(x, y, Pos.z);
        }



    }








    [ContextMenu("get")]
    public void GetScreenEdges()
    {
        Camera c = Camera.main;

        Vector3 o = c.transform.position;

        RaycastHit hitinfo;

        if (Physics.Raycast(o, c.ViewportToWorldPoint(new(0, 1, 20)) - o, out hitinfo, 10000f, PlaySurfaceLayer))
        {
            ScreenSurface.Corners[0] = hitinfo.point;
        }

        if (Physics.Raycast(o, c.ViewportToWorldPoint(new(1, 1, 20)) - o, out hitinfo, 10000f, PlaySurfaceLayer))
        {
            ScreenSurface.Corners[1] = hitinfo.point;
        }

        if (Physics.Raycast(o, c.ViewportToWorldPoint(new(1, 0, 20)) - o, out hitinfo, 10000f, PlaySurfaceLayer))
        {
            ScreenSurface.Corners[2] = hitinfo.point;
        }

        if (Physics.Raycast(o, c.ViewportToWorldPoint(new(0, 0, 20)) - o, out hitinfo, 10000f, PlaySurfaceLayer))
        {
            ScreenSurface.Corners[3] = hitinfo.point;
        }


    

        Debug.Log(c.fieldOfView);
        Debug.Log(c.aspect);
        Debug.Log(c.ScreenToWorldPoint(new(0,0,0)));


    }

    public void MoveTestPointScreen()
    {
        TestpointScreen = Logic.LerpVector(StartPoint, EndPoint, Progress);

    }

    public void MoveTestPointWorld()
    {
        TestpointWorld = Logic.LerpVector(PlayArea.NormalToSurface(StartPoint), PlayArea.NormalToSurface(EndPoint), Progress);

    }



   


    public void DrawPointWorld(Vector2 point, Color color)
    {
        point = PlayArea.NormalToSurface(point);

        Gizmos.color = color;

        Gizmos.DrawSphere(new(point.x , point.y), GizmoSize);

    }

    public void DrawPoint(Vector2 point, Color color)
    {
        Gizmos.color = color;

        Gizmos.DrawSphere(new(point.x, point.y), GizmoSize);

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameController.Controller.Bounds = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ExecuteAlways]
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(PlayArea.NW, GizmoSize);

        Gizmos.color = Color.orange;

        Gizmos.DrawWireSphere(PlayArea.NE, GizmoSize);

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(PlayArea.SE, GizmoSize);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(PlayArea.SW, GizmoSize);


        DrawPointWorld(StartPoint, StartColor);

        DrawPointWorld(EndPoint, EndColor);

        MoveTestPointScreen();

        DrawPointWorld(TestpointScreen, (Color.yellow + Logic.LerpColor(StartColor,EndColor,Progress)) / 2f);

        MoveTestPointWorld();

        DrawPoint(TestpointWorld, (Color.black + Logic.LerpColor(StartColor, EndColor, Progress)) / 2f);


        Camera c = Camera.main;


        Gizmos.color = Color.white;

      //  Gizmos.DrawSphere(c.ScreenToWorldPoint(new(0, 0, 40)), GizmoSize);
      //  Gizmos.DrawSphere(c.ScreenToWorldPoint(new(Screen.width, 0, 20)), GizmoSize);
      //  Gizmos.DrawSphere(c.ScreenToWorldPoint(new(0, Screen.height, 40)), GizmoSize);
    //    Gizmos.DrawSphere(c.ScreenToWorldPoint(new(Screen.width, Screen.height, 20)), GizmoSize);

        Gizmos.color = Color.black;

        for (int i = 0; i < ScreenSurface.Corners.Length; i++)
        {
            Gizmos.DrawSphere(ScreenSurface.Corners[i], GizmoSize);
        }
        Gizmos.color = Color.black;
       
        Debug.DrawRay(c.transform.position, c.ViewportToWorldPoint(new(0, 1, 2000 )) - c.transform.position);

        Gizmos.color = Color.white;

    


        Debug.DrawRay(c.transform.position, c.ViewportToWorldPoint(new(0, 0, 20)) - c.transform.position);


    }
}
