using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public abstract class ActionUnit
    {
        public readonly float baseAmount;

        [Flags]
        public enum Type {
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

        public readonly Type type;

        public readonly GameObject source;

        public ActionUnit(float baseAmount, Type damageType, GameObject source)
        {
            this.baseAmount = baseAmount;
            this.type = damageType;
            this.source = source;
        }
    }
}