using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillSystem;

public class PlayerMP : MonoBehaviour
{
    public StatsTracker stats;

    public Slider slider;


    void Start()
    {
        //slider = GetComponent<Slider>();
        slider.value = stats.currentPercent;
    }


    void Update()
    {
        slider.value = stats.currentPercent;
    }


}
