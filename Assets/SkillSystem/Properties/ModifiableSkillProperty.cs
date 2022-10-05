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
        radius
    };

    public static float GetModifiedValue(ModifyValue value, float baseValue , GameObject source) 
    {
        var allMods = source.GetComponents<IModifySkillProperty>();
        //Debug.Log("BASE Property Value is  " + baseValue + " all mods length is: " + allMods.Length);
        float modifiedValue = baseValue;

        foreach ( var list in allMods )
        {
            //Debug.Log("in first foreach loop");
            foreach (var func in list.GetPropertyModifiers())
            {
                modifiedValue = (func.value == value) ? func.Evaluate(modifiedValue) : modifiedValue;
                if (func.value == ModifyValue.damage)
                {
                    //Debug.Log("mod Property Value is now " + modifiedValue);    
                }
            }
        }
        // Debug.Log(this.ToString());
        return modifiedValue;
    }
    public static int GetModifiedValueInt(ModifyValue value, float baseValue , GameObject source)
         => Mathf.RoundToInt( GetModifiedValue(  value,  baseValue ,  source ) );


}}