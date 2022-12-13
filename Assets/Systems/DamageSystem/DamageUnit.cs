using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DamageSystem
{
    public class DamageUnit
    {
        public float baseAmount;
        [Flags]
        public enum DamageType {
            pure = 1 ,
            fire = 2,
            water = 4 
        };
        public DamageType damageType;
        public GameObject source;

        public DamageUnit(float baseAmount, DamageType damageType, GameObject source)
        {
            this.baseAmount = baseAmount;
            this.damageType = damageType;
            this.source = source;
        }

        /// <summary>
        /// Combine 2 DamageUnits into one. Adds baseAmounts together and 'ands' the damageTypes.
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

        // public static List<DamageUnit> operator + (DamageUnit a, DamageUnit b)
        // {
        //     List<DamageUnit> list = new List<DamageUnit>();
        //     list.Add(a);
        //     list.Add(b);

        //     return list;
        // }
        // #warning I don't thing this should be used..
        // public static List<DamageUnit> operator + (List<DamageUnit> list, DamageUnit b)
        // {
        //     list.Add(b);
        //     return list;
        // }
    }
}