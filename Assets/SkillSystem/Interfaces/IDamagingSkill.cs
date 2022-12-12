using System;
using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using UnityEngine;

namespace SkillSystem
{
    public interface IDamagingSkill : IOnDealDamageEvents
    {
        new public event Action<DamageInfo?> OnDealDamage;

        public DamageInfo? DealDamageTo(DamageUnit damageUnit, IDamageable target)
        {
            var damageInfo = target.TakeDamage(damageUnit);
            //OnDealDamage?.Invoke(damageInfo);
            //OnDealDamage(damageInfo);
            return damageInfo;
        }
    }
}
