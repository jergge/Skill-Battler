using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class SkillCube : MonoBehaviour, IAutoInteract<SkillManager>
{
    public Skill skillToAquire;

    public float range = 2;

    float IAutoInteract<SkillManager>.range { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Interact(SkillManager caller)
    {
        caller.Aquire(skillToAquire);
    }
}
