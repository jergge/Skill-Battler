using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SkillSystem.Properties {
public class SkillPropertyModifier {
    public ModifiableSkillProperty.ModifyValue value;

    /// <summary>
    /// Fuction which determines how the base value is changed. Can be called in any order!
    /// </summary> 
    protected Func<float, float> changeFunction;
    public enum ModifierType {
        addToBase,
        multiplyBase,
        addAfterMultiply,
        multiplyfinal,
        immunity
    }

    public SkillPropertyModifier (ModifiableSkillProperty.ModifyValue value, Func<float, float> changeFuncton)
    {
        this.value = value;
        this.changeFunction = changeFuncton;
    }

    /// <summary>
    /// Applies the changeFunction to the input and returns the result
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public float Evaluate(float input) {        
        return changeFunction(input);
    } 
        
}}
