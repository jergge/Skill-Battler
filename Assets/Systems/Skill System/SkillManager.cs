using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem
{
    /// <summary>
    /// Manages the Skills for a GameObject
    /// </summary>
    [RequireComponent(typeof(StatsTracker))]
    public class SkillManager : MonoBehaviour, IOnCastEvents, ISendToastNotifications
    {
        public GameObject enabledSkills;
        List<Skill> enabledSkillsList = new List<Skill>();
        public event Action<Skill> OnSkillEnabled;

        public GameObject disabledSkills;
        List<Skill> disabledSkillsList = new List<Skill>();
        public event Action<Skill> OnSkillDisabled;

        public List<Skill> allSkills
        {
            get
            {
                List<Skill> allSkills = new List<Skill>();
                allSkills.AddRange(enabledSkillsList);
                allSkills.AddRange(disabledSkillsList);

                return allSkills;
            }
        }

        public Transform skillSpawnLocation;
        public TargetInfo targetInfo = new TargetInfo();
        public StatsTracker mainEnergyStats;

        public event Action<DPadMap> NewDPadMap;

        //public List<Skill> InitialSkills;
        //protected List<Skill> spellBook = new List<Skill>();
        //bool currentlyCasting = false;
        List<Skill> currentlyCasting = new List<Skill>();

        public Skill attack;
        public Skill block;
        public Skill skill1;
        public Skill skill2;

        public event Action<CastEventInfo, CheckForAny> CanICast;
        public event Action<CastEventInfo> OnBeforeCast;
        public event Action<CastEventInfo> OnAfterCast;

        public event Action<ToastNotificationInfo> PushToast;

        void Awake()
        {
            if (enabledSkills == null)
            {
                enabledSkills = new GameObject("DEFAULT collection of enabled skills");
                enabledSkills.transform.SetParent(transform);
                enabledSkills.transform.localPosition = Vector3.zero;
            }

            if (skillSpawnLocation == null)
            {
                skillSpawnLocation = new GameObject("DEFAULT skill spawn location").transform;
                skillSpawnLocation.transform.SetParent(transform);
                skillSpawnLocation.transform.localPosition = Vector3.zero + transform.forward;
            }

            if (disabledSkills == null)
            {
                disabledSkills = new GameObject("DEFAULT collection of disabled skills");
                disabledSkills.transform.SetParent(transform);
                disabledSkills.transform.localPosition = Vector3.zero;
            }

            foreach (Transform t in enabledSkills.transform)
            {
                Destroy(t.gameObject);
            }

            Invoke("AquireInitialSkills", .5f);

        }

        void AquireInitialSkills()
        {
            AquireInitialSkill(ref attack);
            AquireInitialSkill(ref block);
            AquireInitialSkill(ref skill1);
            AquireInitialSkill(ref skill2);            
        }

        /// <summary>
        /// Adds a new skill to the skill manager
        /// </summary>
        /// <param name="skillToAquire">The skill to be aquired</param>
        /// <param name="addToActiveBook">Should the skill be made active when aquired?</param>
        /// <returns></returns>
        public Skill Aquire(Skill skillToAquire, bool addToActiveBook = false)
        {
            //fist check to make sure we don't already know the skill
            if (enabledSkillsList.Contains(skillToAquire) || disabledSkillsList.Contains(skillToAquire))
            {
                return null;
            }

            Skill newSkill = GameObject.Instantiate(skillToAquire);

            newSkill.source = gameObject;
            newSkill.transform.position = transform.position;

            PushToast?.Invoke(new ToastNotificationInfo(newSkill.icon, "You have learned a new skill: " + newSkill.skillName));

            if (addToActiveBook || skill1 is null || skill2 is null)
            {
                EnableSkill(newSkill);

                if (skill1 is null)
                {
                    skill1 = newSkill;
                } else if (skill2 is null)
                {
                    skill2 = newSkill;
                }
            }
            else
            {
                DisableSkill(newSkill);
            }

            return newSkill;
        }

        void EnableSkill(Skill skill)
        {
            //skill.gameObject.SetActive(true);
            //skill.MakeActive();
            disabledSkillsList.Remove(skill);
            enabledSkillsList.Add(skill);
            skill.transform.SetParent(enabledSkills.transform);
            skill.Enabled();
            OnSkillEnabled?.Invoke(skill);
        }

        void DisableSkill(Skill skill)
        {
            //skill.gameObject.SetActive(false);
            //skill.MakeInactive();
            enabledSkillsList.Remove(skill);
            disabledSkillsList.Add(skill);
            skill.transform.SetParent(disabledSkills.transform);
            skill.Disabled();
            OnSkillDisabled?.Invoke(skill);
        }

        public bool TryGetEnabledSkill<SkillType>(out SkillType skill) where SkillType : Skill
        {
            Debug.Log("TryGetEnabledSkill called with " + enabledSkillsList.Count + " active skills in the list");
            foreach ( Skill enabledSkill in enabledSkillsList)
            {
                //Debug.Log("Checking if skill: [" + s.name + "] is of type [" + typeof(SkillType).ToString());
                if ( enabledSkill is SkillType )
                {
                    skill = (SkillType)enabledSkill;
                    return true;
                }
            }

            skill = null;
            return false;
        }

        void AquireInitialSkill(ref Skill skill)
        {
            if (skill != null)
            {
                skill = Aquire(skill, true);
            }
        }

        void Update()
        {
            //spellBook = enabledSkills.GetComponentsInChildren<Skill>().ToList<Skill>();
            IHaveTargetInfo info;
            if (TryGetComponent<IHaveTargetInfo>(out info))
            {
                targetInfo = info.GetTargetInfo();
            }
        }


        void OnAttack(InputValue inputValue)
        {
            UseSkill(attack, inputValue.isPressed);
        }

        void OnBlock(InputValue inputValue)
        {
            UseSkill(block, inputValue.isPressed);
        }

        void OnSkill1(InputValue inputValue)
        {
            UseSkill(skill1, inputValue.isPressed);
        }

        void OnSkill2(InputValue inputValue)
        {
            UseSkill(skill2, inputValue.isPressed);
        }

        public void NPCUseSkill(Skill skill, TargetInfo targetInfo, bool triggerDown)
        {
            this.targetInfo = targetInfo;

            if (skill is IActiveSkill activeSkill)
            {
                activeSkill.Cast(skillSpawnLocation, targetInfo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skill">The Skill that is being used / cast</param>
        /// <param name="triggerDown">True if fired on key down, fasle if fired on key up</param>
        protected void UseSkill(Skill skill, bool triggerDown)
        {
            if (skill is null)
            {
                return;
            }

            //Debug.Log("using skill: " + skill.name);


            if (skill is IUpdateDPad dPadSkill && triggerDown)
            {
                Debug.Log("Skill is IUpdateDPad");
                if (NewDPadMap != null)
                {
                    NewDPadMap(dPadSkill.GetDPadMap());
                }

            }

            //If you're releasing the key for a channeled skill
            if (skill is IChanneledSkill channeledSkill && !triggerDown)
            {
                // Debug.Log("Skill is IChanneledSkill");
                channeledSkill.EndChannel();
                //currentlyCasting.Remove(skill);
                return;
            }

            //If you are already casting something, or you don't have the energy to pay for it, or it's null
            if (skill == null || currentlyCasting.Count() > 0 || skill.baseCost > mainEnergyStats.current)
            {
                Debug.Log("Skill is null or something already casting or costs too much");
                return;
            }

            //If you are activating an active skill
            if (skill is IActiveSkill activeSkill && triggerDown)
            {
                // Debug.Log("skill is IActiveSkill");
                //currentlyCasting = true;

                if (skill.remainingCooldown > 0)
                {
                    return;
                }

                CastEventInfo castInfo = new CastEventInfo(gameObject, skill, targetInfo.target);

                CheckForAny checker = new CheckForAny(false);

                CanICast?.Invoke(castInfo, checker);

                if (checker.Found() || skill.CoolingDown())
                {
                    Debug.Log("Checker says skill cannot be cast: Found(): " + checker.Found() + " ||  OnCooldown(): " + skill.CoolingDown());
                    return;
                }

                // Debug.Log("Casting the active skill: " + skill.name);
                activeSkill.Cast(skillSpawnLocation, targetInfo);

                if (skill is IChanneledSkill channeledSkillx)
                {
                    channeledSkillx.CastEnded += SkillEnded;
                    currentlyCasting.Add(skill);
                }

                OnAfterCast?.Invoke(castInfo);

                //mainEnergyStats.Reduce(skill.cost);
                mainEnergyStats -= skill.baseCost;
            }
        }

        void SkillEnded(Skill skill)
        {
            currentlyCasting.Remove(skill);
            IChanneledSkill c = skill as IChanneledSkill;
            c.CastEnded -= SkillEnded;
        }

    }

    
}
