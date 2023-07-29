using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    public abstract class UniqueBuff<Self> : Buff where Self : UniqueBuff<Self>
    {
        /// <summary>
        /// Provides a means of handeling multiple buffs of the same type on a GameObject.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        public S CombineBuffs<S>()
            where S : Self
        {
            var allS = gameObject.GetComponents<S>();

            if (allS.Length == 1)
            {
                return (S)this;
            }

            if (allS[0] == this)
            {
                return CombineTwoBuffs(allS[1]);
            }
            else
            {
                return CombineTwoBuffs(allS[0]);
            }
        }

        protected abstract S CombineTwoBuffs<S>(S other)
            where S : Self;

        public override void Configure(Skill skill)
        {
            throw new System.NotImplementedException();
        }
    }
}