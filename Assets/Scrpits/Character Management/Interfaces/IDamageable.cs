using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public DamageInfo TakeDamage(int damage);
    public bool IsDead();
}

public struct DamageInfo 
{
    public bool lethalHit;
    public int amountDone;
    public int remainingHP;

    public DamageInfo(bool lethalHit, int amountDone, int remainingHP)
    {
        this.lethalHit = lethalHit;
        this.amountDone = amountDone;
        this.remainingHP = remainingHP;
    }
}
