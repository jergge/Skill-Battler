using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using SkillSystem.Properties;
using UnityEngine;

public class BigDamageBuff : Buff, IModifySkillProperty
{
    List<SkillPropertyModifier> temp = new List<SkillPropertyModifier>();


    void Start()
    {
        temp.Add(new SkillPropertyModifier(
            ModifiableSkillProperty.ModifyValue.damage,
            (damage) => damage = damage*2
        ));

    }

    public List<SkillPropertyModifier>? GetPropertyModifiers(ModifiableSkillProperty.ModifyValue type)
    {
        if (type == ModifiableSkillProperty.ModifyValue.damage)
        {
            return temp;
        }
        return null;
    }

    public override void Configure(Skill skill)
    {
        throw new System.NotImplementedException();
    }

    public override void AddStack(int count)
    {
        throw new System.NotImplementedException();
    }

}
