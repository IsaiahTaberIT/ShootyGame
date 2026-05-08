using UnityEngine;
using UnityEngine.UIElements;
using static Logic;

public class Enemy : MonoBehaviour
{
    public int Index = 0;
    public MeshRenderer Renderer;
    public ParticleSystem DeathParticles;
    public float PierceResistance = 1f;
    public Timer AttackCooldown = new Timer(1,0);
    public WorldBounds bounds;
    public float KnockbackEffectiveness = 1f;
    public float SelfDamageOnHit = 0.1f;
    public float AttackThreshold = 0.05f;
    public float Health;
    public float Damage;
    public Vector3 NormailzedPosition;
    public float Speed;

    public EnemyBaseStats BaseStats;



    public void SetDeathParticlesColor()
    {
        Material instance = new(Renderer.material);
        ParticleSystem.MainModule main = DeathParticles.main;
        Renderer.material = instance;
        main.startColor = Renderer.sharedMaterial.GetColor("_BaseColor");


    }

 


    public virtual void InitializeStats()
    {

        Speed = BaseStats.Speed;
        Health = BaseStats.Health;
        Damage = BaseStats.Damage;
        PierceResistance = BaseStats.PierceResistance;
        KnockbackEffectiveness = 1f / BaseStats.KnockBackResistance;

        SelfDamageOnHit = Health * BaseStats.SelfDamageRatio;
    }


    public virtual void SensorTriggered(Weapon weapon, Vector3 dir)
    {
    }

 
    public virtual void KnockBack(Vector3 direction, float magnitude)
    {

    }

    public virtual void Die()
    {

        DeathParticles.transform.SetParent(null, false);
        DeathParticles.gameObject.SetActive(true);

        DeathParticles.transform.position = transform.position; 
        GameController.Controller.RemoveEnemy(Index);

        GameObject.Destroy(gameObject);
    }

    public virtual void Attack()
    {
        GameController.Controller.Player_Ref.Hurt(Damage);
    }

    public virtual void Hurt(float incomingDamage)
    {

        Health -= incomingDamage;

        if (Health <= 0)
        {
            Die();
        }
    }
}
