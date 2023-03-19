using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// Returns a random element from the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T Random<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Returns a random element from a list, after performing a filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static T Random<T>(this List<T> list, Func<T, bool> filter)
    {
        var filteredList = list.Where((item) => filter(item));
        return filteredList.ToList().Random();
    }

    public static bool Contains(this LayerMask layerMask, GameObject gameObject)
    {
        return (layerMask == (layerMask | (1<<gameObject.layer)));
    }

    public static List<T> GetInRange<T>(this GameObject gameObject, float range) where T:MonoBehaviour
    {
        List<T> things = new List<T>();
        var allT = GameObject.FindObjectsOfType<T>();

        float squareRange = range * range;

        foreach (var obj in allT)
        {
            if (Vector3.Distance(obj.transform.position, gameObject.transform.position) <= range)
            {
                things.Add(obj);
            }
        }
        return things;
    }

    public static float Remap(float input, float inputMin, float inputMax, float outputMin, float outputMax)
    {
            float noramlised = Mathf.InverseLerp(inputMin, inputMax, input);

            float output = Mathf.Lerp(outputMin, outputMax, noramlised);

            return output;
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


