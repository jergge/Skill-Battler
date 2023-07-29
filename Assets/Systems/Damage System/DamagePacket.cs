using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public class DamagePacket : ActionUnit
    {

        public DamagePacket(float baseAmount, Type type, GameObject source) : base(baseAmount, type, source)
        {        }

        /// <summary>
        /// Combine 2 DamagePacket into one. Adds baseAmounts together and 'bitwise ors' the damageTypes.
        /// The source of a will be used.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static DamagePacket Combine (DamagePacket a, DamagePacket b)
        {
            if ( a.source != b.source)
            {
                throw new NotSupportedException("Cannot combine DamgeUnits from different sources");
            }
            float newBaseAmoumt = a.value + b.value;
            Type type = a.type | b.type;
            return new DamagePacket(newBaseAmoumt, type, a.source);
        }

        public DamagePacket Combine (DamagePacket other)
        {
            return Combine(this, other);
        }

        public static explicit operator float(DamagePacket unit)
        {
            return unit.value;
        }

        public static explicit operator DamagePacket(float n)
        {
            return Default(n);
        }

        public static DamagePacket Default(float amount) => new DamagePacket(amount, 0, null);

        protected override void ModifyOutgoing()
        {
            if (source is null)
            {
                return;
            }

            var mods = source.GetComponents<IModifyOutgoing<DamagePacket>>();

            foreach (var mod in mods)
            {
                mod.ModifyOutgoing(this);
            }
        }
    }
}