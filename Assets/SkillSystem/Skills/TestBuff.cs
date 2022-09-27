using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using SkillSystem.Properties;

public class TestBuff : MonoBehaviour, IModifySkillProperty
{
    [Range(0,20)]public float multiplier;

    SkillPropertyModifier modifier;

    void Awake()
    {
        modifier = new SkillPropertyModifier(
            ModifiableSkillProperty.ModifyValue.damage,
            (damage) => 
            {
                float newDamage =  damage*multiplier;
                //Debug.Log(damage + " inside the lambda is now " + newDamage);
                return newDamage;
            }
        );
    }

    public List<SkillPropertyModifier> GetPropertyModifiers()
    {
        List<SkillPropertyModifier> list = new List<SkillPropertyModifier>();
        list.Add(modifier);
        //Debug.Log("accessing the test buff interface implementation");
        return list;
    }
}