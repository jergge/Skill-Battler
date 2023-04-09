using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillSystem
{
    public class TooltipDisplay : MonoBehaviour
    {
        public Image image;
        public TMP_Text skillName;
        public TMP_Text descripton;
        public TMP_Text cost;
        public TMP_Text cooldown;
        
        public void ShowTooltip(Skill skill)
        {
            SkillTooltip tooltip = skill.GetToolip();
            image.sprite = tooltip.icon;
            skillName.text = tooltip.name;
            descripton.text = tooltip.basicDescription;
            cost.text = tooltip.cost.ToString();
            cooldown.text = tooltip.cooldown.ToString();
        }
    }
}
