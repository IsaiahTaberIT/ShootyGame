using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBaseStats", menuName = "Scriptable Objects/EnemyBaseStats")]
public class EnemyBaseStats : ScriptableObject
{
    public GlobalEnemyStatMultipliers GlobalStats;

    [SerializeField] private float _Damage = 1f;

    public float Damage
    {
        get
        {
            return _Damage * GlobalStats.DamageMult;
        }
        set
        {
            _Damage = value;
        }
    }

    [SerializeField] private float _SelfDamageRatio = 1f;

    public float SelfDamageRatio
    {
        get
        {
            return _SelfDamageRatio;
        }
        set
        {
            _SelfDamageRatio = value;
        }
    }



    [SerializeField] private float _Health = 1f;

    public float Health
    {
        get
        {
            return _Health * GlobalStats.HealthMult;
        }
        set
        {
            _Health = value;
        }
    }


    [SerializeField] private float _KnockBackResistance = 1f;

    public float KnockBackResistance
    {
        get
        {
            return _KnockBackResistance * GlobalStats.KnockBackResistanceMult;
        }
        set
        {
            _KnockBackResistance = value;
        }
    }


    [SerializeField] private float _PierceResistance = 1f;

    public float PierceResistance
    {
        get
        {
            return _PierceResistance * GlobalStats.PierceResistanceMult;
        }
        set
        {
            _PierceResistance = value;
        }
    }

    [SerializeField] private float _Speed = 1f;

    public float Speed
    {
        get
        {
            return _Speed * GlobalStats.SpeedMult;
        }
        set
        {
            _Speed = value;
        }
    }


    [SerializeField] private float _StrafeSpeed = 1f;

    public float StrafeSpeed
    {
        get
        {
            return _StrafeSpeed * GlobalStats.StrafeSpeedMult;
        }
        set
        {
            _StrafeSpeed = value;
        }
    }



    [ContextMenu("log damage")]
    
    public void Log()
    {
        Debug.Log("_Damage " + _Damage);
        Debug.Log("Damage " + Damage);

    }



}
