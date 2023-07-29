using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    /// <summary>
    /// Provides a report on what happens during TakeHeal()
    /// </summary>
    public readonly struct HealReport
    {
        // General information about the incident
        public readonly GameObject source;
        public readonly IHealable target;

        public readonly float time;
        public readonly Vector3 position;

        // Numbers from the damage calculations
        public readonly float preMitigationAmount;
        public readonly float postMitigationAmount;
        public readonly float changeToHP;
        public readonly float newHP;
        public readonly float newHPPercent;
        public readonly bool healedToFull;

        public HealReport(GameObject source, IHealable target, Vector3 position, float preMitigationAmount, float postMitigationAmount, StatsTracker.StatsTrackerReport statsTrackerReport)
        {
            this.source = source;
            this.target = target;
            this.time = Time.time;
            this.position = position;
            this.preMitigationAmount = preMitigationAmount;
            this.postMitigationAmount = postMitigationAmount;
            this.changeToHP = statsTrackerReport.delta;
            this.newHP = statsTrackerReport.valueAfter;
            this.newHPPercent = statsTrackerReport.percentAfter;
            this.healedToFull = statsTrackerReport.setToMaxValue;
        }

        public HealReport(HealPacket preMitigatedDamage, HealPacket postMitigatedDamage, IHealable target, Vector3 position, StatsTracker.StatsTrackerReport statsTrackerReport)
        {
            this.source = preMitigatedDamage.source;
            this.target = target;
            this.time = Time.time;
            this.position = position;
            this.preMitigationAmount = preMitigatedDamage.value;
            this.postMitigationAmount = postMitigatedDamage.value;
            this.changeToHP = statsTrackerReport.delta;
            this.newHP = statsTrackerReport.valueAfter;
            this.newHPPercent = statsTrackerReport.percentAfter;
            this.healedToFull = statsTrackerReport.setToMaxValue;
        }

        public HealReport(GameObject source, IHealable target, Vector3 position, float preMitigationAmount, float postMitigationAmount, float changeToHP,  float targetRemainingHP, float targetRemainingHPPercent, bool healedToFull)
        {
            this.source = source;
            this.target = target;
            this.time = Time.time;
            this.position = position;
            this.preMitigationAmount = preMitigationAmount;
            this.postMitigationAmount = postMitigationAmount;
            this.changeToHP = changeToHP;
            this.newHP = targetRemainingHP;
            this.newHPPercent = targetRemainingHPPercent;
            this.healedToFull = healedToFull;
        }

        public static HealReport NoHeal(GameObject source, IHealable target, Vector3 position, float preMitigationAmount, float targetRemainingHP, float targetRemainingHPPercent)
        {
            return new HealReport(source, target, position, preMitigationAmount, 0, 0, targetRemainingHP, targetRemainingHPPercent, false);
        }

    }
}


