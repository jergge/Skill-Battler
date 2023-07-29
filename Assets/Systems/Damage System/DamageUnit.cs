using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public class DamageUnit : ActionUnit
    {

        public DamageUnit(float baseAmount, Type type, GameObject source) : base(baseAmount, type, source)
        {        }

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
            Type type = a.type | b.type;
            return new DamageUnit(newBaseAmoumt, type, a.source);
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

        protected override void ModifyOutgoing()
        {
            if (source is null)
            {
                return;
            }

            var mods = source.GetComponents<IModifyOutgoing<DamageUnit>>();

            foreach (var mod in mods)
            {
                mod.ModifyOutgoing(this);
            }
        }
    }
}