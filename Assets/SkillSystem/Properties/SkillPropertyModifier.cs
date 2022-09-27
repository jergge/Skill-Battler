using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SkillSystem { namespace Properties {
public class SkillPropertyModifier {
    public ModifiableSkillProperty.ModifyValue value; 
    protected Func<float, float> method;
    public enum ModifierType {
        addToBase,
        multiplyBase,
        addAfterMultiply,
        multiplyfinal,
        immunity
    }

    public SkillPropertyModifier (ModifiableSkillProperty.ModifyValue value, Func<float, float> method)
    {
        this.value = value;
        this.method = method;
    }

    public float Evaluate(float f) {        
        return method(f);
    } 
        
}}}
