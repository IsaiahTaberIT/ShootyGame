using UnityEngine;
using static Logic;
public class AvoidingEnemy : Enemy
{
    public float LockedZ = 1f;
    public Rigidbody SelfBody;
    public float GroundSpeed;

    Vector3 Forward = Vector2.up;
    Vector3 Right = Vector2.right;
    public float StrafeDir;
    public float AvoidingRange;
    public float AvoidingFalloffPower = 2f;
    public float StrafeSpeed = 20;
    [Range(0f,2f)]  public float ReactionTime;
    public void Move()
    {
        AvoidPlayerLineOfSight();


        Vector3 ScreenPos = bounds.PlayArea.NormalizedPos(transform.position);

        ScreenPos.z = LockedZ;

        NormailzedPosition = ScreenPos + (Forward * GroundSpeed + Right * StrafeDir * StrafeSpeed) * 0.01f * Time.fixedDeltaTime;

        Vector3 NewWorldPos = bounds.PlayArea.NormalToSurface(NormailzedPosition);

        SelfBody.MovePosition(NewWorldPos);

    }

    public override void Die()
    {
        GameObject.Instantiate(DeathParticles, transform.position, Quaternion.identity);
        base.Die();

    }

    public override void KnockBack(Vector3 direction, float magnitude)
    {
        direction.z = 0;
        direction.Normalize();

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

            Hurt(SelfDamageRatio * BaseHealth);
        }

    }

    void OnEnable()
    {

        SelfBody = GetComponent<Rigidbody>();
        Health = BaseHealth;

        if (bounds == null)
        {
            bounds = FindAnyObjectByType<WorldBounds>();
        }
    }

    public void AvoidPlayerLineOfSight()
    {
        PlayerController p = GameController.Controller.Player_Ref;

       // Vector2 MousePos = p.WorldMousePos;
        Vector2 MousePos = GameController.Controller.GetPastMousePosition(ReactionTime);

        

        Vector2 dist = Logic.NearestPointOnInfiniteLine(p.transform.position, MousePos, transform.position) - (Vector2)transform.position;

        float absoluteDistance = Mathf.Abs(dist.magnitude);


     //  Debug.Log(dist);

        absoluteDistance /= AvoidingRange;

        float avoidStrength = 1 - Mathf.Clamp01(absoluteDistance);

        avoidStrength = Mathf.Pow(avoidStrength, AvoidingFalloffPower);

        StrafeDir = Mathf.Sign(dist.x) * avoidStrength;
    }



    // Update is called once per frame




    void FixedUpdate()
    {

        Move();

        AttackCooldown.Step(Time.fixedDeltaTime);

        if (AttackCooldown.Ratio == 1)
        {
            Attack();
            AttackCooldown.Time %= AttackCooldown.EndTime;
        }

    }
}
