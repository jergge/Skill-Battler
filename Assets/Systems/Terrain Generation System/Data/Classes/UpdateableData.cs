using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpdateableData : ScriptableObject 
{
    public event Action OnVaulesUpdated;
    public bool autoUpdate;

    void OnValidate()
    {
        if (autoUpdate)
        {
            NotifyOfUpdatedValues();
        }
    }

    public void NotifyOfUpdatedValues()
    {
        if (OnVaulesUpdated != null)
        {
            OnVaulesUpdated();
        }
    }
}
