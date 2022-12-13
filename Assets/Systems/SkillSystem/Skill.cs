using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DamageSystem;
using SkillSystem.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem
{
    /// <summary>
    /// The base class for all Skills
    /// </summary>   
    public abstract class Skill : MonoBehaviour //IOnDealDamageEvents
    {
        protected GameObject source;
        public GameObject GetSource() => source;

        /// <summary>
        /// The Skill's name (not the same as the Unity Object name in the inspector). Used for windows in game
        /// </summary>
        public string skillName;

        /// <summary>
        /// Used for windoes in game to explain to the player what the Skill does
        /// </summary>
        public string basicDescription;

        /// <summary>
        /// A square icon that represents the Skill
        /// </summary>
        public Sprite icon;

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
        /// The base interval between successive uses of the Skill (modified by the cooldownRate)
        /// </summary>
        public float cooldown = 2;

        protected float cooldownRate =>
            GetModifiedValue(ModifiableSkillProperty.ModifyValue.cooldownRate, 1);
        public float remainingCooldown;

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
            Debug.Log(remainingCooldown);
            return (remainingCooldown > 0) ? true : false;
        }

        /// <summary>
        /// Sets the Skill's current cooldown to its base cooldown
        /// </summary>
        protected void ResetCooldown()
        {
            remainingCooldown = cooldown;
        }

        public virtual void OnMadeActive() { }
        public virtual void OnMadeInActive() { }

        //protected int layerMaskFromTargets => 255;

        /// <summary>
        /// Uses LayerMasks to determine if this Skill can interact with another object
        /// </summary>
        /// <param name="source">The source of the Skill</param>
        /// <param name="target">The object to interact with</param>
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

        protected List<T> GetInDistance<T>(float range) where T : MonoBehaviour
        {
            var physicsList = Physics.OverlapSphere(gameObject.transform.position, range, Physics.AllLayers, QueryTriggerInteraction.UseGlobal);

            var list = new List<T>();

            foreach (var item in physicsList)
            {
                T tempT;
                if (item.gameObject.TryGetComponent<T>(out tempT))
                {
                    list.Add(tempT);
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