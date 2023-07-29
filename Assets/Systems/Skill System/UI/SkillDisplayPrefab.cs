using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SkillSystem
{
    public class SkillDisplayPrefab : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Skill> OnMouseOver;
        public event Action OnMouseOff;
        public event Action<Skill> OnMouseClick;

        Skill skill;

        public Image icon;

        [SerializeField] GameObject border;

        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("pointer click registered");
            OnMouseClick?.Invoke(skill);
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMouseOver?.Invoke(skill);
            ShowBorder(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnMouseOff?.Invoke();
            ShowBorder(false);
        }

        void ShowBorder(bool input)
        {
            if (input)
            {
                border.SetActive(true);
            } else {
                border.SetActive(false);
            }
        }
    }
}