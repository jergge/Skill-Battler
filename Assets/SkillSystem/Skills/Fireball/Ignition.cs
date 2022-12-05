using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class Ignition : MonoBehaviour, IStackableComponent
{

    public float timeRemaining;
    public float baseDuration = 10f;
    public int damageAtStacks = 200;

    public GameObject source;

    public int stackCount { get; set; }

    void Start()
    {
        timeRemaining = baseDuration;
        stackCount ++;
    }

    void Update()
    {
        if (stackCount >= 3)
        {
            AtMaxStacks();
        }

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            Destroy(this);
        }
    }

    void AtMaxStacks()
    {
        IDamageable target;
        if (gameObject.TryGetComponent<IDamageable>(out target))
        {
            target.TakeDamage(damageAtStacks);
        }

        Destroy(this);
    }

    public void AddStacks(int s)
    {
        stackCount += s;
        timeRemaining = baseDuration;
    }

}
