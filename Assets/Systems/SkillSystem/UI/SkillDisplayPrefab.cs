using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SkillSystem
{
    public class SkillDisplayPrefab : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Skill> OnClick;

        Skill skill;

        public Image icon;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(skill);
        }

        public void Configure(Skill? skill)
        {
            if (skill is null)
            {
                return;
            }
            this.skill = skill;
            this.icon.sprite = skill.icon;
            name = skill.skillName + " display prefab";
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}