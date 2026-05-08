using UnityEngine;
using static Logic;
public class WanderingEnemy : Enemy
{
    public Timer WanderCooldown = new(1,0,false);
    public Vector2 TargetWanderDir;
    public float MinDelay = 0.1f;
    public float MaxDelay = 0.75f;
    public float LockedZ = 1f;
    public Rigidbody SelfBody;
    public Vector2 MoveDir;
    public Vector2 BaseMoveDir = Vector2.up;
    public float StrafeSpeed = 20;
    public float DirectionChangeSpeed = 1f;
    [Range(0f, 2f)] public float ReactionTime;

    public override void InitializeStats()
    {
        base.InitializeStats();
        StrafeSpeed = BaseStats.StrafeSpeed;
    }
  
    void UpdateWanderDirection()
    {
        WanderCooldown.EndTime = UnityEngine.Random.Range(MinDelay, MaxDelay);

        TargetWanderDir = UnityEngine.Random.insideUnitCircle;

    }


    public void Move()
    {
        float movementSpeed = Speed;




     
        MoveDir = (Vector2)Vector3.RotateTowards(MoveDir, TargetWanderDir, Time.deltaTime * DirectionChangeSpeed, 1f);

        Vector3 ScreenPos = bounds.PlayArea.NormalizedPos(transform.position);





        ScreenPos.z = LockedZ;

        NormailzedPosition = ScreenPos + ((Vector3)(MoveDir.normalized + BaseMoveDir) * movementSpeed) * 0.01f * Time.fixedDeltaTime;

        Vector3 NewWorldPos = bounds.PlayArea.NormalToSurface(NormailzedPosition);

        SelfBody.MovePosition(NewWorldPos);

    }

    public override void KnockBack(Vector3 direction, float magnitude)
    {
        direction.z = 0;

        Vector3 ScreenPos = bounds.PlayArea.NormalizedPos(transform.position);

        NormailzedPosition = ScreenPos + direction * magnitude;

        ScreenPos.z = LockedZ;

        Vector3 NewWorldPos = bounds.PlayArea.NormalToSurface(NormailzedPosition);

        SelfBody.AddForce(direction * magnitude * KnockbackEffectiveness);

    }

    public override void Attack()
    {

        if (NormailzedPosition.y > 1f - AttackThreshold)
        {
            base.Attack();

            Hurt(SelfDamageOnHit * Damage);
        }

    }

    void OnEnable()
    {
        UpdateWanderDirection();
        SetDeathParticlesColor();

        AttackCooldown.OnLoop += Attack;
        WanderCooldown.OnLoop += UpdateWanderDirection;
        GameController.OnFixedUpdateUnPaused += OnFixedUpdate;

        InitializeStats();

        SelfBody = GetComponent<Rigidbody>();

        if (bounds == null)
        {
            bounds = FindAnyObjectByType<WorldBounds>();
        }
    }
    private void OnDisable()
    {
        AttackCooldown.OnLoop -= Attack;
        WanderCooldown.OnLoop -= UpdateWanderDirection;

        GameController.OnFixedUpdateUnPaused -= OnFixedUpdate;

    }

    void OnFixedUpdate()
    {
        Move();
        AttackCooldown.Step(Time.fixedDeltaTime);
        WanderCooldown.Step(Time.fixedDeltaTime);

    }




}
