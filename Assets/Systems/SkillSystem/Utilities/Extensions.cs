using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem.Extensions {
public static class Extensions
{
    
    public static T AddStackableComponent<T>(this GameObject gameObject, int count = 1) where T: MonoBehaviour, IStackableComponent
    {
        T test;
        if (gameObject.TryGetComponent<T>(out test))
        {
            test.AddStack(count);
            return test;
        } else{
            return gameObject.AddComponent<T>();
        }
    }

    public static T AddComponent<T>(this GameObject gameObject, int count) where T: MonoBehaviour, IStackableComponent
    {
        T test;
        if (gameObject.TryGetComponent<T>(out test))
        {
            test.AddStack(count);
            return test;
        } else{
            return gameObject.AddComponent<T>();
        }
    }

    public static T AddComponent<T>(this GameObject gameObject, Skill skill) where T: Buff
    {
            T tempBuff = gameObject.AddComponent<T>();

            tempBuff.Configure(skill);

            return tempBuff;
        }

}}
