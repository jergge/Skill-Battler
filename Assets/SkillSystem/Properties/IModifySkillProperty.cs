using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem.Properties {
public interface IModifySkillProperty {
    public List<SkillPropertyModifier> GetPropertyModifiers();
}}
