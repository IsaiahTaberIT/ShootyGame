using System;
using System.Collections.Generic;
using UnityEngine;
using static Logic;
using static UnityEditor.Progress;
[RequireComponent(typeof(Rigidbody))]
public class Projectile : Weapon
{
    public float SameEnemyHitCoolDown = 0.15f;
    public ContactFilter2D ContactFilter;
    public CircleCollider2D HomingRegion;
    public Timer LifeSpan = new Timer(100f, 0);
    

    public Rigidbody SelfBody;
    public float HomingFactor = 1f;
    public float HomingRadius = 20f;

    public float KnockBackForce = 10f;
    public float Speed = 20f;
    public Vector3 MovementDIr;
    public float ZLock;

    private HitBoxController LastEnemyHitBox;
    private float LastCollisionTime = 0f;

    private void HandleHoming()
    {
        if (HomingFactor == 0f)
        {
            return;
        }

        float minDist = float.MaxValue;
        Collider2D closest = null;
        List<Collider2D> output = new();




        if (HomingRegion.Overlap(ContactFilter,output) != 0)
        {
        //    Debug.Log(output.Count);



            foreach (Collider2D item in output)
            {
                if (item.gameObject.TryGetComponent<HitBoxController>(out HitBoxController h))
                {
                   if (LastEnemyHitBox == h)
                   {
                        if (LastCollisionTime - Time.realtimeSinceStartup < SameEnemyHitCoolDown)
                        {
                            continue;
                        }
                    }

                   float potential = Vector2.Distance(item.gameObject.transform.position, transform.position);
                
                   if (potential < minDist)
                   {
                       minDist = potential;
                       closest = item;
                   }

                }
            }

            if (closest != null)
            {
                Vector2 directionToClosest = closest.gameObject.transform.position - transform.position;

            //    MovementDIr = Logic.LerpVector((Vector2)MovementDIr, directionToClosest.normalized,);

                MovementDIr = Vector3.RotateTowards((Vector2)MovementDIr, directionToClosest.normalized, Time.deltaTime * HomingFactor / 20f * (1f -(minDist / HomingRegion.radius)) , 100f);



            }


            HomingRegion.transform.localPosition = (Vector3)(((Vector2)MovementDIr + new Vector2(0, 0.5f)).normalized * HomingRegion.radius * 0.8f);
        }
    }

    private void OnEnable()
    {
        if (HomingRegion != null)
        {
            HomingRegion.radius = HomingRadius;
        }

        
        LifeSpan.OnLoop += Die;
        SelfBody = GetComponent<Rigidbody>();
        MovementDIr = GameController.WorldMousePos - transform.position;
        MovementDIr.Normalize();

        Quaternion particleOrientation = Quaternion.LookRotation(MovementDIr, Vector3.forward);


        Move();

        Instantiate(AtSpawnParticles, GameController.Controller.Player_Ref.transform.position + MovementDIr + Vector3.forward , particleOrientation);

    }


    void FixedUpdate()
    {
        LifeSpan.Step(Time.fixedDeltaTime);
        Move();
    }


    public override void ImpactEnemy(HitBoxController enemyHitBox)
    {
        if (enemyHitBox == LastEnemyHitBox && LastCollisionTime - Time.realtimeSinceStartup > SameEnemyHitCoolDown)
        {
            return;
        }



        enemyHitBox.Enemy.Hurt(Damage);

        enemyHitBox.Enemy.KnockBack(MovementDIr, KnockBackForce);
       
        SpawnParticles();



        LastCollisionTime = Time.realtimeSinceStartup;
        LastEnemyHitBox = enemyHitBox;

        PiercePower -= enemyHitBox.Enemy.PierceResistance;

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

        Quaternion particleOrientation = Quaternion.LookRotation(-MovementDIr, Vector3.forward);

        Instantiate(ImpactParticles, transform.position, particleOrientation);
    }


    public void Die()
    {
        Destroy(gameObject);

    }

    public void Move(float amount)
    {
        HandleHoming();

        Vector3 NewPos = transform.position + MovementDIr * amount;

        NewPos.z = Mathf.Max(NewPos.z, ZLock);
        NewPos.z = ZLock;
        SelfBody.MovePosition(NewPos);
    }

    public void Move()
    {
        Move(Speed * Time.deltaTime);
    }


}
