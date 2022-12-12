using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public abstract class BuffSkill<T> : Skill where T:MonoBehaviour
{
    T buff;
    // Start is called before the first frame update
    void OnEnable()
    {
        buff = source.AddComponent<T>();
    }

    
    void OnDisable()
    {
        Destroy(buff);
    }
}
