using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    public readonly struct SkillTooltip
    {
        public readonly Sprite icon;
        public readonly string name;
        public readonly string basicDescription;
        public readonly float cost;
        public readonly float cooldown;

        public SkillTooltip(Skill skill)
        {
            this.icon = skill.icon;
            this.name = skill.skillName;
            this.basicDescription = skill.basicDescription;
            this.cost = skill.baseCost;
            this.cooldown = skill.cooldown;
        }


    }
}
