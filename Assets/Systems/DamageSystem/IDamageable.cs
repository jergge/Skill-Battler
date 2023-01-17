using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    /// <summary>
    /// Object can be damaged and healed
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// The default implementation calls TakeDamge(float damageUnit.baseAmount)
        /// </summary>
        /// <param name="damageUnit"></param>
        /// <returns></returns>
        public abstract DamageInfo? TakeDamage(DamageUnit damageUnit);

        public bool IsDead();

        public abstract DamageInfo? TakeHeal(DamageUnit healUnit);
    }
}