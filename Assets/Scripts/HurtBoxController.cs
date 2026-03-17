using UnityEngine;

public class HurtBoxController : MonoBehaviour
{
    public Projectile Projectile;

    private void OnEnable()
    {
        Projectile = GetComponentInParent<Projectile>();
    }



}
