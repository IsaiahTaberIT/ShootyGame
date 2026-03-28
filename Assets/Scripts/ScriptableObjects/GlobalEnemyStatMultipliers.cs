using UnityEngine;

[CreateAssetMenu(fileName = "GlobalEnemyStatMultipliers", menuName = "Scriptable Objects/GlobalEnemyStatMultipliers")]
public class GlobalEnemyStatMultipliers : ScriptableObject
{
    public float GlobalScaler = 1f;

    [Header("Category Scalers")]

    public float SpeedScaler = 1f;
    public float DamageScaler = 1f;
    public float ResistanceScaler = 1f;

    [Header("Individual Stat Mults")]

    [SerializeField] private float _DamageMult = 1f;

    public float DamageMult
    {
        get
        {
            return _DamageMult * GlobalScaler * DamageScaler;
        }
        set
        {
            _DamageMult = value;
        }
    }

    [SerializeField] private float _HealthMult = 1f;

    public float HealthMult
    {
        get
        {
            return _HealthMult * GlobalScaler;
        }
        set
        {
            _HealthMult = value;
        }
    }

    [SerializeField] private float _KnockBackResistanceMult = 1f;

    public float KnockBackResistanceMult
    {
        get
        {
            return _KnockBackResistanceMult * GlobalScaler * ResistanceScaler;
        }
        set
        {
            _KnockBackResistanceMult = value;
        }
    }


    [SerializeField] private float _SpeedMult = 1f;

    public float SpeedMult
    {
        get
        {
            return _SpeedMult * GlobalScaler * SpeedScaler;
        }
        set
        {
            _SpeedMult = value;
        }
    }


    [SerializeField] private float _PierceResistanceMult = 1f;

    public float PierceResistanceMult
    {
        get
        {
            return _PierceResistanceMult * GlobalScaler * ResistanceScaler;
        }
        set
        {
            _PierceResistanceMult = value;
        }
    }



    [SerializeField] private float _StrafeSpeedMult = 1f;

    public float StrafeSpeedMult
    {
        get
        {
            return _StrafeSpeedMult * GlobalScaler * SpeedScaler;
        }
        set
        {
            _StrafeSpeedMult = value;
        }
    }


}
