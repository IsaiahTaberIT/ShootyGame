using UnityEngine;
using static Logic;

public class Enemy : MonoBehaviour
{
    public float PierceResistance = 1f;
    public Timer AttackCooldown;
    public int State;
    public WorldBounds bounds;
    public float KnockbackEffectiveness = 1f;

    public float BaseHealth;
    public float Health;
    public float Damage;
    public Vector3 NormailzedPosition;

  
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
