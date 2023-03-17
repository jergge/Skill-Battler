using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public class DamageUnit
    {
        public readonly float baseAmount;

        [Flags]
        public enum DamageType {
            pure    = 1 ,
            fire    = 2,
            water   = 4,
            wind    = 8,
            earth   = 16,
            solar   = 32,
            lunar   = 64,
            holy    = 128,
            shadow  = 256,

            steam   = fire  | water,
            scorch  = fire  | wind,
            lava    = fire  | earth,
            vapour  = water | wind,
            mud     = water | earth,
            nature  = wind  | earth,
            astral  = solar | lunar
        };

        public readonly DamageType damageType;

        public readonly GameObject source;

        public DamageUnit(float baseAmount, DamageType damageType, GameObject source)
        {
            this.baseAmount = baseAmount;
            this.damageType = damageType;
            this.source = source;
        }

        /// <summary>
        /// Combine 2 DamageUnits into one. Adds baseAmounts together and 'bitwise ors' the damageTypes.
        /// The source of a will be used.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static DamageUnit Combine (DamageUnit a, DamageUnit b)
        {
            if ( a.source != b.source)
            {
                throw new NotSupportedException("Cannot combine DamgeUnits from different sources");
            }
            float newBaseAmoumt = a.baseAmount + b.baseAmount;
            DamageType newDamageType = a.damageType | b.damageType;
            return new DamageUnit(newBaseAmoumt, newDamageType, a.source);
        }

        public DamageUnit Combine (DamageUnit other)
        {
            return Combine(this, other);
        }

        public static explicit operator float(DamageUnit unit)
        {
            return unit.baseAmount;
        }

        public static explicit operator DamageUnit(float n)
        {
            return Default(n);
        }

        public static DamageUnit Default(float amount) => new DamageUnit(amount, 0, null);
    }
}