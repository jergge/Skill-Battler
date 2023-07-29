using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [RequireComponent(typeof(SkillManager))]
    public class SkillTreeManager : MonoBehaviour
    {
        public string friendlyName = "Skill Tree";
        [SerializeField] public UnidirectionalGraph<Skill, LevelVertex<Skill>> graph;
        public Library skillLibrary;

        List<LevelVertex<Skill>> nodes = new List<LevelVertex<Skill>>();


        public bool LevelUp(Skill skill)
        {
            return false;
        }

        void Start()
        {

        }


    }
}