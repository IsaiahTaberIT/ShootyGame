using UnityEngine;

public class HurtBoxController : MonoBehaviour
{
    public Weapon Weapon_Ref;
    public bool TriggerOnFirstHit = true;
    private void OnEnable()
    {
        Weapon_Ref = GetComponentInParent<Weapon>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (TriggerOnFirstHit)
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
    
        if (!TriggerOnFirstHit)
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
