using System;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;

public class WaterShield : Skill, IChanneledSkill
{
    bool casting;
    public WaterShieldPrefab shieldPrefab;
    public float energyDrainPerSecond = 10;
    StatsTracker energy;

    public event Action<Skill> CastEnded;

    void Update()
    {
        TickCooldown();
    }

    public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        if (!CoolingDown())
        {
            StartCoroutine(Channel());

        }
    }

    public void StopCast()
    {
        if (casting)
        {
            casting = false;
        }
    }

    IEnumerator Channel()
    {
        casting = true;

        WaterShieldPrefab shield = GameObject.Instantiate<WaterShieldPrefab>(shieldPrefab);
        shield.transform.position = source.transform.position;
        shield.source = source;

        Player player;
        if (source.TryGetComponent<Player>(out player))
        {
            //ffplayer.playerCameraController.SetLocation(player.playerCameraController.presets[1]);
        }

        while (casting && energy.current > energyDrainPerSecond * Time.deltaTime)
        {
            //s.transform.position = source.transform.position;
            energy -= (energyDrainPerSecond * Time.deltaTime);
            yield return null;
        }

        if (CastEnded != null)
        {
            CastEnded(this);
        }
        Destroy(shield.gameObject);

        if (source.TryGetComponent<Player>(out player))
        {
            //player.playerCameraController.SetLocation(player.playerCameraController.presets[0]);
        }
        ResetCooldown();
    }

    public override void OnMadeActive()
    {
        energy = source.AddComponent<StatsTracker>();
        energy.baseValue = 50f;
        energy.regenPerSecond = 2;
    }

    public override void OnMadeInActive()
    {
        energy = null;
    }

}
