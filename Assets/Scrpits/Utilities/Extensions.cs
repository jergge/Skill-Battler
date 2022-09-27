using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jergge.Extensions {

public static class Extensions
{
    public static string Truncate(this string s, int length)
    {
        if (string.IsNullOrEmpty(s))
        {
            return s;
        }

        return s.Length <= length ? s : s.Substring(0, length);
    }

    public static T Random<T>(this List<T> l)
    {
        int count = l.Count;

        int randomIndex =  UnityEngine.Random.Range(0, count);

        return l[randomIndex];
    }
        
    public static bool Contains(this LayerMask layerMask, GameObject gameObject)
    {
        return (layerMask == (layerMask | (1<<gameObject.layer)));
    }

    public static Vector3 ToV3(this Vector4 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
}}


