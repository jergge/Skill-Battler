using System;
using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using SkillSystem.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem
{
    /// <summary>
    /// The base class for all Skills
    /// </summary>   
    public abstract class Skill : MonoBehaviour//, IOnDealDamageEvents
    {
        /// <summary>
        /// Who used or ownes the skill / where did the skill come from
        /// </summary>
        public GameObject source;
        [Obsolete("Get the property from Skill.source instead")]
        public GameObject GetSource() => source;

        /// <summary>
        /// The Skill's name (not the same as the Unity Object name in the inspector). Used for display text in game
        /// </summary>
        public string skillName;

        /// <summary>
        /// Used for display text in game to explain to the player what the Skill does
        /// </summary>
        public string basicDescription;

        /// <summary>
        /// A square icon that represents the Skill
        /// </summary>
        public Sprite icon;

        /// <summary>
        /// Which resource does the skill use;
        /// </summary>
        public StatsTracker.StatType? resourceType = null;

        /// <summary>
        /// How much of a resource it takes to use the skill
        /// </summary>
        public float baseCost;
        public float cost => GetModifiedValue(ModifiableSkillProperty.ModifyValue.cost, baseCost);

        public enum ValidTargets
        {
            All,
            Self,
            Others,
            Allies,
            Enemies
        };

        public ValidTargets validTargets;

        /// <summary>
        /// The base interval between successive uses of the Skill
        /// </summary>
        public float cooldown = 2;

        /// <summary>
        /// 
        /// </summary>
        protected float cooldownRate =>
            GetModifiedValue(ModifiableSkillProperty.ModifyValue.cooldownRate, 1);
        public float remainingCooldown { get; protected set; }

        /// <summary>
        /// Controlls whether or not the Skill will reduce its cooldown each frame
        /// </summary>
        private bool cooldownPaused = false;

        public event Action<DamageInfo?> OnDealDamage;

        protected void PauseCooldown()
        {
            cooldownPaused = true;
        }

        protected void ResumeCooldown()
        {
            cooldownPaused = false;
        }

        protected void TickCooldown()
        {
            if (!cooldownPaused && remainingCooldown != 0)
            {
                remainingCooldown = Mathf.Max(remainingCooldown - cooldownRate * Time.deltaTime, 0);
            }
        }

        /// <summary>
        /// Is the Skill currently cooling down? Is it unable to be used because of this?
        /// </summary>
        public bool CoolingDown()
        {
            // Debug.Log(remainingCooldown);
            return (remainingCooldown > 0) ? true : false;
        }

        /// <summary>
        /// Sets the Skill's current cooldown to its base cooldown
        /// </summary>
        protected void ResetCooldown()
        {
            remainingCooldown = cooldown;
        }

        /// <summary>
        /// Called by SkillManager when the skill is made active, or aquired directly to active skills
        /// </summary>
        public virtual void Enabled() { }

        /// <summary>
        /// Called by SkillManager when the skill is made inactive, or added directly to inactive skills (the spellbook)
        /// </summary>
        public virtual void Disabled() { }

        //protected int layerMaskFromTargets => 255;

        [System.Obsolete("Object inheriting from Skill should use IsValidTarget(GameObject target) instead")]
        /// <summary>
        /// Uses LayerMasks to determine if this Skill can interact with another object
        /// </summary>
        /// <param name="source">The source of the Skill</param>
        /// <param name="target">The object that the skill could interact with</param>
        /// <returns>True if the Skill can interact, false otherwise</returns>
        protected bool IsValidTarget(GameObject source, GameObject target)
        {
            //Debug.Log("Layer Comparison between [" + source.name + " on layer (" + source.layer + ")] and [" + target.name + " on layer (" + target.layer + ")] based on rule {" + validTargets + "}");
            if (target.layer == 0)
                return false;

            switch (validTargets)
            {
                case ValidTargets.All:
                    return true;

                case ValidTargets.Self:
                    if (source == target)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case ValidTargets.Others:
                    return true;

                case ValidTargets.Allies:
                    return (source.layer == target.layer);

                case ValidTargets.Enemies:
                    //Debug.Log("Enemy Layer Compaere");
                    return (source.layer != target.layer);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Uses LayerMasks to determine if this Skill can interact with another object
        /// </summary>
        /// <param name="target">The object that the skill could interact with</param>
        /// <returns>True if the Skill can interact, false otherwise</returns>
        protected bool IsValidTarget(GameObject target)
        {
            //Debug.Log("Layer Comparison between [" + source.name + " on layer (" + source.layer + ")] and [" + target.name + " on layer (" + target.layer + ")] based on rule {" + validTargets + "}");
            if (target.layer == 0)
                return false;

            switch (validTargets)
            {
                case ValidTargets.All:
                    return true;

                case ValidTargets.Self:
                    if (source == target)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case ValidTargets.Others:
                    return true;

                case ValidTargets.Allies:
                    return (source.layer == target.layer);

                case ValidTargets.Enemies:
                    //Debug.Log("Enemy Layer Compaere");
                    return (source.layer != target.layer);
                default:
                    return false;
            }
        }

        public static bool IsValidTarget(GameObject source, GameObject target, ValidTargets validTargets)
        {
            //Debug.Log("Layer Comparison between [" + source.name + " on layer (" + source.layer + ")] and [" + target.name + " on layer (" + target.layer + ")] based on rule {" + validTargets + "}");
            if (target.layer == 0)
                return false;

            switch (validTargets)
            {
                case ValidTargets.All:
                    return true;

                case ValidTargets.Self:
                    if (source == target)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case ValidTargets.Others:
                    return true;

                case ValidTargets.Allies:
                    return (source.layer == target.layer);

                case ValidTargets.Enemies:
                    //Debug.Log("Enemy Layer Compaere");
                    return (source.layer != target.layer);
                default:
                    return false;
            }
        }

        [System.Obsolete("Set the property with Skill.source instead")]
        /// <summary>
        /// Set's the Skill's source to a GameObject
        /// </summary>
        /// <param name="gameObject"></param>
        public void SetSource(GameObject gameObject)
        {
            source = gameObject;
        }

        /// <summary>
        /// Calculates any modifications to the stats of a Skill and returns the results without mutating the base stats
        /// </summary>
        /// <param name="propertyIdentifier"></param>
        /// <param name="baseAmount"></param>
        /// <returns></returns>
        protected float GetModifiedValue(ModifiableSkillProperty.ModifyValue propertyIdentifier, float baseAmount)
        {
            return ModifiableSkillProperty.GetModifiedValue(propertyIdentifier, baseAmount, source);
        }

        /// <summary>
        /// Finds all objects of a certian type withint a certain range
        /// </summary>
        /// <typeparam name="T">The MonoBehaviour to look for</typeparam>
        /// <param name="range">The range to search in</param>
        /// <returns></returns>
        protected List<T> GetInDistancePhysics<T>(float range) where T : MonoBehaviour
        {
            var overlapSphere = Physics.OverlapSphere(gameObject.transform.position, range, Physics.AllLayers, QueryTriggerInteraction.UseGlobal);

            var list = new List<T>();

            foreach (var collider in overlapSphere)
            {
                T componentType;
                if (collider.gameObject.TryGetComponent<T>(out componentType))
                {
                    list.Add(componentType);
                }
            }

            return list;
        }

        public DamageInfo? DealDamageTo(DamageUnit damageUnit, IDamageable target)
        {
            var damageInfo = target.TakeDamage(damageUnit);
            OnDealDamage?.Invoke(damageInfo);
            return damageInfo;
        }
    }
}