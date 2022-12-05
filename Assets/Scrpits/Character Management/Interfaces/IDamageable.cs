using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public DamageInfo TakeDamage(float damage);
    public bool IsDead();
}

public struct DamageInfo 
{
    public bool lethalHit;
    public float amountDone;
    public float remainingHP;

    public DamageInfo(bool lethalHit, float amountDone, float remainingHP)
    {
        this.lethalHit = lethalHit;
        this.amountDone = amountDone;
        this.remainingHP = remainingHP;
    }

    public DamageInfo(StatsTracker.InfoFromLastOperator info)
    {
        this.amountDone = info.delta;
        this.lethalHit = info.isZeroOrLess;
        this.remainingHP = info.current;
    }
}
