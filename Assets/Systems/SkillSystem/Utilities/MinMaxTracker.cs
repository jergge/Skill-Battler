using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxTracker
{
    public float max = float.MinValue;
    public float min = float.MaxValue;

    public void Clear()
    {
        max = float.MinValue;
        min = float.MaxValue;
    }

    public void Track(float input)
    {
        if ( input < min )
        {
            min = input;
        } else if (input > max )
        {
            max = input;
        }
    }

    public void Track(float[] input)
    {
        foreach ( float i in input)
        {
            Track(i);
        }
    }
}
