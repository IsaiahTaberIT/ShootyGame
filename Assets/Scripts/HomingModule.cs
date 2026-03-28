using System;
using System.Collections.Generic;
using UnityEngine;

public class HomingModule : ProjectileModule
{
    public ContactFilter2D ContactFilter;
    public CircleCollider2D HomingRegion;
    public float HomingFactor = 1f;
    public float HomingRadius = 20f;

    private void OnEnable()
    {

        HomingRegion = GetComponent<CircleCollider2D>();
        if (HomingRegion != null)
        {
            HomingRegion.radius = HomingRadius;
        }
    }

    public override void Apply(Projectile p)
    {
        if (HomingFactor == 0f)
        {
            return;
        }

        float minDist = float.MaxValue;
        Collider2D closest = null;
        List<Collider2D> output = new();

        if (HomingRegion.Overlap(ContactFilter, output) != 0)
        {
            //    Debug.Log(output.Count);
            foreach (Collider2D item in output)
            {
                if (item.gameObject.TryGetComponent<HitBoxController>(out HitBoxController h))
                {
                    if (p.LastEnemyHitBox == h)
                    {
                        if (p.LastCollisionTime - Time.realtimeSinceStartup < p.SameEnemyHitCoolDown)
                        {
                            continue;
                        }
                    }

                    if (h.IsSensor)
                    {
                        continue;
                    }




                    float potential = Vector2.Distance(item.gameObject.transform.position, p.transform.position);

                    if (potential < minDist)
                    {
                        minDist = potential;
                        closest = item;
                    }

                }
            }

            if (closest != null)
            {
                Vector2 directionToClosest = closest.gameObject.transform.position - p.transform.position;

                p.MovementDir = Vector3.RotateTowards((Vector2)p.MovementDir, directionToClosest.normalized, Time.deltaTime * HomingFactor / 20f * (1f - (minDist / HomingRegion.radius)), 100f);
            }

            HomingRegion.transform.localPosition = (Vector3)(((Vector2)p.MovementDir + new Vector2(0, 0.5f)).normalized * HomingRegion.radius * 0.8f);
        }
    }


}
