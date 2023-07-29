using System;
using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using UnityEngine;

namespace SkillSystem
{
    /// <summary>
    /// This is a skill that deals damage to a target
    /// </summary>
    public interface IDamagingSkill : IOnDealDamageEvents
    {
        public DamageReport? DealDamageTo(DamagePacket damageUnit, IDamageable target)
        {
            return target.TakeDamage(damageUnit);
        }
    }
}
