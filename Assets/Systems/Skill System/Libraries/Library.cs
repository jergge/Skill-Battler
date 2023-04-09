using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(menuName = "Skill System/Library")]
    public class Library : ScriptableObject
    {
        public List<Skill> skills = new List<Skill>();
    }
}
