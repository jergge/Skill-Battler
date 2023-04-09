using System.Collections;
using System.Collections.Generic;
using Jergge.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SkillSystem {
[RequireComponent(typeof(Image))]
public class SkillUI : MonoBehaviour
{
    Skill skill;
    float cooldownPercent => skill.remainingCooldown / skill.cooldown;
    public Text cooldownText;
    public Image icon;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Configure(Skill skill)
    {
        this.skill = skill;
        icon.sprite = skill.icon;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownText.text = Mathf.Clamp(skill.remainingCooldown, 0, float.MaxValue).ToString().Truncate(3);
    }
}}
