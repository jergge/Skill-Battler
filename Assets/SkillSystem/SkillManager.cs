using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SkillSystem {
public class SkillManager : MonoBehaviour, IOnCastEvents
{
    public GameObject skillHolder;
    public Transform skillSpawnLocation;
    public TargetInfo targetInfo = new TargetInfo();

    //public List<Skill> InitialSkills;
    protected List<Skill> spellBook = new List<Skill>();

    public Skill attack;
    public Skill block;
    public Skill skill1;
    public Skill skill2;

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
            skillSpawnLocation.transform.localPosition = Vector3.zero + transform.forward *2;
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
    // Update is called once per frame
    void Update()
    {
        spellBook = skillHolder.GetComponentsInChildren<Skill>().ToList<Skill>();
        IHaveTargetInfo info;
        if (TryGetComponent<IHaveTargetInfo>(out info))
        {
            targetInfo = info.GetTargetInfo();
        }
    }


    List<Skill> activeList = new List<Skill>();
    public List<Skill> GetActiveSkills() {
        activeList.Clear();
            foreach (Transform t in skillHolder.transform)
            {
                if (t.GetComponent<Skill>().hasActiveComponent) {
                    activeList.Add(t.GetComponent<Skill>());
                }
            }
        return activeList; 
    }

    void OnAttack()
    {
        UseSkill(attack);
    }

    void OnBlock()
    {
        UseSkill(block);
    }

    void OnSkill1()
    {
        UseSkill(skill1);
    }

    void OnSkill2()
    {
        UseSkill(skill2);
    }

    protected void UseSkill(Skill skill)
    {
        if(skill == null)
        {
            return;
        }
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

        skill.Cast(skillSpawnLocation, targetInfo);

        if ( OnAfterCast != null)
        {
            Debug.Log("invoking after cast delegates");
            OnAfterCast(castInfo);
        }
    }
}}
