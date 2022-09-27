using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStackableComponent
{
    public int stackCount {get; set;}

    public void AddStacks(int count) {
        stackCount += count;
    }
}
