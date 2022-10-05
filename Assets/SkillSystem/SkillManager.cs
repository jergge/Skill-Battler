using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

namespace SkillSystem {
[RequireComponent(typeof(ManaStats))]
public class SkillManager : MonoBehaviour, IOnCastEvents
{
    public GameObject skillHolder;
    public Transform skillSpawnLocation;
    public TargetInfo targetInfo = new TargetInfo();
    public ManaStats mainEnergyStats;

    //public List<Skill> InitialSkills;
    protected List<Skill> spellBook = new List<Skill>();

    public Skill attack;
    public Skill block;
    public Skill skill1;
    public Skill skill2;

    //bool currentlyCasting = false;
    List<Skill> currentlyCasting = new List<Skill>();

    public event Action<CastEventInfo, CheckForAny> CanICast;
    public event Action<CastEventInfo> OnBeforeCast;
    public event Action<CastEventInfo> OnAfterCast;

    // Start is called before the first frame update
    void Start()
    {
        if (skillHolder == null)
        {
            skillHolder = new GameObject("DEFAULT skill Holder for " + name);
            skillHolder.transform.SetParent(transform);
            skillHolder.transform.localPosition = Vector3.zero;
        }

        if (skillSpawnLocation == null)
        {
            skillSpawnLocation = new GameObject("DEFAULT skill spawn for " + gameObject.name).transform;
            skillSpawnLocation.transform.SetParent(transform);
            skillSpawnLocation.transform.localPosition = Vector3.zero + transform.forward;
        }

        foreach (Transform t in skillHolder.transform)
        {
            Destroy(t.gameObject);
        }

        InitialSkill(ref attack);
        InitialSkill(ref block);
        InitialSkill(ref skill1);
        InitialSkill(ref skill2);

    }

    void InitialSkill (ref Skill s )
    {
        if (s != null)
            s = s.Aquire(this);
    }
   
    void Update()
    {
        spellBook = skillHolder.GetComponentsInChildren<Skill>().ToList<Skill>();
        IHaveTargetInfo info;
        if (TryGetComponent<IHaveTargetInfo>(out info))
        {
            targetInfo = info.GetTargetInfo();
        }
    }

    public List<Skill> activeList = new List<Skill>();
    public List<Skill> GetActiveSkills() {
        activeList.Clear();
            foreach (Transform t in skillHolder.transform)
            {
                IActiveSkill active;

                if( t.gameObject.TryGetComponent<IActiveSkill>(out active))
                {
                    activeList.Add(t.GetComponent<Skill>());
                    // Debug.Log("getting something");

                }
            }
        return activeList; 
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
        // UseSkill(skill, triggerDown);

        if (skill is IActiveSkill)
        {
            IActiveSkill s = skill as IActiveSkill;
            //Debug.Log("Casting without the extras");
            s.Cast(skillSpawnLocation, targetInfo);
        }
    }

    protected void UseSkill(Skill skill, bool triggerDown)
    {
        if (skill is IChanneledSkill && !triggerDown)
        {
            IChanneledSkill s = skill as IChanneledSkill;
            s.StopCast();
            //currentlyCasting.Remove(skill);
            return;
        }

        if (skill == null || currentlyCasting.Count() > 0 || skill.cost > mainEnergyStats.GetCurrent())
        {
            return;
        }
        

        if (skill is IActiveSkill && triggerDown)
        {
            //currentlyCasting = true;
            
            IActiveSkill activeSkill = skill as IActiveSkill;

            CastEventInfo castInfo = new CastEventInfo(gameObject, skill, targetInfo.target);

            CheckForAny checker = new CheckForAny(false);
            if ( CanICast != null)
            {
                CanICast(castInfo, checker);
            }

            if (checker.Found() || skill.OnCooldown())
            {
                return;
            }

            activeSkill.Cast(skillSpawnLocation, targetInfo);

            if ( skill is IChanneledSkill)
            {
                IChanneledSkill c = skill as IChanneledSkill;
                c.CastEnded += SkillEnded;
                currentlyCasting.Add(skill);
            }

            if ( OnAfterCast != null)
            {
                //Debug.Log("invoking after cast delegates");
                OnAfterCast(castInfo);
            }

            mainEnergyStats.Reduce(skill.cost);
        }
    }

    void SkillEnded(Skill skill)
    {
        currentlyCasting.Remove(skill);
        IChanneledSkill c = skill as IChanneledSkill;
        c.CastEnded -= SkillEnded;
    }
}}
