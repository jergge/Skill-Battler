using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SkillSystem {
public class ActiveSkillsUI : MonoBehaviour
{
    public SkillManager skillManager;
    public SkillUI skillUIPrefab;

    Dictionary<Skill, SkillUI> enabledSkills = new Dictionary<Skill, SkillUI>();

    void Start()
    {

        skillManager.OnSkillEnabled += OnSkillEnabled;
        skillManager.OnSkillDisabled += OnSkillDisabled;
    }

    void OnSkillEnabled(Skill skill)
    {
        if (!enabledSkills.ContainsKey(skill))
        {
            enabledSkills.Add(skill, CreateUIElement(skill));
        }
    }

    void OnSkillDisabled(Skill skill)
    {
        enabledSkills.Remove(skill);
    }

    SkillUI CreateUIElement(Skill skill)
    {
        SkillUI skillUI = GameObject.Instantiate(skillUIPrefab);
        skillUI.transform.SetParent(transform);
        skillUI.Configure(skill);
        return skillUI;
    }
}}
