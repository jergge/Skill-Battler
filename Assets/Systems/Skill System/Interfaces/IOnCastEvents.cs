using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem {
public interface IOnCastEvents
{
    // These should be called in the order written in this script
    // as this order will be the logic used by other events / scripts in the engine
    // also, good luck!

    //check that the caller can cast at the moment
    public event Action<CastEventInfo, CheckForAny> CanICast;

    //check for any triggers that fire before the spell is cast
    public event Action<CastEventInfo> OnBeforeCast;

    //check for any triggers after the spell in cast
    public event Action<CastEventInfo> OnAfterCast;
}

#nullable enable
public class CastEventInfo
{
    public readonly GameObject source;
    public readonly Skill skill;
    public readonly GameObject? target;

    public CastEventInfo(GameObject source, Skill skill, GameObject? target)
    {
        this.source = source;
        this.skill = skill;
        this.target = target;
    }

    public override string ToString()
    {
        string message = source.name + " fired " + skill.name + " was fired at: " + ((target is null) ? "nothing in particualr" : target.name);
        return message;
    }
}}
#nullable restore