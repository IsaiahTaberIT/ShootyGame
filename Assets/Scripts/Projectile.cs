using System;
using System.Collections.Generic;
using UnityEngine;
using static Logic;
[RequireComponent(typeof(Rigidbody))]
public class Projectile : Weapon
{
    public float Spread = 5f;
    public float Precision = 5f;

    public float Health;
    public float SameEnemyHitCoolDown = 0.15f;
    public Timer LifeSpan = new Timer(100f, 0);
    public Rigidbody SelfBody;
    public float Speed = 20f;
    public Vector2 MovementDir;
    public float ZLock;
    public HitBoxController LastEnemyHitBox;
    public float LastCollisionTime = 0f;

    public HomingModule Homing = null;
    public ExplosionModule Explosion = null;


    public override void InitializeStats()
    {
        base.InitializeStats();
        Speed = BaseStats.Speed;
    }

    private void OnEnable()
    {
        GameController.OnFixedUpdateUnPaused += OnFixedUpdate;

        Homing = GetComponentInChildren<HomingModule>();
        Explosion = GetComponentInChildren<ExplosionModule>();


        if (Explosion != null)
        {
            Explosion.Init(this);
        }

        if (Homing != null)
        {
            Homing.Init(this);
        }


      

        InitializeStats();
        LifeSpan.OnLoop += Die;
        SelfBody = GetComponent<Rigidbody>();



        Vector2 perfectMovementDir = GameController.WorldMousePos - transform.position;

        float rotationOffset = UnityEngine.Random.Range(-1f, 1f);

        rotationOffset = Mathf.Abs(Mathf.Pow(rotationOffset, Precision)) * Mathf.Sign(rotationOffset) * Spread;




        MovementDir = Quaternion.AngleAxis(rotationOffset, Vector3.forward) * perfectMovementDir;






        MovementDir.Normalize();

        Quaternion particleOrientation = Quaternion.LookRotation(MovementDir, Vector3.forward);

        Move();

        if (GameController.Controller != null)
        {
            Instantiate(AtSpawnParticles, GameController.Controller.Player_Ref.transform.position + (Vector3)MovementDir + Vector3.forward, particleOrientation);

        }
    }

    private void OnDisable()
    {
        GameController.OnFixedUpdateUnPaused -= OnFixedUpdate;


    }

    void OnFixedUpdate()
    {
        LifeSpan.Step(Time.fixedDeltaTime);
        Move();
    }


    public override void ImpactHitbox(HitBoxController hitBox, Collider2D collision)
    {

        if (hitBox.IsSensor)
        {
            hitBox.Enemy.SensorTriggered(this, MovementDir);
            return;
        }

        OnImpact.Invoke();


        if (hitBox == LastEnemyHitBox && LastCollisionTime - Time.realtimeSinceStartup > SameEnemyHitCoolDown)
        {
            return;
        }

      
        hitBox.Enemy.Hurt(Damage);

        hitBox.Enemy.KnockBack(MovementDir, KnockBackForce);
       
        SpawnParticles();



        LastCollisionTime = Time.realtimeSinceStartup;
        LastEnemyHitBox = hitBox;

        PiercePower -= hitBox.Enemy.PierceResistance;

        if (PiercePower < UnityEngine.Random.Range(0f, 1f))
        {
            Die();
        }

    }


    public override void SpawnParticles()
    {
        if (ImpactParticles == null)
        {
            return;
        }


        // Move(-Speed * Time.deltaTime);

        Quaternion particleOrientation = Quaternion.LookRotation(-MovementDir, Vector3.forward);

        Instantiate(ImpactParticles, transform.position, particleOrientation);
    }


    public void Die()
    {
        Destroy(gameObject);

    }

    public void Move(float amount)
    {
        if (Homing != null)
        {
            Homing.Apply(this);
        }

        if (Explosion != null)
        {
            Explosion.Apply(this);

        }


        Vector3 NewPos = transform.position + (Vector3)MovementDir * amount;

        NewPos.z = Mathf.Max(NewPos.z, ZLock);
        NewPos.z = ZLock;
        SelfBody.MovePosition(NewPos);
    }

    public void Move()
    {
        Move(Speed * Time.deltaTime);
    }


}
