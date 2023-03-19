using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  SkillSystem
{
    
public class SkillManagementUI : PlayerUI
    {
        /// <summary>
        /// Skills that are active for the player, they will listen to their triggers, and can be cast if active
        /// </summary>
        public Canvas EquippedSkills;

        /// <summary>
        /// Skills that the player knows, but that are not active in the world at all
        /// </summary>
        public Canvas KnownSkills;

    }

}   