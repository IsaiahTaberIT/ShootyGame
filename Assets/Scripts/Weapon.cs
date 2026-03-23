using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    public float Damage = 100f;
    public GameObject AtSpawnParticles;
    public GameObject ImpactParticles;
    public float PiercePower = 2f;

    public virtual void Released()
    {

    }

    public virtual void ImpactEnemy(HitBoxController enemyHitBox)
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
