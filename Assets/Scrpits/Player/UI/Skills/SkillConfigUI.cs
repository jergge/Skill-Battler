using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;
using UnityEngine.UI;

namespace SkillSystem
{
    public class SkillConfigUI : PlayerUI
    {
        public SkillManager skillManager;

        public SkillDisplayPrefab skillSlot1;
        public SkillDisplayPrefab skillSlot2;
        public SkillDisplayPrefab skillSlot3;
        public SkillDisplayPrefab skillSlot4;

        List<Skill> allSkills = new List<Skill>();
        public List<SkillDisplayPrefab> allPrefabs = new List<SkillDisplayPrefab>();

        public SkillDisplayPrefab skillDisplayPrefab;
        public GameObject allSkillsSection;
        public TooltipDisplay tooltipDisplay;

        //public MenuManager menuManager;

        void OnEnable()
        {
            UseMenuInputMap();
            UnlockMouse();
            allSkills = skillManager.allSkills;
            skillSlot1.Configure(skillManager.attack);
            skillSlot2.Configure(skillManager.block);
            skillSlot3.Configure(skillManager.skill1);
            skillSlot4.Configure(skillManager.skill2);

            foreach (var skill in allSkills)
            {
                SkillDisplayPrefab skillIcon = GameObject.Instantiate(skillDisplayPrefab);
                skillIcon.transform.SetParent(allSkillsSection.transform);
                skillIcon.Configure(skill);
                skillIcon.OnMouseOver += ShowSkillTooltip;
                allPrefabs.Add(skillIcon);
            }
        }

        void OnDisable()
        {
            // Debug.Log("calling on disable");
            foreach (var prefab in allPrefabs)
            {
                prefab.OnMouseOver -= ShowSkillTooltip;
                // Debug.Log("Destroying " + prefab);
                // Destroy(prefab);
                prefab.Destroy();
            }

            allPrefabs.Clear();
        }

        void ShowSkillTooltip(Skill skill)
        {
            tooltipDisplay.ShowTooltip(skill);
        }

    }
}
