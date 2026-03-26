using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    public float PlayerSpeedMult;
    public float Damage = 100f;
    public GameObject AtSpawnParticles;
    public GameObject ImpactParticles;
    public float PiercePower = 2f;

    public Vector3 Origin => this.GetOrigin();

 



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
