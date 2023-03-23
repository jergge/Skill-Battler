using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem {

public class Silenced : StatusCondition
{
    // Start is called before the first frame update
    void Start()
    {
        livingEntity.CanICast += NoCast;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NoCast(CastEventInfo info, CheckForAny check)
    {
        check.False();
    }

    void OnDestroy()
    {
        livingEntity.CanICast -= NoCast;
    }
}}
