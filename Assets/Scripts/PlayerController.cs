using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Logic;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private InputActionReference MovementAction;
    [SerializeField] private InputActionReference ShootAction;
    [SerializeField] private InputActionReference ScrollAction;
    [SerializeField] private UnityEvent OnHurt;
    [SerializeField] private Timer ShootCooldown = new(0.1f, 0, true);

    public Rigidbody PlayerBody;
    public float WeaponMovementSpeedMultiplier => GetMovementSpeedMultFromCurrentWeapon();
    public float Health = 100;
    public bool InEditor = true;
    public float Direction = 0;
    public Vector3 StartPoint = Vector3.zero;
    public Vector3 EndPoint = Vector3.zero;
    public Vector3 TargetPoint = Vector3.zero;
    public Vector3 ViewMousePos = Vector3.zero;
    public Vector3 WorldMousePos = Vector3.zero;
    public Transform PlayerTransform;
    public float Progress;
    [Min(0.01f)] public float Speed;
    private float BaseSpeed = 100f;
    public float GizmoSize;
    public float Velocity;


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

    float GetMovementSpeedMultFromCurrentWeapon()
    {
        if (!Weapons.IsValueInRange(WeaponIndex) ||
            Weapons[WeaponIndex] == null ||
            Weapons[WeaponIndex].WeaponObject == null ||
            !Weapons[WeaponIndex].isActiveAndEnabled
            )
        {
            return 1;
        }

        return Weapons[WeaponIndex].WeaponObject.PlayerSpeedMult;






    }

    public WeaponWrapper[] Weapons;


    [ContextMenu("try")]
    public void Try()
    {


    }

    public void ChangeWeapon(int newIndex)
    {
        Weapons[WeaponIndex].ReleaseTrigger();

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

        PlayerBody = GetComponent<Rigidbody>();

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

        float speed = Speed / BaseSpeed * WeaponMovementSpeedMultiplier;

        Velocity = speed * Direction * Vector3.Distance(StartPoint, EndPoint);

        speed *= Time.deltaTime;


        Progress += speed * Direction;

        Progress = Mathf.Clamp01(Progress);




        TargetPoint = Logic.LerpVector(StartPoint, EndPoint, Progress);

        PlayerBody.MovePosition(TargetPoint);

    }



    


    // Update is called once per frame
    void OnUpdate()
    {
        Direction = MovementAction.action.ReadValue<Vector2>().x;


 
        if (ShootAction.action.IsPressed())
        {
            TryShoot();
        }



        Vector2 scrollDelta = ScrollAction.action.ReadValue<Vector2>();


        if (Mathf.Abs(scrollDelta.y) > 0)
        {
            WeaponIndex += Mathf.CeilToInt(scrollDelta.y);

        }


    




        

        Move();
    }

    public void Lose()
    {
        GameController.Controller.GameEnd();
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
    public void TryShoot(InputAction.CallbackContext obj)
    {
        TryShoot();
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

   


    void ReleaseTriggerWrapper(InputAction.CallbackContext obj)
    {
        Weapons[WeaponIndex].ReleaseTrigger();
    }


    private void OnEnable()
    {

        ShootAction.action.started += TryShoot;
        ShootAction.action.canceled += ReleaseTriggerWrapper;
   







        GameController.OnUpdateUnPaused += OnUpdate;


        ShootCooldown.OnLoop += Shoot;
    }

    private void OnDisable()
    {
        GameController.OnUpdateUnPaused -= OnUpdate;

        ShootAction.action.started -= TryShoot;
        ShootAction.action.canceled -= ReleaseTriggerWrapper;
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
