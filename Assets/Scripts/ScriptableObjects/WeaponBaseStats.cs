using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBaseStats", menuName = "Scriptable Objects/WeaponBaseStats")]
public class WeaponBaseStats : ScriptableObject
{

    [SerializeField] private float _Damage = 1f;

    public float Damage
    {
        get
        {
            return _Damage;
        }
        set
        {
            _Damage = value;
        }
    }


    [SerializeField] private float _KnockBackPower = 1f;

    public float KnockBackPower
    {
        get
        {
            return _KnockBackPower;
        }
        set
        {
            _KnockBackPower = value;
        }
    }


    [SerializeField] private float _Pierce = 1f;

    public float PiercePower
    {
        get
        {
            return _Pierce;
        }
        set
        {
            _Pierce = value;
        }
    }

    [SerializeField] private float _Speed = 1f;

    public float Speed
    {
        get
        {
            return _Speed;
        }
        set
        {
            _Speed = value;
        }
    }

}
