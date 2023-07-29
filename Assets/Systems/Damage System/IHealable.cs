using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    /// <summary>
    ///  GameObject that can be healed
    /// </summary>
    public interface IHealable
    {
        /// <summary>
        /// Performs healing on the GameObject
        /// </summary>
        /// <param name="healPacket">The healing to be given</param>
        /// <returns>Information about the actual healing done</returns>
        public abstract HealReport TakeHeal(HealPacket healPacket);
    }
}