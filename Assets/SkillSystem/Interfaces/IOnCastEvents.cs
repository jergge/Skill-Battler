using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    //including costs that get paid???? somewhere this is happening....???!
    public event Action<CastEventInfo> OnAfterCast;
}

#nullable enable
public class CastEventInfo
{
    public GameObject source;
    public Skill skill;
    public GameObject? target;

    public CastEventInfo(GameObject source, Skill skill, GameObject? target)
    {
        this.source = source;
        this.skill = skill;
        this.target = target;
    }
}}
#nullable restore