using UnityEngine;

public class BasicEnemy : Enemy
{
    public float GroundSpeed;
    public float SelfDamageRatio = 0.1f;
    public float AttackThreshold = 0.05f;
    public void Move()
    {
        Vector3 ScreenPos = bounds.PlayArea.NormalizedPos(transform.position);

        NormailzedPosition = ScreenPos + Vector3.up * Time.deltaTime * GroundSpeed * 0.01f;

        //  Debug.Log(ScreenPos);
        //  Debug.Log(NormailzedPosition);

        Vector3 NewWorldPos = bounds.PlayArea.NormalToSurface(NormailzedPosition);
       // Debug.Log(transform.position);
       // Debug.Log(NewWorldPos);

        transform.position = NewWorldPos;
    }

    public override void Attack()
    {

        base.Attack();

        if (NormailzedPosition.y > 1f - AttackThreshold)
        {
            Hurt(SelfDamageRatio * BaseHealth);
        }


    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
