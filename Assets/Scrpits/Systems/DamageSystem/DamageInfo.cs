using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    /// <summary>
    /// Provides information on what happened to the receiver of the damage during their last TakeDamage call
    /// </summary>
    public struct DamageInfo
    {
        public bool wasLethalHit;
        public float damageAmount;
        public float realHPLost;
        public float remainingHP;

        [Obsolete("Better to pass in StatsTracker.InfoFromLastOperator instead")]
        public DamageInfo(bool lethalHit, float amountDone, float remainingHP, float damageAmount)
        {
            this.wasLethalHit = lethalHit;
            this.realHPLost = amountDone;
            this.remainingHP = remainingHP;
            this.damageAmount = damageAmount;
        }

        public DamageInfo(StatsTracker.InfoFromLastOperator info)
        {
            this.realHPLost = info.delta;
            this.wasLethalHit = info.isZeroOrLess;
            this.remainingHP = info.current;
            this.damageAmount = info.operatorValueInput;
        }
    }
}
