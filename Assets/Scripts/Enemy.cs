using UnityEngine;
using static Logic;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour
{
    public GameObject DeathParticles;
    public float PierceResistance = 1f;
    public Timer AttackCooldown;
    public int State;
    public WorldBounds bounds;
    public float KnockbackEffectiveness = 1f;
    public float SelfDamageRatio = 0.1f;
    public float AttackThreshold = 0.05f;
    public float BaseHealth;
    public float Health;
    public float Damage;
    public Vector3 NormailzedPosition;



    public virtual void SensorTriggered(Weapon weapon, Vector3 dir)
    {

    }


    public virtual void KnockBack(Vector3 direction, float magnitude)
    {

    }

    public virtual void Die()
    {
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
