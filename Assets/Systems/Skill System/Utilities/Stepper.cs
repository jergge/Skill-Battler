using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SkillSystem.Properties;
using DamageSystem;

namespace SkillSystem.Utilities {

public class Stepper
{
    int duration;
    float damageSum = 0;
    IDamageable target;
    bool DestroyOnCompletion;
    int secondsRemaining;
    Skill script;

    public Stepper(
        int duration,
        IDamageable target,
        Skill script,
        bool destroyOnCompletion = false
    )
    {
        this.duration = duration;
        this.secondsRemaining = duration + 1;
        this.target = target;
        this.script = script;
        DestroyOnCompletion = destroyOnCompletion;
    }

    public void SummingDamage(float d)
    {
        damageSum += d;
    }

    public IEnumerator GetCoroutine()
    {
        while (secondsRemaining > 0)
        {
            target.TakeDamage( (DamagePacket)damageSum );
            damageSum = 0;
            secondsRemaining--;
            yield return new WaitForSeconds(1);
        }

        if (DestroyOnCompletion)
        {
            GameObject.Destroy(script);
        }
    }
}}