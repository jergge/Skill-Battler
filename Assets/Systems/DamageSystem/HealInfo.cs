using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    /// <summary>
    /// Provides information on what happened to the receiver of the damage during their last TakeDamage call
    /// </summary>
    public readonly struct HealInfo
    {
        public readonly float healingSent;
        public readonly float healingTaken;
        public readonly float remainingHP;

        [Obsolete("Better to pass in StatsTracker.InfoFromLastOperator")]
        public HealInfo(bool lethalHit, float amountDone, float remainingHP, float damageAmount)
        {
            this.healingSent = damageAmount;
            this.healingTaken = amountDone;
            this.remainingHP = remainingHP;
        }

        public HealInfo(StatsTracker.InfoFromLastOperator info)
        {
            this.healingSent = info.preMitigationChange;
            this.healingTaken = info.postMitigationChange;
            this.remainingHP = info.current;
        }
    }
}

