using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LootDropManager))]
public class Flora : MonoBehaviour, IOnDeathEvents
{
    public event Action OnDeath;

    protected void TriggerOnDeath()
    {
        OnDeath?.Invoke();
    }
}
