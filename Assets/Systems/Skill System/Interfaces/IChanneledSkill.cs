using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SkillSystem {
public interface IChanneledSkill : IActiveSkill
{
    public void EndChannel();

    public event Action<Skill> CastEnded;
}} 