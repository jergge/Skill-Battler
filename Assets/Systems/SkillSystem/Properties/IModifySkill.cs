using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using UnityEngine;

namespace SkillSystem.Properties {
public abstract class IModifySkill<T> : Buff where T:Skill
{
    SkillManager skillManager;
    protected T skill;
    
    void Start()
    {
        if ( gameObject.TryGetComponent<SkillManager>(out skillManager) )
        {
            skillManager.OnAfterCast += WhenOnSkillCast;

            if ( skillManager.TryGetActiveSkill<T>(out skill) )
            {
                skill.OnDealDamage += OnDealDamage;
            }
        }
    }

    private void WhenOnSkillCast(CastEventInfo info)
    {
        if ( info.skill is T )
        {
            OnSkillCast(info);
        }
    }
    protected virtual void OnSkillCast(CastEventInfo castEventInfo) { }

    // private void WhenOnSkillCollisionOffload()
    // {
    //     OnSkillCollisionOffload();
    // }
    protected virtual void OnSkillCollisionOffload() { }

    // private void WhenOnDealDamage(DamageInfo damageInfo)
    // {
    //     OnDealDamage(damageInfo);
    // }
    protected virtual void OnDealDamage(DamageInfo? damageInfo) { }

    public override void AddStack(int count)
    {
        throw new System.NotImplementedException();
    }

    public override void Configure(Skill skill)
    {
        throw new System.NotImplementedException();
    }

    void Destroy()
    {
        skillManager.OnAfterCast -= WhenOnSkillCast;
        skill.OnDealDamage -= OnDealDamage;
    }

}}
