using UnityEngine;
using static Logic;

public class Enemy : MonoBehaviour
{
    public Timer AttackCooldown;
    public int State;
    public WorldBounds bounds;

    public float BaseHealth;
    public float Health;
    public float Damage;
    public Vector3 NormailzedPosition;

  


    public virtual void Die()
    {
        GameObject.Destroy(gameObject);
    }

    public virtual void Attack()
    {
        GameController.Controller.Player_Ref.Damage(Damage);
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
