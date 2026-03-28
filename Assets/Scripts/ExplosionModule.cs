using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Logic;

public class ExplosionModule : ProjectileModule
{
    public float KnockbackUpBias = 0.5f;
    public float KnockBackPower = 200;
    public float BaseExplosionSize = 10f;
    public ParticleSystem ExplosionParticles;
    public float ExplosionDamage;
    Projectile Projectile_Ref;
    public float DetectionRange;
    public float ExplosionRadius;

    public bool OnlyDetonateOnImpact = false;
    public ContactFilter2D ContactFilter;
    public CircleCollider2D ExplosionHitRegion;
    public int EHRChildIndex = 0;
    public CircleCollider2D ExplosionDetonationRegion;
    public int EDRChildIndex = 1;
    public Transform[] Children = new Transform[10];





    private void OnEnable()
    {

        Children = gameObject.GetComponentsInChildren<Transform>();
        ExplosionHitRegion = Children[EHRChildIndex + 1].GetComponent<CircleCollider2D>();
        ExplosionDetonationRegion = Children[EDRChildIndex + 1].GetComponent<CircleCollider2D>();

        ExplosionHitRegion.radius = ExplosionRadius;
        ExplosionDetonationRegion.radius = DetectionRange;



    }

    void Detonate()
    {

        if (ExplosionHitRegion == null)
        {
            return;
        }

        if (ExplosionParticles != null)
        {
            GameObject particles = Instantiate(ExplosionParticles.gameObject, transform.position, Quaternion.identity);

            particles.transform.localScale = Vector3.one * ExplosionRadius * BaseExplosionSize;


        }



        List<Collider2D> output = new();

        if (ExplosionHitRegion.Overlap(ContactFilter, output) != 0)
        {
            foreach (Collider2D item in output)
            {
                if (item.gameObject.TryGetComponent<HitBoxController>(out HitBoxController h))
                {
                    if (h.IsSensor)
                    {
                        h.Enemy.SensorTriggered(Projectile_Ref, Projectile_Ref.MovementDir);

                        continue;
                    }


                    float damage = ExplosionDamage + Projectile_Ref.Damage;
                    Vector2 dir = h.Enemy.transform.position - transform.position;



                     float t = EaseIn(dir.magnitude / ExplosionRadius, 2f);
                 //   float t = Vector2.Distance(h.Enemy.transform.position, transform.position) / ExplosionRadius;

                    damage = Mathf.Lerp(damage, 0, t);
                    float knockback = Mathf.Lerp(KnockBackPower, 0, t);


                    //  Debug.Log(damage);
                    //  Debug.Log(t);
                    //  Debug.Log(Vector2.Distance(h.Enemy.transform.position, Projectile_Ref.transform.position));

                    GlobalDebugRenderer.AddSphere(h.Enemy.transform.position, 2, Color.red, 0.5f);

                    h.Enemy.Hurt(damage );
                    h.Enemy.KnockBack(dir.normalized + Vector2.up * KnockbackUpBias, knockback);
                }
            }
        }

        Projectile_Ref.Die();


    }




    public override void Init(Projectile p)
    {
        Projectile_Ref = p;
        p.OnImpact += Detonate;
    }


    public override void Apply(Projectile p)
    {
        if (OnlyDetonateOnImpact)
        {
            return;
        }

        if (ExplosionDetonationRegion == null)
        {
            return;
        }



        List<Collider2D> output = new();

        if (ExplosionDetonationRegion.Overlap(ContactFilter, output) != 0)
        {
            foreach (Collider2D item in output)
            {
                if (item.gameObject.TryGetComponent<HitBoxController>(out HitBoxController h))
                {
                    if (h.IsSensor)
                    {
                        continue;
                    }

                    Detonate();
                    
                    return;
                }
            }
        }
    }
}

