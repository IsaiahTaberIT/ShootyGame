using UnityEngine;

public class HurtBoxController : MonoBehaviour
{
    public Weapon Weapon_Ref;
    public CollisionBehavior Behavior;
    public enum CollisionBehavior
    {
        Off =0,
        FirstHit=1,
        PerFrame=2,
        Scripted = 3

    }
    private void OnEnable()
    {
        Weapon_Ref = GetComponentInParent<Weapon>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Behavior != CollisionBehavior.PerFrame || (int)Behavior == 0)
        {
            return;
        }

        if (collision.gameObject.TryGetComponent<HitBoxController>(out HitBoxController hitbox))
        {
            Enemy e = hitbox.Enemy;

            Weapon_Ref.ImpactEnemy(hitbox);



        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (Behavior != CollisionBehavior.FirstHit || (int)Behavior == 0)
        {
            return;
        }
            Debug.Log("Hit!");


            if (collision.gameObject.TryGetComponent<HitBoxController>(out HitBoxController hitbox))
            {
                Enemy e = hitbox.Enemy;

                Weapon_Ref.ImpactEnemy(hitbox);



            }
        
    }


}
