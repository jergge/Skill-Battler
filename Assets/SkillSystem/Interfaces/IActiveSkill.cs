using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem{
public interface IActiveSkill
{
    public void Cast(Transform spawnLoaction, TargetInfo targetInfo);

}}
