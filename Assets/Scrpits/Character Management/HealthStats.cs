using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStats : MonoBehaviour
{
    public int baseValue = 100;
    public int bonus;
    int current;
    public int regenPerSecond;
    
    int max => baseValue + bonus;
    public float currentPercent => current / (float)(baseValue + bonus);

    void Start()
    {
        current = baseValue;
    }

    public (int delta, int current, int max, float percent) SubtractHP(int d)
    {
        int delta = current;
        current -= d;
        if (current <=0)
        {
            current = 0;
        }
        
        delta -= current;
        return (delta, current, max, current/(float)max);
    }

    public bool AddHP(int d)
    {
        current += d;
        if (current >= max)
        {
            current = max;
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
        if (current > max)
        {
            current = max;
        }
    }
}