using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    public abstract class Buff : MonoBehaviour, IStackableComponent
    {
        public bool active = true;

        public float baseDuration;
        public float remainingTime;

        GameObject source;

        public event Action OnBuffExpired;
        public event Action OnBuffRemoved;

        public abstract void AddStack(int count);
        public abstract void Configure(Skill skill);
    }
}
