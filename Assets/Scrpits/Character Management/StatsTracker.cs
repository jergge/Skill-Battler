using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using System;

public class StatsTracker : MonoBehaviour 
{
    public float baseValue;
    public float bonus;
    public float current { get; protected set; }
    public float regenPerSecond;

    float maxValue => baseValue + bonus;
    public float currentPercent => current / (float)(maxValue);

    void Start()
    {
        current = baseValue;
    }

    void Update()
    {
        current = Mathf.Min(current + (regenPerSecond * Time.deltaTime), maxValue);
    }

    (bool hadEnough, float current, float currentPercent) ApplyCost(int cost)
    {
        current -= cost;

        return (true, current, currentPercent);
    }

    [Obsolete("Use the overloaded '+' operator instead")]
    public void Regen(float r)
    {
        current += r;
        current = Mathf.Min(current, maxValue);
    }

    /// <summary>
    /// Increses the stat's current amount
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static StatsTracker operator + (StatsTracker stats, float a)
    {
        stats.current = Mathf.Min(stats.current + a, stats.maxValue);
        return stats;
    }

    /// <summary>
    /// Reduces the stat's current amount
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static StatsTracker operator - (StatsTracker stats, float a)
    {
        stats.current = Mathf.Max(stats.current - a, 0);
        return stats;
    }

    [Obsolete("Use overloaded '-' operator instead")]
    public void Reduce(float r)
    {
        current -= r;
        current = Mathf.Max(current, 0);
    }

    [Obsolete("Directly access 'current' instead")]
    public float GetCurrent()
    {
        return current;
    }

    [Obsolete("No idea, will get rid of this soon!")]
    void EnoughToCast(CastEventInfo info, CheckForAny checker)
    {    }

    [Obsolete("No idea, will get rid of this soon!")]
    void AfterCast(CastEventInfo info)
    {    }
}
