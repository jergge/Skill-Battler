using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IForceable
{
    //public void TakeForce (Vector3 directon, float magnitude);
    public void ApplyForce(Vector3 directon, float magnitude, ForceMode forceMode);
    public void ApplyExplosiveForce(
        float magnitude,
        Vector3 origin,
        float explosionRadius,
        float upwardsModifier,
        ForceMode forceMode
    );
}
