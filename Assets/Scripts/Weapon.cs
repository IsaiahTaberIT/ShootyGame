using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{

    public Action OnImpact = () => { };
    public float PlayerSpeedMult = 1f;
    public float Damage = 100f;
    public GameObject AtSpawnParticles;
    public GameObject ImpactParticles;
    public float PiercePower = 2f;
    public float KnockBackForce = 10f;

    public Vector3 Origin => this.GetOrigin();

    public WeaponBaseStats BaseStats;

    public virtual void InitializeStats()
    {

        Damage = BaseStats.Damage;
        PiercePower = BaseStats.PiercePower;
        KnockBackForce = BaseStats.KnockBackPower;
    }



    public virtual Vector3 GetOrigin()
    {
        //Instead Of always getting the transform position,
        //when it makes sense this can be over-written to be a different position

        return transform.position;


    }




    public virtual void Released()
    {

    }


    public virtual void ImpactHitbox(HitBoxController HitBox)
    {



    }


    public virtual void ImpactHitbox(HitBoxController HitBox, RaycastHit2D hitInfo)
    {



    }


    public virtual void ImpactHitbox(HitBoxController HitBox, Collider2D collision)
    {



    }

    public virtual void SpawnParticles(Vector3 position, Quaternion rotation)
    {
        if (ImpactParticles == null)
        {
            return;
        }

        Instantiate(ImpactParticles, position, rotation);
    }


    public virtual void SpawnParticles()
    {
        SpawnParticles(transform.position, Quaternion.identity);

    }
}
