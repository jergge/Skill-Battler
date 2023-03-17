using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTracker : MonoBehaviour
{
    //public string niceName;

    public enum StatType { Health, Mana, Energy };
    public StatType statType = StatType.Mana;

    public float baseValue = 100;
    public float bonus;
    public float current { get; protected set; }
    public float regenPerSecond;

    float maxValue => baseValue + bonus;
    public float currentPercent => current / (float)maxValue;

    /// <summary>
    /// Holds information about the last change (+ or - operation) that occured
    /// </summary>
    public InfoFromLastOperator afterLastChange;

    void Start()
    {
        current = baseValue;
    }

    void Update()
    {
        current = Mathf.Min(current + (regenPerSecond * Time.deltaTime), maxValue);
    }

    /// <summary>
    /// Increses the stat's current amount
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static StatsTracker operator + (StatsTracker stats, float value)
    {
        float valueBefore = stats.current;
        stats.current = Mathf.Min(stats.current + value, stats.maxValue);
        stats.afterLastChange = new InfoFromLastOperator
            {preMitigationChange = value, postMitigationChange = stats.current - valueBefore, isZeroOrLess = (stats.current <= 0)? true : false, current = stats.current, percent = stats.currentPercent };
        return stats;
    }

    /// <summary>
    /// Reduces the stat's current amount
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static StatsTracker operator - (StatsTracker stats, float input)
    {
        float valueBefore = stats.current;
        stats.current = Mathf.Max(stats.current - input, 0);
        stats.afterLastChange = new InfoFromLastOperator
            {preMitigationChange = input, postMitigationChange = valueBefore - stats.current, isZeroOrLess = (stats.current <= 0)? true : false, current = stats.current, percent = stats.currentPercent };
        return stats;
    }

    public struct InfoFromLastOperator
    {
        public float preMitigationChange;
        public float postMitigationChange;
        public bool isZeroOrLess;
        public float current;
        public float percent;
    }
}
