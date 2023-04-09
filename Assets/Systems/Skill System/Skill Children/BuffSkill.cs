using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;

public abstract class BuffSkill<T> : Skill where T : Buff
{
    T buff;

    public override void Enabled()
    {
        if (buff is null)
        {
            buff = source.AddComponent<T>();
        }
        buff?.gameObject.SetActive(true);
    }

    public override void Disabled()
    {
        buff?.gameObject.SetActive(false);
    }
}
