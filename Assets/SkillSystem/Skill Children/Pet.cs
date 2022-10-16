using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace SkillSystem.Pet {
public class Pet : Skill, IActiveSkill
{
    public PetPrefab petToSummon;

    PetPrefab summonedPet;

    public void Cast(Transform spawnLocation, TargetInfo targetInfo)
    {
        summonedPet = GameObject.Instantiate<PetPrefab>(petToSummon, spawnLocation.position, Quaternion.identity);
    }
}}
