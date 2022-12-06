using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DamageSystem
{
    /// <summary>
    /// This object can be 'damaged' in some way
    /// </summary>
    public interface IDamageable
    {
        public DamageInfo? TakeDamage(float damage);
        /// <summary>
        /// The default implementation calls TakeDamge(float damageUnit.baseAmount)
        /// </summary>
        /// <param name="damgeUnit"></param>
        /// <returns></returns>
        public virtual DamageInfo? TakeDamage(DamageUnit damgeUnit)
        {
            return TakeDamage(damgeUnit.baseAmount);
        }
        public bool IsDead();

        public DamageInfo? TakeHeal(float heal);

        public virtual DamageInfo? TakeHeal(DamageUnit healUnit)
        {
            return TakeHeal(healUnit.baseAmount);
        }
    }
}