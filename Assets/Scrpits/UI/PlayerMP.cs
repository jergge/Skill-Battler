using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillSystem;

public class PlayerMP : MonoBehaviour
{
    public LivingEntity player;

    ManaStats mp => player.MP;

    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        //player.CanICast += EnoughToCast;
        //player.OnAfterCast += AfterCast;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.MP.currentPercent;
    }

    // void EnoughToCast(CastEventInfo info, CheckForAny checker)
    // {
    //     if ( info.skill.cost > mp.current )
    //     {
    //         checker.False();
    //     }
    // }

    // void AfterCast(CastEventInfo info)
    // {
    //     mp.current -= info.skill.cost;
    // }
}
