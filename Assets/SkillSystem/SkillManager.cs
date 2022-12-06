using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

namespace SkillSystem {
[RequireComponent(typeof(StatsTracker))]
public class SkillManager : MonoBehaviour, IOnCastEvents
{
    public GameObject skillHolder;
    public Transform skillSpawnLocation;
    public TargetInfo targetInfo = new TargetInfo();
    public StatsTracker mainEnergyStats;

    public event Action<DPadMap> NewDPadMap;

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

    public Skill Aquire(Skill skillToAquire, bool addToActiveBook = false)
    {
        Skill newSkill = GameObject.Instantiate(skillToAquire);

        //Make it nice in the Unity hierachy
        newSkill.transform.SetParent(skillHolder.transform);
        //Locate them in the same in game space..
        newSkill.transform.position = transform.position;
        //set the object to have the right update and cast methods
        newSkill.SetSpellState(Skill.SpellState.SpellBook);
        //disable all the visuals and colliders
        newSkill.DisableColliders();
        newSkill.SetSource(gameObject);
        newSkill.OnStartInSpellbook();
        return newSkill;
    }

    void InitialSkill (ref Skill skill)
    {
        if (skill != null)
        {
            skill = Aquire(skill);
        }
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

        if (skill is IActiveSkill activeSkill)
        {
            activeSkill.Cast(skillSpawnLocation, targetInfo);
        }
    }

    protected void UseSkill(Skill skill, bool triggerDown)
    {
        if (skill is null)
        {
            return;
        }

        Debug.Log("using skill: " + skill.name);
        if (skill is IUpdateDPad dPadSkill && triggerDown)
        {
            Debug.Log("Skill is IUpdateDPad");
            if (NewDPadMap != null)
            {
                NewDPadMap(dPadSkill.GetDPadMap());
            }

        }

        if (skill is IChanneledSkill channeledSkill && !triggerDown)
        {
            Debug.Log("Skill is IChanneledSkill");
            channeledSkill.StopCast();
            //currentlyCasting.Remove(skill);
            return;
        }

        if (skill == null || currentlyCasting.Count() > 0 || skill.cost > mainEnergyStats.current)
        {
            Debug.Log("Skill is null or something already casting or costs too much");
            return;
        }
        

        if (skill is IActiveSkill activeSkill && triggerDown)
        {
            Debug.Log("skill is IActiveSkill");
            //currentlyCasting = true;
            
            CastEventInfo castInfo = new CastEventInfo(gameObject, skill, targetInfo.target);

            CheckForAny checker = new CheckForAny(false);

            CanICast?.Invoke(castInfo, checker);

            if (checker.Found() || skill.CoolingDown())
            {
                Debug.Log("Checker says skill cannot be cast: Found():" + checker.Found() + "   OnCooldown():" + skill.CoolingDown());
                return;
            }

            Debug.Log("Casting the active skill: " + skill.name);
            activeSkill.Cast(skillSpawnLocation, targetInfo);

            if ( skill is IChanneledSkill cSkill )
            {
                cSkill.CastEnded += SkillEnded;
                currentlyCasting.Add(skill);
            }

            OnAfterCast?.Invoke(castInfo);

            //mainEnergyStats.Reduce(skill.cost);
            mainEnergyStats -= skill.cost;
            }
    }

    void SkillEnded(Skill skill)
    {
        currentlyCasting.Remove(skill);
        IChanneledSkill c = skill as IChanneledSkill;
        c.CastEnded -= SkillEnded;
    }
}}
