using UnityEngine;

public class SmartAvoidingEnemy : Enemy
{
    public float TestPointSize = 50f;
    public Vector3 p1;
    public Vector3 p2;




    public float LockedZ = 1f;
    public Rigidbody SelfBody;
    public float GroundSpeed;


    public Vector2 MoveDir;
    public Vector2 BaseMoveDir = Vector2.up;
    public float StrafeDir;


    public float AvoidingRange;
    public float AvoidingFalloffPower = 2f;
    public float BaseStrafeSpeed = 20f;
    public float StrafeSpeed = 20;
    public Vector3 BiggestDangerPos;
    public float BiggestDangerCoefficient;
    public bool InDanger = false;
    public float Stubborness = 10f;
    public float DirectionChangeSpeed = 1f;
    [Range(0f, 2f)] public float ReactionTime;


    public override void SensorTriggered(Weapon w, Vector3 dir)
    {
        Vector3 pos = w.Origin;

        p1 = pos;
        p2 = pos + dir *10f;

        p1.z = 5f;
        p2.z = 5f;

        Vector3 test = Logic.NearestPointOnInfiniteLine(pos,pos+dir,(Vector2)transform.position + MoveDir.normalized * (GroundSpeed) * Time.smoothDeltaTime);
        float tempBDC = 0;

        float predictedPathWeight = 0.5f;


        tempBDC += (Logic.LerpVector(pos, test, predictedPathWeight) - transform.position).sqrMagnitude;
        tempBDC *= w.Damage;

        if (tempBDC < BiggestDangerCoefficient || !InDanger)
        {
            BiggestDangerCoefficient = tempBDC;
            BiggestDangerPos = test;
            InDanger = true;
        }
    }




    public void Move()
    {
        if (!InDanger)
        {
            MoveDir = (Vector2)Vector3.RotateTowards(MoveDir, BaseMoveDir, Time.deltaTime * DirectionChangeSpeed, 0f);

        }

        StrafeSpeed = 0;

        if (InDanger)
        {
            AvoidHarm();
            InDanger = false;
            BiggestDangerCoefficient = float.MaxValue;
        }
        else
        {
            AvoidPlayerLineOfSight();
        }

        


        Vector3 ScreenPos = bounds.PlayArea.NormalizedPos(transform.position);

        ScreenPos.z = LockedZ;

        //NormailzedPosition = ScreenPos + (Vector3)MoveDir.normalized *( GroundSpeed + StrafeSpeed) * 0.01f * Time.fixedDeltaTime;


        NormailzedPosition = ScreenPos + ((Vector3)MoveDir.normalized * GroundSpeed + Vector3.right * StrafeDir * StrafeSpeed) * 0.01f * Time.fixedDeltaTime;




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


    public void AvoidHarm()
    {
        BiggestDangerCoefficient = float.MaxValue;

        float t = AvoidingRange / (BiggestDangerPos - transform.position).magnitude ;

      

        if (t < 0)
        {
            Debug.Log("negative T");
        }

        t = Mathf.Min(t, Stubborness);

        Vector3 targetMoveDIr = Logic.LerpVector((Vector2)MoveDir, ((Vector2)BiggestDangerPos - (Vector2)transform.position).normalized, t / Stubborness);

        MoveDir = (Vector2)Vector3.RotateTowards(MoveDir, targetMoveDIr, Time.deltaTime * DirectionChangeSpeed * t / Stubborness,0f);







        //BiggestDangerPos = Vector3.zero;
    }



    public void AvoidPlayerLineOfSight()
    {


        StrafeSpeed = BaseStrafeSpeed;


        PlayerController p = GameController.Controller.Player_Ref;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(BiggestDangerPos, 2f);

        Gizmos.DrawLine(transform.position, transform.position + (Vector3)MoveDir * 10f);

        Gizmos.color = Color.lightBlue;


        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawSphere(p1, TestPointSize);
        Gizmos.DrawSphere(p2, TestPointSize);


    }



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

