using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelVertex<T> : Vertex<T>
{
    new public List<LevelVertex<T>> connections = new List<LevelVertex<T>>();

    public event Action OnMaxed;

    public int levelInTree;
    public int currentPoints;
    public int maxPoints;
    public bool selectable = false;

    public Vector2 positionInEditor;

    public bool maxed => (currentPoints == maxPoints);

    public bool LevelUp()
    {
        if (maxed)
        {
            return false;
        }
        currentPoints++;

        if (maxed)
        {
            OnMaxed?.Invoke();
        }
        return true;
    }


}
