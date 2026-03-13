using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public GameObject TestSphere;

    public float Health = 100;
    public bool InEditor = true;
    public float Direction = 0;




    public Vector3 StartPoint = Vector3.zero;
    public Vector3 EndPoint = Vector3.zero;
    public Vector3 TargetPoint = Vector3.zero;

    public Vector3 ViewMousePos = Vector3.zero;

    public Vector3 WorldMousePos = Vector3.zero;

    // public float InterpolationSpeed = 1;
    //public float MinInterpolationSpeed = 0.1f;

    public Transform PlayerTransform;

    public float Progress;

    [Min(0.01f)] public float Speed;
    private float BaseSpeed = 100f;
    public float GizmoSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.Controller.Player_Ref = this;
 


        PlayerTransform = transform;
        InEditor = false;
    }


    public void Move()
    {

        float speed = Time.deltaTime * Speed / BaseSpeed;

        Progress += speed * Direction;

        Progress = Mathf.Clamp01(Progress);

        TargetPoint = Logic.LerpVector(StartPoint, EndPoint, Progress);

        PlayerTransform.position = TargetPoint;

    }


    // Update is called once per frame
    void Update()
    {
        Direction = 0f;

        ViewMousePos = (Input.mousePosition / new Vector2(Screen.width,Screen.height));
        ViewMousePos.y = 1f - ViewMousePos.y;
        ViewMousePos = Input.mousePosition;
        ViewMousePos.z = 30;

        Camera c = Camera.main;

        Vector3 o = c.transform.position;

        RaycastHit hitinfo;

        if (Physics.Raycast(o, c.ScreenToWorldPoint(ViewMousePos) - o, out hitinfo, 10000f, GameController.Controller.Bounds.PlaySurfaceLayer))
        {
            WorldMousePos = hitinfo.point;
        }

        if (TestSphere != null)
        {
            TestSphere.transform.position = WorldMousePos;
        }









      //  WorldMousePos = ;

        if (Input.GetKey(KeyCode.D))
        {
            Direction = 1f;

        }

        if (Input.GetKey(KeyCode.A))
        {
            Direction = -1f;
        }


        Move();
    }

    public void Lose()
    {
        Debug.Log("You Lost");
    }

    public void Damage(float value)
    {
        Health -= value;

        if (Health <= 0)
        {
            Lose(); 
        }
    }



    [ExecuteAlways]
    private void OnDrawGizmosSelected()
    {
        if (InEditor)
        {
            Move();

        }

        Gizmos.color = Color.green;

        Gizmos.DrawSphere(StartPoint, GizmoSize);

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(EndPoint, GizmoSize);

        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(TargetPoint, GizmoSize / 2f);

        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(WorldMousePos, GizmoSize);


    }

}
