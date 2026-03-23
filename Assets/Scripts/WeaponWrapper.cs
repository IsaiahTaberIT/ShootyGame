using UnityEngine;
using static Logic;
public class WeaponWrapper : MonoBehaviour
{
    public string Name = "t";
    public Weapon WeaponObject;
    public Timer FireCooldown = new(1, 0, true);
    public bool Automatic;
    [SerializeField] private bool Released = true;

    public void ReleaseTrigger()
    {
        Released = true;
        WeaponObject.Released();
    }

    private void Update()
    {
        FireCooldown.Step();
    }

    public void UseWeapon(Vector3 position, Quaternion rotation)
    {
        if (WeaponObject == null)
        {
            return;
        }
        else if (WeaponObject is Laser l)
        {
            l.Use();
        }
        else if (WeaponObject is Projectile p)
        {
            TryInstantiateProjectile(position, rotation);
        }
    }
    void TryInstantiateProjectile(Vector3 position, Quaternion rotation)
    {
        if (FireCooldown.Ratio >= 1f)
        {
            if (Automatic)

            {
                GameObject.Instantiate(WeaponObject, position, rotation);
                FireCooldown.Time %= FireCooldown.EndTime;
            }
            else if (Released)
            {
                GameObject.Instantiate(WeaponObject, position, rotation);
                FireCooldown.Time %= FireCooldown.EndTime;
                Released = false;
            }

        }


    }

}
