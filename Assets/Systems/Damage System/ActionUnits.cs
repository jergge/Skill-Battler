using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public abstract class ActionUnit
    {
        public readonly float value;

        [Flags]
        public enum Type {
            none = 0,
            pure    = 1 ,
            fire    = 2,
            water   = 4,
            wind    = 8,
            earth   = 16,
            solar   = 32,
            lunar   = 64,
            holy    = 128,
            shadow  = 256,

            steam       = fire      |   water,
            scorch      = fire      |   wind,
            lava        = fire      |   earth,
            vapour      = water     |   wind,
            mud         = water     |   earth,
            nature      = wind      |   earth,
            astral      = solar     |   lunar,
            light       = holy      |   solar,
            dark        = shadow    |   lunar,
            elemental   = steam     |   nature
        };

        public readonly Type type;

        public readonly GameObject source;

        public ActionUnit(float baseAmount, Type damageType, GameObject source)
        {
            this.value = baseAmount;
            this.type = damageType;
            this.source = source;
        }

        public bool TypeContains(Type other)
        {
            if (this.type.HasFlag(other))
            {
                return true;
            }

            return false;
        }

        public bool TypeIsExactly(Type other)
        {
            if (other == this.type)
            {
                return true;
            }

            return false;
        }
    }
}