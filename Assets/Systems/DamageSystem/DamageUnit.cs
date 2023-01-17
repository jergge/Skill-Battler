using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public class DamageUnit
    {
        public float baseAmount;

        [Flags]
        public enum DamageType {
            pure = 1 ,
            fire = 2,
            water = 4,
            wind = 8,
            earth = 16,
            solar = 32,
            lunar = 64,
            holy = 128,
            shadow = 256,

            steam   = fire  | water,
            scorch  = fire  | wind,
            lava    = fire  | earth,
            vapour  = water | wind,
            mud     = water | earth,
            nature  = wind  | earth

        };

        public DamageType damageType;

        public GameObject? source;

        public DamageUnit(float baseAmount, DamageType damageType, GameObject source)
        {
            this.baseAmount = baseAmount;
            this.damageType = damageType;
            this.source = source;
        }

        /// <summary>
        /// Combine 2 DamageUnits into one. Adds baseAmounts together and 'bitwise ands' the damageTypes.
        /// The source of a will be used.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static DamageUnit operator * (DamageUnit a, DamageUnit b)
        {
            float newBaseAmoumt = a.baseAmount + b.baseAmount;
            DamageType newDamageType = a.damageType & b.damageType;
            return new DamageUnit(newBaseAmoumt, newDamageType, a.source);
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