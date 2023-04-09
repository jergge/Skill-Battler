using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public class HealUnit : ActionUnit
    {

        public HealUnit(float baseAmount, Type type, GameObject source) : base(baseAmount, type, source)
        {        }

        /// <summary>
        /// Combine 2 HealUnits from the same source into one. Adds baseAmounts together and 'bitwise ors' the damageTypes.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static HealUnit Combine (HealUnit a, HealUnit b)
        {
            if ( a.source != b.source)
            {
                throw new NotSupportedException("Cannot combine HealUnit from different sources");
            }
            float newBaseAmoumt = a.baseAmount + b.baseAmount;
            Type type = a.type | b.type;
            return new HealUnit(newBaseAmoumt, type, a.source);
        }

        public HealUnit Combine (HealUnit other)
        {
            return Combine(this, other);
        }

        public static explicit operator float(HealUnit unit)
        {
            return unit.baseAmount;
        }

        public static explicit operator HealUnit(float n)
        {
            return Default(n);
        }

        public static HealUnit Default(float amount) => new HealUnit(amount, 0, null);

        protected override void ModifyOutgoing()
        {
            if (source is null)
            {
                return;
            }

            var mods = source.GetComponents<IModifyOutgoing<HealUnit>>();

            foreach (var mod in mods)
            {
                mod.ModifyOutgoing(this);
            }
        }
    }
}