using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SkillSystem.Properties {
public static class ModifiableSkillProperty { 
    
    public enum ModifyValue {
        damage,
        range,
        speed,
        cooldownRate,
        radius,
        sizeScale,
        cost
    };

    public static float GetModifiedValue(ModifyValue value, float baseValue , GameObject source) 
    {
        var allMods = source.GetComponents<IModifySkillProperty>();
        //Debug.Log("BASE Property Value is  " + baseValue + " all mods length is: " + allMods.Length);
        float modifiedValue = baseValue;

        foreach ( var list in allMods )
        {
            //Debug.Log("in first foreach loop");
            foreach (var function in list.GetPropertyModifiers(value))
            {
                modifiedValue = (function.value == value) ? function.Evaluate(modifiedValue) : modifiedValue;
            }
        }
        // Debug.Log(this.ToString());
        return modifiedValue;
    }
}}