using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using SkillSystem.Utilities;

public class MoonBurn : Skill
{
    LivingEntity target;
    public float duration = 5;
    float timeAttached;
    public float damagePerSecond = 5;
    [SerializeField] float timeRemaining;
    Stepper damageStepper;


    public override void OnStartInWorld()
    {
        target = GetComponent<LivingEntity>(); 
        timeAttached = Time.time;
        timeRemaining = duration;

        if (!IsValidTarget(source, target.gameObject))
        {
            Destroy(this);
        } else
        {
            damageStepper = new Stepper(Mathf.RoundToInt(duration), target, this, true);
            StartCoroutine(damageStepper.GetCoroutine());
        }
    }
    
    public override void UpdateInWorld()
    {
        damageStepper.SummingDamage(damagePerSecond*Time.deltaTime);
        Debug.DrawLine(gameObject.transform.position, GameObject.FindObjectOfType<Moon>().transform.position, Color.magenta);

    }

    public override void UpdateInSpellBook()
    {
        //throw new System.NotImplementedException();
    }

    public override void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        var tmp = FindObjectsOfType<LivingEntity>();

        foreach (LivingEntity p in tmp)
        {
            var temp = p.gameObject.AddComponent<MoonBurn>();
            temp.spellState = SpellState.InWorld;
            temp.validTargets = validTargets;
            temp.SetSource(source);
        }

    }

}
