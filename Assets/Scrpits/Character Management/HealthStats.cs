using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Use the more generic StatsTracker instead")]
public class HealthStats : MonoBehaviour
{
    public float baseValue = 100;
    public float bonus;
    public float current { get; protected set; }
    public float regenPerSecond;
    
    float maxValue => baseValue + bonus;
    public float currentPercent => current / (float)maxValue;

    void Start()
    {
        current = baseValue;
    }

    void Update()
    {
        current = Mathf.Min(current + (regenPerSecond * Time.deltaTime), maxValue);
    }

    public (float delta, float current, float max, float percent) SubtractHP(int d)
    {
        float delta = current;
        current -= d;
        if (current <=0)
        {
            current = 0;
        }
        
        delta -= current;
        return (delta, current, maxValue, current/(float)maxValue);
    }

    public bool AddHP(int d)
    {
        current += d;
        if (current >= maxValue)
        {
            current = maxValue;
            return true;
        }
        return false;
    }

    public void AddBonusHP(int d)
    {
        current += d;
        bonus += d;
    }

    public void RemoveBonusHP(int d)
    {
        bonus -= d;
        if (current > maxValue)
        {
            current = maxValue;
        }
    }
}