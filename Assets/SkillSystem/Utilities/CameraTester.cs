using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using UnityEngine.InputSystem;
using System;

public class CameraTester : Skill, IChanneledSkill
{
    bool casting;

    public event Action<Skill> CastEnded;

    public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        Player p;

        if (source.TryGetComponent<Player>(out p))
        {
            StartCoroutine(tempLocation(p));
        }
    }

    public void StopCast()
    {
        casting = false;
    }

    IEnumerator tempLocation(Player p)
    {
        p.playerCameraController.SetLocation(p.playerCameraController.presets[1]);
        p.playerController.LockTilt();
        Debug.Log("starting thing");

        casting = true;
        
        while(casting)
        {
            yield return null;
        }

        p.playerController.UnlockTilt();
        p.playerCameraController.SetLocation(p.playerCameraController.presets[1]);
        Debug.Log("ending thing");

    }
}
