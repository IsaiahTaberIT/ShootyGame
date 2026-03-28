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

    private void OnDisable()
    {
        GameController.OnFixedUpdateUnPaused -= OnUpdate;

    }
    private void OnEnable()
    {
        GameController.OnFixedUpdateUnPaused += OnUpdate;

    }

    private void OnUpdate()
    {
        FireCooldown.Step(Time.fixedDeltaTime);

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
        Weapon w = null;


        if (FireCooldown.Ratio >= 1f)
        {
            if (Automatic)
            {
                w = GameObject.Instantiate(WeaponObject, position, rotation);
                w.gameObject.SetActive(true);
                FireCooldown.Time %= FireCooldown.EndTime;
            }
            else if (Released)
            {
                w = GameObject.Instantiate(WeaponObject, position, rotation);
                w.gameObject.SetActive(true);
                FireCooldown.Time %= FireCooldown.EndTime;
                Released = false;
            }

        }


    }

}
