using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForcePush : Skill, IActiveSkill
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

    Transform spawnLocation;
    TargetInfo targetInfo;

    public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        this.spawnLocation = spawnLoaction;
        this.targetInfo = targetInfo;
    }

    void Update()
    {
        TickCooldown();
        if (hitRadius >= maxHitDistance)
        {
            hitRadius = maxHitDistance;
        } else {
            hitRadius += Time.deltaTime * radiusIncresePerSecond;
        }
    }

    void OnDrawGizmos()
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

    public void Cast()
    {
        Debug.Log("Casting ForchPush");
        Collider[] hitColliders = Physics.OverlapSphere(source.transform.position, maxHitDistance);
        foreach(Collider other in hitColliders)
        {
            if(IsValidTarget(other.gameObject))
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
}
