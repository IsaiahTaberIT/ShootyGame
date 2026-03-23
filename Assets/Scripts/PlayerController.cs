using System;
using UnityEngine;
using UnityEngine.Events;
using static Logic;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    public UnityEvent OnHurt;

    public Timer ShootCooldown = new(0.1f, 0, true);

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

  [SerializeField]  private int _WeaponIndex; 

    public int WeaponIndex
    {
        get
        {
            return _WeaponIndex;
        }
        set
        {
            ChangeWeapon(value);
        }
    }

    private void OnValidate()
    {
    //    WeaponIndex = _WeaponIndex;
    }


    public WeaponWrapper[] Weapons;


    [ContextMenu("try")]
    public void Try()
    {


    }

    public void ChangeWeapon(int newIndex)
    {

        newIndex %= Weapons.Length;


        if (newIndex < 0)
        {
            newIndex += Weapons.Length;
        }


        Debug.Log(WeaponIndex);
        Debug.Log(newIndex);

        if (WeaponIndex >= 0 && WeaponIndex < Weapons.Length)
        {
            if (Weapons[WeaponIndex] != null)
            {
                Weapons[WeaponIndex].gameObject.SetActive(false);

            }
        }


        _WeaponIndex = newIndex;
        Weapons[_WeaponIndex].gameObject.SetActive(true);
    }






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.Controller.Player_Ref = this;

        InitialiseAllWeapons();



        PlayerTransform = transform;
        InEditor = false;
    }


    void InitialiseAllWeapons()
    {
        foreach (WeaponWrapper weapon in Weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        ChangeWeapon(0);

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

        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            WeaponIndex += Mathf.CeilToInt(Input.mouseScrollDelta.y);

        }


        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Weapons[WeaponIndex].ReleaseTrigger();
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


    void UseActiveWeapon()
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


        Weapons[WeaponIndex].UseWeapon(spawnPos, Quaternion.identity);


       // GameObject.Instantiate(Weapons[WeaponIndex].gameObject, spawnPos, Quaternion.identity);
    }



    public void Shoot()
    {
        if (WeaponIndex >= Weapons.Length & Weapons[WeaponIndex] == null)
        {
            return;
        }


        UseActiveWeapon();

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
