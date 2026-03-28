using UnityEngine;

public class BasicEnemy : Enemy
{
    public float LockedZ = 1f;
    public Rigidbody SelfBody;


    

    public void Move()
    {
        Vector3 ScreenPos = bounds.PlayArea.NormalizedPos(transform.position);

        ScreenPos.z = LockedZ;

        NormailzedPosition = ScreenPos + Vector3.up * Time.fixedDeltaTime * Speed * 0.01f;

        Vector3 NewWorldPos = bounds.PlayArea.NormalToSurface(NormailzedPosition);

        SelfBody.MovePosition(NewWorldPos);

    }

    public override void Die()
    {
        GameObject.Instantiate(DeathParticles,transform.position,Quaternion.identity);
        base.Die();

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
        GameController.OnFixedUpdateUnPaused -= OnFixedUpdate;

    }


    // Update is called once per frame
    void OnFixedUpdate()
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
