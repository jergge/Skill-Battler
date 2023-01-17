using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    public abstract class Buff : MonoBehaviour
    {
        public abstract void Configure(Skill skill);
    }
}
