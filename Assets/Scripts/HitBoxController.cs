using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    public Enemy Enemy;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        Enemy = GetComponentInParent<Enemy>();
    }




    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("Hit!");


        if (collision.gameObject.TryGetComponent<HurtBoxController>(out HurtBoxController hurtbox))
        {

            hurtbox.Projectile.ImpactEnemy(Enemy);
            
        }
    }

}
