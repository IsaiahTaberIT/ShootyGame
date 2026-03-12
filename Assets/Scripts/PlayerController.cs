using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public bool InEditor = true;
    public float Direction = 0;


    public Vector3 StartPoint = Vector3.zero;
    public Vector3 EndPoint = Vector3.zero;
    public Vector3 TargetPoint = Vector3.zero;
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


    }

}
