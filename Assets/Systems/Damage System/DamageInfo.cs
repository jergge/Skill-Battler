using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    /// <summary>
    /// Provides information on what happened to the receiver of the damage during their last TakeDamage call
    /// </summary>
    public readonly struct DamageInfo
    {
        public readonly float damageSent;
        public readonly float damageTaken;
        public readonly float remainingHP;
        public readonly bool wasLethalHit;

        [Obsolete("Better to pass in StatsTracker.InfoFromLastOperator")]
        public DamageInfo(bool lethalHit, float amountDone, float remainingHP, float damageAmount)
        {
            this.damageSent = damageAmount;
            this.damageTaken = amountDone;
            this.remainingHP = remainingHP;
            this.wasLethalHit = lethalHit;
        }

        public DamageInfo(StatsTracker.InfoFromLastOperator info)
        {
            this.damageSent = info.preMitigationChange;
            this.damageTaken = info.postMitigationChange;
            this.remainingHP = info.current;
            this.wasLethalHit = info.isZeroOrLess;
        }
    }
}
