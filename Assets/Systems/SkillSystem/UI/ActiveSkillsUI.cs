using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SkillSystem {
public class ActiveSkillsUI : MonoBehaviour
{
    public SkillManager skillManager;
    public SkillUI SkillUIPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        foreach (Transform t in transform) 
        {
            Destroy(t.gameObject);
        }
        foreach (Skill skill in skillManager.GetActiveSkills()) {
            SkillUI tmp = GameObject.Instantiate(SkillUIPrefab);
            tmp.transform.SetParent(transform);
            tmp.skill = skill;
        }
    }
}}
