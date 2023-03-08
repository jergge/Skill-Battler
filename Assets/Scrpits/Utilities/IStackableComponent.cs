using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    public interface IStackableComponent
    {
        //public int stackCount { get; set; }

        public abstract void AddStack(int count);
    }
}
