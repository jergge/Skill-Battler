using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    /// <summary>
    /// Attatches to a GameObject and provides a benifit
    /// </summary>
    public abstract class Buff : MonoBehaviour, UniqueComponent
    {
        [Header("Base Class - Buff settings")]
        //public bool active = true;

        public float baseDuration = Mathf.Infinity;
        public float remainingTime = Mathf.Infinity;
        
        protected void ReduceDuration()
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                OnBuffExpired?.Invoke();
                Destroy(this);
            }
        }

        GameObject source;

        public event Action OnBuffExpired;
        public event Action OnBuffRemoved;

        public abstract void Configure(Skill skill);

        protected void UseMaxTime()
        {
            
        }
    }
}
