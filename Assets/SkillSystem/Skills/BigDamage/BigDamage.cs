using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class BigDamage : Skill
{
    BigDamageBuff buff;

    public override void OnStartInSpellbook()
    {
        buff = source.AddComponent<BigDamageBuff>();
    }

    void Destroy()
    {
        Destroy(buff);
    }

}
