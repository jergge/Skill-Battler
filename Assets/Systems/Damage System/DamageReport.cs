using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    /// <summary>
    /// Provides a report on what happens during TakeDamage()
    /// </summary>
    public readonly struct DamageReport
    {
        // General information about the incident
        public readonly GameObject source;
        public readonly IDamageable target;

        public readonly float time;
        public readonly Vector3 position;

        // Numbers from the damage calculations
        public readonly float preMitigationAmount;
        public readonly float postMitigationAmount;
        public readonly float damageToShields;
        public readonly float damageToHP;
        public readonly float targetRemainingHP;
        public readonly float targetRemainingHPPercent;
        public readonly bool lethalHit;

        public DamageReport(GameObject source, IDamageable target, Vector3 position, float preMitigationAmount, float postMitigationAmount, float damageToShields, StatsTracker.StatsTrackerReport statsTrackerReport)
        {
            this.source = source;
            this.target = target;
            this.time = Time.time;
            this.position = position;
            this.preMitigationAmount = preMitigationAmount;
            this.postMitigationAmount = postMitigationAmount;
            this.damageToHP = statsTrackerReport.delta;
            this.damageToShields = damageToShields;
            this.targetRemainingHP = statsTrackerReport.valueAfter;
            this.targetRemainingHPPercent = statsTrackerReport.percentAfter;
            this.lethalHit = statsTrackerReport.lethalHit;
        }

        public DamageReport(DamagePacket preMitigatedDamage, DamagePacket postMitigatedDamage, IDamageable target, Vector3 position, float damageToShields, StatsTracker.StatsTrackerReport statsTrackerReport)
        {
            this.source = preMitigatedDamage.source;
            this.target = target;
            this.time = Time.time;
            this.position = position;
            this.preMitigationAmount = preMitigatedDamage.value;
            this.postMitigationAmount = postMitigatedDamage.value;
            this.damageToHP = statsTrackerReport.delta;
            this.damageToShields = damageToShields;
            this.targetRemainingHP = statsTrackerReport.valueAfter;
            this.targetRemainingHPPercent = statsTrackerReport.percentAfter;
            this.lethalHit = statsTrackerReport.lethalHit;
        }

        public DamageReport(GameObject source, IDamageable target, Vector3 position, float preMitigationAmount, float postMitigationAmount, float damageToShields, float damageToHP,  float targetRemainingHP, float targetRemainingHPPercent, bool lethalHit)
        {
            this.source = source;
            this.target = target;
            this.time = Time.time;
            this.position = position;
            this.preMitigationAmount = preMitigationAmount;
            this.postMitigationAmount = postMitigationAmount;
            this.damageToHP = damageToHP;
            this.damageToShields = damageToShields;
            this.targetRemainingHP = targetRemainingHP;
            this.targetRemainingHPPercent = targetRemainingHPPercent;
            this.lethalHit = lethalHit;
        }

        public static DamageReport NoDamge(GameObject source, IDamageable target, Vector3 position, float preMitigationAmount, float targetRemainingHP, float targetRemainingHPPercent)
        {
            return new DamageReport(source, target, position, preMitigationAmount, 0, 0, 0, targetRemainingHP, targetRemainingHPPercent, false);
        }

    }
}
