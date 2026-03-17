using JetBrains.Annotations;
using System;
using UnityEngine;
using static Logic;
[RequireComponent (typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{

    public Timer LifeSpan = new Timer(100f, 0);

    public Rigidbody SelfBody;

    public float Damage = 100f;
    public float Speed = 20f;
    public Vector3 MovementDIr;
    public float ZLock;

    private void OnEnable()
    {
        LifeSpan.OnLoop += Die;
        SelfBody = GetComponent<Rigidbody>();
        MovementDIr = GameController.Controller.Player_Ref.WorldMousePos - transform.position;
        
        MovementDIr.Normalize();
    }


    void Update()
    {
        LifeSpan.Step();
        Move();
    }


    public void ImpactEnemy(Enemy enemy)
    {
        enemy.Hurt(Damage);

        Die();
    }





    public void Die()
    {
        Destroy(gameObject);

    }


    public void Move()
    {
        Vector3 NewPos = transform.position + MovementDIr * Speed * Time.deltaTime;

        NewPos.z = Mathf.Max(NewPos.z,ZLock);

        SelfBody.MovePosition(NewPos);
    }


}
