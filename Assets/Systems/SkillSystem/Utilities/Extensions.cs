using System;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;

public static partial class Extensions
{
    [Obsolete("Use the overloaded AddComponent<T> instead")]
    public static T AddStackableComponent<T>(this GameObject gameObject) where T: MonoBehaviour, UniqueComponent<T>
    {
        T test;
        if (gameObject.TryGetComponent<T>(out test))
        {
            test.AddNew();
            return test;
        } else{
            return gameObject.AddComponent<T>();
        }
    }

    [Obsolete("This is currently not working, do this manually at the moment")]
    public static T AddComponent<T>(this GameObject gameObject) where T: UniqueComponent<T>
    {
        T test;
        if (gameObject.TryGetComponent<T>(out test))
        {
            test.AddNew();
            return test;
        } else{
            return gameObject.AddComponent<T>();
        }
    }

    [Obsolete("Call Configure(Skill skill) after adding component instead of using this")]
    public static T AddComponent<T>(this GameObject gameObject, Skill skill) where T: Buff
    {
            T buff = gameObject.AddComponent<T>();

            buff.Configure(skill);

            return buff;
    }

    /// <summary>
    /// Returns the Component if it is found on the GameObject, otherwise creates a new Component
    /// </summary>
    /// <typeparam name="C">The Component to search for / create</typeparam>
    /// <param name="gameObject">the GameObject</param>
    /// <returns>The component found or created</returns>
    public static C AddOrGetComponent<C>(this GameObject gameObject) where C : MonoBehaviour
    {
        C test;
        if (gameObject.TryGetComponent<C>(out test))
        {
            return test;
        } else{
            return gameObject.AddComponent<C>();
        }
    }

    /// <summary>
    /// Returns the Component if it is found on the GameObject, otherwise creates a new Component
    /// </summary>
    /// <typeparam name="C">The Component to search for / create</typeparam>
    /// <param name="gameObject">the GameObject</param>
    /// <param name="foundOnObject">Returns True if the component already existed</param>
    /// <returns>The component found or created</returns>
    public static C AddOrGetComponent<C>(this GameObject gameObject, out bool foundOnObject) where C : MonoBehaviour
    {
        C test;
        if (gameObject.TryGetComponent<C>(out test))
        {
            foundOnObject = true;
            return test;
        } else{
            foundOnObject = false;
            return gameObject.AddComponent<C>();
        }
    }

    /// <summary>
    /// Returns the Component if it is found on the GameObject and the filter function returns True, otherwise creates a new Component
    /// </summary>
    /// <typeparam name="C">The Component to search for / create</typeparam>
    /// <param name="gameObject">the GameObject</param>
    /// <param name="filter">A function to search for a more specific existing Component of Type C</param>
    /// <returns>The component found or created</returns>
    public static C AddOrGetComponent<C>(this GameObject gameObject, Func<C, bool> filter) where C : MonoBehaviour
    {
        var components = gameObject.GetComponents<C>();
        foreach (var component in components)
        {
            if ( filter(component) )
            {
                return component;
            }
        }

        return gameObject.AddComponent<C>();
    }

    public static bool TryGetComponentInParent<C>(this GameObject gameObject, out C component) where C : MonoBehaviour
    {
        component = gameObject.GetComponentInParent<C>();

        if ( component is not null )
        {
            return true;
        }

        return false;
    }

}
