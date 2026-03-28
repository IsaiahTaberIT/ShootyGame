using UnityEngine;
using static Logic;

public class Enemy : MonoBehaviour
{
    public int Index = 0;
    public GameObject DeathParticles;
    public float PierceResistance = 1f;
    public Timer AttackCooldown;
    public WorldBounds bounds;
    public float KnockbackEffectiveness = 1f;
    public float SelfDamageOnHit = 0.1f;
    public float AttackThreshold = 0.05f;
    public float Health;
    public float Damage;
    public Vector3 NormailzedPosition;
    public float Speed;

    public EnemyBaseStats BaseStats;

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
