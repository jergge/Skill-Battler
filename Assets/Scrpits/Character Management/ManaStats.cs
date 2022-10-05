using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class ManaStats : MonoBehaviour 
{
    public float baseValue;
    public float bonus;
    float current;
    public float regenPerSecond;

    float max => baseValue + bonus;
    public float currentPercent => current / (float)(max);

    void Start()
    {
        // SkillManager skillManager = GetComponent<SkillManager>();
        current = baseValue;
        // skillManager.OnAfterCast += AfterCast;
        // skillManager.CanICast += EnoughToCast;
    }

    void Update()
    {
        Regen(regenPerSecond * Time.deltaTime);
    }

    (bool hadEnough, float current, float currentPercent) ApplyCost(int cost)
    {
        current -= cost;

        return (true, current, currentPercent);
    }

    public void Regen(float r)
    {
        current += r;
        current = Mathf.Min(current, max);
    }

    public void Reduce(float r)
    {
        current -= r;
        current = Mathf.Max(current, 0);
    }

    public float GetCurrent()
    {
        return current;
    }

    //cast Events:
    void EnoughToCast(CastEventInfo info, CheckForAny checker)
    {
        // if ( info.skill.cost > current )
        // {
        //     checker.False();
        // }
    }

    void AfterCast(CastEventInfo info)
    {
        // Debug.Log("That just cost " + info.skill.cost + " mana!");
        //current -= info.skill.cost;
    }

    //End cast Events

}
