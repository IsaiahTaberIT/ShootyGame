using UnityEngine;
using static Logic;
public class Laser : Weapon
{
    Vector3 playerpos = Vector3.zero;
    public LayerMask mask;
    public Timer ChargeUpTime = new(0.5f, 0, true);
    public Timer CoolDownTime = new(1, 1, true);

    public Timer UpTime = new(10f, 0, true);
    public Vector3 TargetDirection;
    public Vector3 CurrentDir;
    public float Z;
    public float OffsetMult;
    public bool IsOverheated= false;
    public float MaxUpTime = 10f;
    public float TurningSpeed = 1f;
    public float RotSpeedFalloffRange = 10;
    
    private void Update()
    {
        CoolDownTime.Step();

        //    Debug.Log("LaserActive");


        Vector3 self = transform.position;


     
        Vector3 mouse = GameController.WorldMousePos;
        mouse.z = Z;
        playerpos = GameController.Controller.Player_Ref.transform.position;
        playerpos.z = Z;





        TargetDirection = mouse - playerpos;

        CurrentDir = transform.TransformDirection(Vector3.forward);


        float angle = Vector3.Angle(CurrentDir, TargetDirection);

        float distanceModifier = Mathf.Min(1, angle / RotSpeedFalloffRange);




        Vector3 newDir = Vector3.RotateTowards(CurrentDir, TargetDirection, Time.deltaTime * TurningSpeed * distanceModifier, 100f);





        self = newDir.normalized * transform.localScale.z * 5f + playerpos;
        self.z = Z;
        transform.position = self;

        transform.rotation = Quaternion.LookRotation(newDir, Vector3.forward);


    }

    public override void ImpactEnemy(HitBoxController enemyHitBox)
    {
        //enemyHitBox.Enemy.Hurt(Damage);
      //  SpawnParticles(enemyHitBox.transform.position,Quaternion.identity);

    
    }

    public override void Released()
    {

        ChargeUpTime.Reset();

        UpTime.Reset();

        ReScaleBeam(0f);

    }

    public void Use()
    {

        if (!CoolDownTime.IsFinished)
        {
            ReScaleBeam(0f);

            return;
        }

        IsOverheated = false;

        ChargeUpTime.Step();


        if (ChargeUpTime.IsFinished )
        {
            UpTime.Step();
            if (UpTime.IsFinished)
            {
                IsOverheated = true;
                UpTime.Reset();
                ChargeUpTime.Reset();
                CoolDownTime.Reset();
            }


            HitScanEnemy();
        }
        


    }

    public void ReScaleBeam(float length)
    {
        Vector3 newscale = transform.localScale;
        newscale.z = length;
        transform.localScale = newscale;
    }


    public void HitScanEnemy()
    {
        float RemainingPierce = PiercePower;

        float initalPiercePower = PiercePower;
        if (RemainingPierce <= 0)
        {
            RemainingPierce = 1f;
            initalPiercePower = 1f;
        }



        RaycastHit2D[] hits = RaycastByTypeAll<HitBoxController>(playerpos, CurrentDir, 100f, mask);

        if (hits == null)
        {
            ReScaleBeam(100f);

            return;
        }

        Debug.Log("HitAnything");


        for (int i = 0; i < hits.Length; i++ )
        {
            if (hits[i].collider.gameObject.TryGetComponent<HitBoxController>(out HitBoxController enemyHitBox))
            {
                enemyHitBox.Enemy.Hurt(Damage * RemainingPierce / initalPiercePower);
                SpawnParticles(enemyHitBox.transform.position,Quaternion.identity);
                RemainingPierce -= enemyHitBox.Enemy.PierceResistance;
            }

            if (RemainingPierce <= 0)
            {
                ReScaleBeam(Vector2.Distance(playerpos, hits[i].point) / 10f);
                return;

            }
        }


    }



}
