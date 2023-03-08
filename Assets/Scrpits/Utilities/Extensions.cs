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

    public static T Random<T>(this List<T> list)
    {
        int count = list.Count;
        int randomIndex =  UnityEngine.Random.Range(0, count -1);
        return list[randomIndex];
    }
        
    public static bool Contains(this LayerMask layerMask, GameObject gameObject)
    {
        return (layerMask == (layerMask | (1<<gameObject.layer)));
    }

    public static List<T> GetInRange<T>(this MonoBehaviour monoBehaviour, float range) where T:MonoBehaviour
    {
        List<T> things = new List<T>();
        var allT = GameObject.FindObjectsOfType<T>();

        float squareRange = range * range;

        foreach (var obj in allT)
        {
            if (Vector3.Distance(obj.transform.position, monoBehaviour.transform.position) <= range)
            {
                things.Add(obj);
            }
        }
        return things;
    }

    // swizzles
    public static Vector3 ToV3(this Vector4 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public static Vector2 XY(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 XZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 YZ(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }
}}


