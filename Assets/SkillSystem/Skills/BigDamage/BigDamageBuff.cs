using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem.Properties;
using SkillSystem;

public class BigDamageBuff : Buff, IModifySkillProperty
{
    List<SkillPropertyModifier> temp = new List<SkillPropertyModifier>();

    void Start()
    {
        temp.Add(new SkillPropertyModifier(
            ModifiableSkillProperty.ModifyValue.damage,
            (x) => x = x*2
        ));

    }

    public List<SkillPropertyModifier> GetPropertyModifiers()
    {
        return temp;
    }

    
}
