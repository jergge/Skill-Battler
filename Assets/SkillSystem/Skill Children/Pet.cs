using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace SkillSystem.Pet {
public class Pet : Skill, IActiveSkill, IUpdateDPad
{
    public PetPrefab petToSummon;
    PetPrefab summonedPet;

    public void Cast(Transform spawnLocation, TargetInfo targetInfo)
    {
        if(OnCooldown())
        {
            return;
        }
        summonedPet = GameObject.Instantiate<PetPrefab>(petToSummon, spawnLocation.position, Quaternion.identity);
        summonedPet.master = source;

        summonedPet.OnDeath += OnPetDeath;
        ResetCooldown();
        PauseCooldown();
    }

    public DPadMap GetDPadMap()
    {
//        return summonedPet.GetDPadMap();
        return new DPadMap();
    }

        void OnPetDeath()
    {
        summonedPet.OnDeath -= OnPetDeath;
        ResumeCooldown();
    }
}}
