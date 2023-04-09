using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SkillSystem.Properties 
{
    public interface IModifySkillProperty 
    {
        public List<SkillPropertyModifier>? GetPropertyModifiers(ModifiableSkillProperty.ModifyValue type);
    }
}
