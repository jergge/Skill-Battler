using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jergge.Extensions;

namespace SkillSystem {
public class SkillUI : MonoBehaviour
{
    public Skill skill;
    float cooldownPercent => skill.remainingCooldown / skill.cooldown;
    public Text cooldownText;
    // Start is called before the first frame update
    void Start()
    {
        Image img;
        if (TryGetComponent<Image>(out img))
        {
            img.sprite = skill.icon;
            cooldownText.text = Mathf.Clamp(skill.remainingCooldown, 0, float.MaxValue).ToString().Truncate(3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}}
