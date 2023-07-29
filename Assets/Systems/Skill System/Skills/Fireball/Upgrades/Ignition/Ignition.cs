using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using SkillSystem.Properties;
using UnityEngine;

public class Ignition : IModifySkill<Fireball>
{

    void Update()
    {
        ReduceDuration();
    }

    protected override void OnIDamagableOffload(GameObject other)
    {
        Ignited buff = other.AddComponent<Ignited>();
        buff.CombineBuffs<Ignited>();
    }
}
