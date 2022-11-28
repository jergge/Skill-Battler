using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IDamageable
{
    public float SingleHitDestroyAmount = 10f;

    public bool IsDead()
    {
        throw new System.NotImplementedException();
    }

    public DamageInfo TakeDamage(int damage)
    {
        if ( damage >= SingleHitDestroyAmount )
        {
            Die();
            return new DamageInfo(true, damage, 0);
        }
        else return new DamageInfo(false, 0, Mathf.CeilToInt(SingleHitDestroyAmount));
    }

    protected void Die()
    {
        gameObject.SetActive(false);
    }
}
