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
    public StatsTrackerReport eventReport;

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
        float valueBeforeOpperation = stats.current;
        float percentBeforeOpperation = stats.currentPercent;
        stats.current = Mathf.Min(stats.current + value, stats.maxValue);
        stats.eventReport = new StatsTrackerReport
        {
            // amountChanged = value, 
            // postMitigationChange = stats.current - valueBefore, 
            // isZeroOrLess = (stats.current <= 0)? true : false, 
            // current = stats.current, 
            // percent = stats.currentPercent 
            valueBefore = valueBeforeOpperation,
            percentBefore = percentBeforeOpperation,
            valueAfter = stats.current,
            percentAfter = stats.currentPercent,
            delta = Mathf.Abs(valueBeforeOpperation - stats.current),
            percentDelta = Mathf.Abs(percentBeforeOpperation - stats.currentPercent),
            //lethalHit = (stats.current == 0 && stats.statType == StatType.Health)
            setToMaxValue = (stats.current == stats.maxValue)
        };
        return stats;
    }

    /// <summary>
    /// Reduces the stat's current amount
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="inputValue"></param>
    /// <returns></returns>
    public static StatsTracker operator - (StatsTracker stats, float inputValue)
    {
        float valueBeforeOpperation = stats.current;
        float percentBeforeOpperation = stats.currentPercent;
        stats.current = Mathf.Max(stats.current - inputValue, 0);
        stats.eventReport = new StatsTrackerReport 
        {
            // amountChanged = inputValue, 
            // postMitigationChange = valueBeforeOpperation - stats.current, 
            // isZeroOrLess = (stats.current <= 0)? true : false, 
            // current = stats.current, 
            // percent = stats.currentPercent 

            valueBefore = valueBeforeOpperation,
            percentBefore = percentBeforeOpperation,
            valueAfter = stats.current,
            percentAfter = stats.currentPercent,
            delta = Mathf.Abs(valueBeforeOpperation - stats.current),
            percentDelta = Mathf.Abs(percentBeforeOpperation - stats.currentPercent),
            lethalHit = (stats.current == 0 && stats.statType == StatType.Health)
        };
        return stats;
    }

    public struct StatsTrackerReport
    {
        /// <summary>
        /// 
        /// </summary>
        public float valueBefore;
        public float percentBefore;
        public float valueAfter;
        public float percentAfter;
        public float delta;
        public float percentDelta;
        public bool lethalHit;
        public bool setToMaxValue;
    }
}
