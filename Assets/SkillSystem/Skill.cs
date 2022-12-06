using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SkillSystem.Properties;
using System.Linq;
using UnityEngine.InputSystem;


namespace SkillSystem { 
    /// <summary>
    /// The base class for all Skills
    /// </summary>   
public abstract class Skill : MonoBehaviour
{
    public enum SpellState
    {
        Prefab,
        SpellBook,
        InWorld
    };
    protected SpellState spellState = SpellState.Prefab;
    /// <summary>
    /// Who cast the Skill, or who controlles its activation. Where does this Skill come from?
    /// </summary>
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
    public int cost;

    protected Player? player => source.GetComponent<Player>();

    /// <summary>
    /// Controlls whether or not the Skill will reduce its cooldown each frame
    /// </summary>
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
    /// <summary>
    /// The base interval between successive uses of the Skill (modified by the cooldownRate)
    /// </summary>
    public float cooldown = 2;
    //float cooldownRate = 1;

    protected float cooldownRate =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.cooldownRate, 1);
    public float remainingCooldown;

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

    protected int layerMaskFromTargets => 255;

    [Obsolete("use IsValidTarget(GameObject, GameObject) instead")]
    protected bool IsValidTarget(LivingEntity source, GameObject target)
    {
        return IsValidTarget(source.gameObject, target);
    }

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

    [Obsolete("Use Skill.SetSource(GameObject go) instead")]
    public void SetSource(LivingEntity s)
    {
        SetSource(s.gameObject);
    }

    /// <summary>
    /// Set's the Skill's source to a GameObject
    /// </summary>
    /// <param name="gameObject"></param>
    public void SetSource(GameObject gameObject)
    {
        source = gameObject;
    }

    [Obsolete("Please use method SkillManager.Aquire(Skill) instead")]
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spellState"></param>
    public void SetSpellState(SpellState spellState)
    {
        this.spellState = spellState;
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

    /// <summary>
    /// Function that is called when the Skill is added to a SpellBook. Likely Obsolete in future
    /// </summary>
    public virtual void OnStartInSpellbook()
    {
        //Debug.Log("This Skill [" + spellName + "] has not implemented the optional OnStart method");
    }

    [Obsolete("This shouldn't? be used anymore, think of better ways to have these interactions that use prefabs and don't require the 'Skill' scripts to become bloated")]
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

    /// <summary>
    /// Allows the skill to exist in the world but not act as something real
    /// </summary>
    [Obsolete("Should now create Skills that can be set as 'Inactive Gameobjects' instead")]
    public void DisableColliders()
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

    [Obsolete("Should now create Skills that can be set as 'Inactive Gameobjects' instead")]
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

    /// <summary>
    /// Function that runs each frame for all Skills that are currently being used
    /// </summary>
    public virtual void UpdateInSpellBook() 
    {
        if ( !cooldownPaused )
        {
            //remainingCooldown -= (Time.deltaTime * cooldownRate);
            remainingCooldown = Mathf.Max(remainingCooldown - (Time.deltaTime * cooldownRate), 0);
        }
    }

    [Obsolete("Use Skills that spawn prefabs that can handle the actions that would have gone here")]
    public virtual void UpdateInWorld() {}

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

    [Obsolete("Just try and use floats for all this stuff I think")]
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