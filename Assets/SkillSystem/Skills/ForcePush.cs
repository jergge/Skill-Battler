using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class ForcePush : Skill
{
    //public float forceMagnitudeHorizontal;
    //public float forceMagnitudeVertical;

    public float explosiveMagnitude;
    public float explosiveUpwardsModifier;
    public ForceMode forceMode;
    //public bool hitAllValid;
    //public int numTargetsToHit;
    public float maxHitDistance = 10;
    public float minHitDistance = 1;
    public float radiusIncresePerSecond = 5;
    float hitRadius = 1;
    [Range(0, 180)] public float angleFromCastCentre;
    //public float sphereRadiusForTesting;



    public override void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        if(OnCooldown())
        {
            return;
        }
        Debug.Log("Casting ForchPush");
        Collider[] hitColliders = Physics.OverlapSphere(source.transform.position, maxHitDistance);
        foreach(Collider other in hitColliders)
        {
            if(IsValidTarget(source,other.gameObject))
            {
                //Debug.Log("forcing target " + other.gameObject.name);
                Vector3 dirToTarget = other.gameObject.transform.position - source.gameObject.transform.position;
                dirToTarget.Normalize();

                float angleToTarget = Vector3.Angle(dirToTarget, source.transform.forward);
                if(angleToTarget <= angleFromCastCentre)
                {
                    IForceable o;
                    if (other.TryGetComponent<IForceable>(out o))
                    {
                        //Vector2 forceToApply = new Vector2(forceMagnitudeHorizontal, forceMagnitudeVertical);
                        //o.ApplyForce(dirToTarget, forceMagnitudeHorizontal, forceMode);
                        //o.ApplyForce(Vector3.up, forceMagnitudeVertical, forceMode);
                        o.ApplyExplosiveForce(explosiveMagnitude, source.transform.position, hitRadius, explosiveUpwardsModifier, forceMode);
                        //Debug.Log(dirToTarget);
                    }
                }
            }
        }
        hitRadius = minHitDistance;

        ResetCooldown();
    }

    public override void OnStartInSpellbook()
    {
        base.OnStartInSpellbook();
    }

    public override void OnStartInWorld()
    {
        base.OnStartInWorld();
    }

    public override void UpdateInSpellBook()
    {
        base.UpdateInSpellBook();
        if (hitRadius >= maxHitDistance)
        {
            hitRadius = maxHitDistance;
        } else {
            hitRadius += Time.deltaTime * radiusIncresePerSecond;
        }
    }

    public override void UpdateInWorld()
    {
        throw new System.NotImplementedException();
    }

    void OnDrawGizmos()
    {
        if (spellState == SpellState.SpellBook)
        {
            for (int i=-Mathf.RoundToInt(angleFromCastCentre); i<angleFromCastCentre; i++)
            {
                //Vector3 angle = new Vector3(Mathf.Sin((Mathf.Deg2Rad * i)+Mathf.PI/2), 0, Mathf.Cos(Mathf.Deg2Rad * i));
                //angle  = transform.InverseTransformDirection(angle);
                //Debug.Log(i);
                //int tempAngle = Mathf.RoundToInt(angleFromCastCentre);
                int tempAngle = i;
                Vector3 rotatedVector = new Vector3(gameObject.transform.forward.x * Mathf.Cos(Mathf.Deg2Rad * tempAngle) - gameObject.transform.forward.z * Mathf.Sin(Mathf.Deg2Rad * tempAngle), 0, gameObject.transform.forward.x * Mathf.Sin(Mathf.Deg2Rad * tempAngle) + gameObject.transform.forward.z * Mathf.Cos(Mathf.Deg2Rad * tempAngle));
                Gizmos.DrawRay(gameObject.transform.position, rotatedVector * hitRadius);
            }

                
        }
    }

}
