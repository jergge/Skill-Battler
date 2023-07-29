using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public class HealPacket : ActionUnit
    {

        public HealPacket(float baseAmount, Type type, GameObject source) : base(baseAmount, type, source)
        {        }

        /// <summary>
        /// Combine 2 HealUnits from the same source into one. Adds baseAmounts together and 'bitwise ors' the Types.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static HealPacket Combine (HealPacket a, HealPacket b)
        {
            if ( a.source != b.source)
            {
                throw new NotSupportedException("Cannot combine HealUnit from different sources");
            }
            float newBaseAmoumt = a.value + b.value;
            Type type = a.type | b.type;
            return new HealPacket(newBaseAmoumt, type, a.source);
        }

        public HealPacket Combine (HealPacket other)
        {
            return Combine(this, other);
        }

        public static explicit operator float(HealPacket unit)
        {
            return unit.value;
        }

        public static explicit operator HealPacket(float n)
        {
            return Default(n);
        }

        public static HealPacket Default(float amount) => new HealPacket(amount, 0, null);
    }
}