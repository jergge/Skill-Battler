using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using System;

public class WaterShield : Skill, IChanneledSkill
{
    bool casting;
    public WaterShieldPrefab shieldPrefab;
    public float energyDrainPerSecond = 10;
    ManaStats energy;

    public event Action<Skill> CastEnded;

    public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        if (!OnCooldown())
        {
            StartCoroutine(Channel());

        }
    }

    public void StopCast()
    {
        if(casting)
        {
            casting = false;
        }
    }

    IEnumerator Channel()
    {
        casting = true;

        WaterShieldPrefab s = GameObject.Instantiate<WaterShieldPrefab>(shieldPrefab);
        s.transform.position = source.transform.position;
        s.source = source;

        if(player != null)
        {
            player.playerCameraController.SetLocation(player.playerCameraController.presets[1]);
        }

        while(casting && energy.GetCurrent() > energyDrainPerSecond*Time.deltaTime)
        {
            //s.transform.position = source.transform.position;
            energy.Reduce(energyDrainPerSecond * Time.deltaTime);
            yield return null;
        }

        if (CastEnded != null)
        {
            CastEnded(this);
        }
        Destroy(s.gameObject);

        if(player != null)
        {
            player.playerCameraController.SetLocation(player.playerCameraController.presets[0]);
        }
        ResetCooldown();
    }

    public override void OnStartInSpellbook()
    {
        energy = source.AddComponent<ManaStats>();
        energy.baseValue = 50f;
        energy.regenPerSecond = 2;
    }

}
