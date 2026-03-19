using UnityEngine;
using UnityEngine.Events;
using static Logic;

public class PlayerController : MonoBehaviour
{
    public UnityEvent OnHurt;

    public Timer ShootCooldown = new(0.1f,0,true);

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

    public int WeaponIndex = 0;
    public Projectile[] Weapons = new Projectile[0];



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

        //  WorldMousePos = ;


        if (Input.GetMouseButtonDown(0))
        {
            ShootCooldown.Time = 0;
            Shoot();
        }

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

    public void Hurt(float value)
    {
        Health -= value;
        OnHurt.Invoke();

        if (Health <= 0)
        {
            Lose(); 
        }
    }
    public void TryShoot()
    {
        ShootCooldown.Step();
    }


    void InstantiateProjectile()
    {
        Camera c = GameController.Controller.MainCamera_Ref;
        Vector3 o = c.transform.position;
        RaycastHit hitinfo;
        Vector3 spawnPos = transform.position;
        Vector3 dir = transform.position - o;
        Vector3 correction = dir.normalized;
        float correctionMag = 2f;


        //because I can't be fucked to compute the vector math to get the properly aligned spawnpoint for the projectile
        //I'M just gonna raycast from the camera through the player and use the intersection point with the background

        if (Physics.Raycast(o, dir, out hitinfo, 10000f, GameController.Controller.Bounds.PlaySurfaceLayer))
        {
            spawnPos = hitinfo.point - correction * correctionMag;
        }

        WorldBounds.Surface playarea = GameController.Controller.Bounds.PlayArea;
        GameObject.Instantiate(Weapons[WeaponIndex].gameObject, spawnPos, Quaternion.identity);
    }



    public void Shoot()
    {
        if (WeaponIndex >= Weapons.Length & Weapons[WeaponIndex] == null)
        {
            return;
        }

        if (Weapons[WeaponIndex] is Projectile)
        {
            InstantiateProjectile();
        }









    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
    }

    private void OnEnable()
    {
        ShootCooldown.OnLoop += Shoot;
    }

    private void OnDisable()
    {
        ShootCooldown.OnLoop -= Shoot;
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
