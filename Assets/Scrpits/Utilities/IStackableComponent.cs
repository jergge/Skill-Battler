using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    /// <summary>
    /// 
    /// </summary>
    public interface UniqueComponent<TSelf> : UniqueComponent
    {
        public abstract void AddNew(TSelf newComponent);
        public abstract void AddNew();
    }

    public interface UniqueComponent
    {

    }
}
