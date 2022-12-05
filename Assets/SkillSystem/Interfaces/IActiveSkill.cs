using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem{
public interface IActiveSkill
{
    /// <summary>
    /// Function called when the user activates the Skill
    /// </summary>
    /// <param name="spawnLoaction"></param>
    /// <param name="targetInfo"></param>
    public void Cast(Transform spawnLoaction, TargetInfo targetInfo);
}}
