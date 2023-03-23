using System;
using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using SkillSystem;
using UnityEngine;

namespace SkillSystem.Properties {
    
/// <summary>
/// Attatches to a GameObject (with a Skill Manager) and provides modifications to Skills of Type<T>
/// </summary>
/// <typeparam name="S">The Skill Type to modify</typeparam>
public abstract class IModifySkill<S> : Buff where S: Skill
{
    /// <summary>
    /// A Reference to this object's SkillManager
    /// </summary>
    protected SkillManager? skillManager;

    /// <summary>
    /// That skill that will be modified
    /// </summary>
    protected S skill;

    void Start()
    {
        if ( !gameObject.TryGetComponent<SkillManager>(out skillManager) )
        {
            Destroy(this);
        } else {
            TryFindSkill();
        }
    }

    void TryFindSkill()
    {
        if (skillManager.TryGetEnabledSkill<S>(out skill))
        {
            RegisterEvents();
        } else {
            skillManager.OnSkillEnabled += CheckIfEnabled;
        }
    }

    void CheckIfEnabled(Skill skill)
    {
        if (skill is S)
        {
            RegisterEvents();
            skillManager.OnSkillDisabled += CheckIfDisabled;
            skillManager.OnSkillEnabled -= CheckIfEnabled;
        }
    }

    void CheckIfDisabled(Skill skill)
    {
        if (skill == this.skill)
        {
            UnregisterEvents();
            skillManager.OnSkillEnabled += CheckIfEnabled;
            skillManager.OnSkillDisabled -= CheckIfDisabled;
        }
    }


    void RegisterEvents()
    {
        skillManager.OnAfterCast += WhenOnSkillCast;             
        if (skillManager.TryGetEnabledSkill<S>(out skill) )
        {
            skill.OnDealDamage += OnDealDamage;
        }
    }

    void UnregisterEvents()
    {
        skillManager.OnAfterCast -= WhenOnSkillCast;
        skill.OnDealDamage -= WhenOnDealDamage;        
    }



    private void WhenOnSkillCast(CastEventInfo castEventInfo)
    {
        if (castEventInfo.skill is S)
        {
            OnSkillCast(castEventInfo);
        }
    }
        protected virtual void OnSkillCast(CastEventInfo castEventInfo) { }

    private void WhenOnSkillCollisionOffload()
    {
        OnSkillCollisionOffload();
    }
        protected virtual void OnSkillCollisionOffload() { }

    private void WhenOnDealDamage(DamageInfo? damageInfo)
    {
        OnDealDamage(damageInfo);
    }
        protected virtual void OnDealDamage(DamageInfo? damageInfo) { }

    public override void Configure(SkillSystem.Skill skill)
    {
        throw new System.NotImplementedException();
    }

    void Destroy()
    {
        UnregisterEvents();
    }

}}
