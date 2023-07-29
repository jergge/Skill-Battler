using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    /// <summary>
    /// A GameObject that can be damaged.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Causes Damage to the GameObject
        /// </summary>
        /// <param name="damageUnit"></param>
        /// <returns></returns>
        public abstract DamageReport TakeDamage(DamagePacket damageUnit);

        public bool IsDead();
    }
}