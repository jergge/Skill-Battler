using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForAny
{
    bool checkingFor;

    int trues = 0;
    int falses = 0;

    public CheckForAny(bool checkingFor)
    {
        this.checkingFor = checkingFor;
    }

    public void True()
    {
        trues ++;
    }
    
    public void False()
    {
        falses ++;
    }

    public bool Found()
    {
        if (checkingFor)
        {
            return trues > 0;
        } else {
            return falses > 0;
        }
    }

    public int FoundHowMany()
    {
        if (checkingFor)
        {
            return trues;
        } else {
            return falses;
        }
    }

    public bool NotFound()
    {
        if (checkingFor)
        {
            return trues == 0;
        } else {
            return falses == 0;
        }
    }
}