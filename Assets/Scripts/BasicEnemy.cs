using UnityEngine;

public class BasicEnemy : Enemy
{
    public GameObject DeathParticles;
    public float LockedZ = 1f;
    public Rigidbody SelfBody;
    public float GroundSpeed;
    public float SelfDamageRatio = 0.1f;
    public float AttackThreshold = 0.05f;
    public void Move()
    {
        Vector3 ScreenPos = bounds.PlayArea.NormalizedPos(transform.position);

        ScreenPos.z = LockedZ;





        NormailzedPosition = ScreenPos + Vector3.up * Time.deltaTime * GroundSpeed * 0.01f;

        //  Debug.Log(ScreenPos);
        //  Debug.Log(NormailzedPosition);

        Vector3 NewWorldPos = bounds.PlayArea.NormalToSurface(NormailzedPosition);
        // Debug.Log(transform.position);
        // Debug.Log(NewWorldPos);


        SelfBody.MovePosition(NewWorldPos);


    }

    public override void Die()
    {
        GameObject.Instantiate(DeathParticles,transform.position,Quaternion.identity);
        base.Die();

    }


    public override void Attack()
    {


        if (NormailzedPosition.y > 1f - AttackThreshold)
        {
            base.Attack();

            Hurt(SelfDamageRatio * BaseHealth);
        }


    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        SelfBody = GetComponent<Rigidbody>();
        Health = BaseHealth;

        if (bounds == null)
        {
            bounds = FindAnyObjectByType<WorldBounds>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        AttackCooldown.Step();

        if(AttackCooldown.Ratio == 1)
        {
            Attack();
            AttackCooldown.Time %= AttackCooldown.EndTime;
        }

    }
}
