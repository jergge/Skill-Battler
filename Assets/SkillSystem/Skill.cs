using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SkillSystem.Properties;
using System.Linq;
using UnityEngine.InputSystem;


namespace SkillSystem {    
public abstract class Skill : MonoBehaviour
{
    protected enum SpellState
    {
        Prefab,
        SpellBook,
        InWorld
    };

    //public List<Type> allowedTargets = new List<Type>();

    protected SpellState spellState = SpellState.Prefab;
    protected GameObject source;
    public GameObject GetSource() => source;
    public string spellName;
    public string basicDescription;
    public Sprite icon;
    public int cost;

    protected Player? player => source.GetComponent<Player>();

    private bool cooldownPaused = false;
    protected void PauseCooldown() {
        cooldownPaused = true;
    }
    protected void ResumeCooldown() {
        cooldownPaused = false;
    }

    public enum ValidTargets
    {
        All,
        Self,
        Others,
        Allies,
        Enemies
    };

    public ValidTargets validTargets;
    public float cooldown = 2;
    //float cooldownRate = 1;

    protected float cooldownRate =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.cooldownRate, 1);
    public float remainingCooldown;

    public bool OnCooldown() {
        return (remainingCooldown >=0) ? true : false;
    }

    protected void ResetCooldown() {
        remainingCooldown = cooldown;
    }

    protected int layerMaskFromTargets => 255;

    //static float pollingUpdateInterval = .2f;

    protected bool IsValidTarget(LivingEntity source, GameObject target)
    {
        return IsValidTarget(source.gameObject, target);
    }

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

    public void SetSource(LivingEntity s)
    {
        SetSource(s.gameObject);
    }
    public void SetSource(GameObject go)
    {
        source = go;
    }

    public Skill Aquire(SkillManager manager)
    {
        Skill temp = GameObject.Instantiate(this);

        //Make it nice in the Unity hierachy
        temp.transform.SetParent(manager.skillHolder.transform);
        //Locate them in the same in game space..
        temp.transform.position = manager.transform.position;
        //set the object to have the right update and cast methods
        temp.spellState = SpellState.SpellBook;
        //disable all the visuals and colliders
        temp.DisableColliders();
        temp.SetSource(manager.gameObject);
        temp.OnStartInSpellbook();
        return temp;
    }

    void Start()
    {
        if (spellState == SpellState.InWorld)
        {
            EnableColliders();
            OnStartInWorld();
        }
        else if (spellState == SpellState.SpellBook)
        {
            //OnStartInSpellbook();
        }
    }

    public virtual void OnStartInSpellbook()
    {
        //Debug.Log("This Skill [" + spellName + "] has not implemented the optional OnStart method");
    }

    public virtual void OnStartInWorld()
    {
        // Debug.Log(
        //     "This Skill [" + spellName + "] has not implemented the optional OnStartInWorld method"
        // );
    }

    public void Update()
    {
        if (spellState == SpellState.SpellBook)
        {
            UpdateInSpellBook();
        }
        else
        {
            UpdateInWorld();
        }
    }

    protected void DisableColliders()
    {
        foreach (MeshRenderer mr in gameObject.GetComponents<MeshRenderer>())
        {
            mr.enabled = false;
        }
        foreach (Collider col in gameObject.GetComponents<Collider>())
        {
            col.enabled = false;
        }
    }

    protected void EnableColliders()
    {
        foreach (MeshRenderer mr in gameObject.GetComponents<MeshRenderer>())
        {
            mr.enabled = true;
        }
        foreach (Collider col in gameObject.GetComponents<Collider>())
        {
            col.enabled = true;
        }
    }

    public virtual void UpdateInSpellBook() {
        if ( !cooldownPaused )
        {
            remainingCooldown -= (Time.deltaTime * cooldownRate);
        }
     }

    public virtual void UpdateInWorld() { }

    //public abstract void Cast(Transform spawnLoaction, TargetInfo targetInfo, InputValue inputValue);

    protected float GetModifiedValue(ModifiableSkillProperty.ModifyValue propertyIdentifier, float baseAmount)
    {
        return ModifiableSkillProperty.GetModifiedValue(propertyIdentifier, baseAmount, source);
    }
    protected int GetModifiedValueInt(ModifiableSkillProperty.ModifyValue propertyIdentifier, float baseAmount)
    {
        return ModifiableSkillProperty.GetModifiedValueInt(propertyIdentifier, baseAmount, source);
    }

    protected List<T> GetInDistance<T>(float range) where T : MonoBehaviour
    {
        var physicsList = Physics.OverlapSphere(gameObject.transform.position, range, Physics.AllLayers, QueryTriggerInteraction.UseGlobal);
        
        var list = new List<T>();

        foreach( var item in physicsList )
        {
            T tempT;
            if (item.gameObject.TryGetComponent<T>(out tempT))
            {
                list.Add(tempT);
            }
        }

        return list;
    }
}}