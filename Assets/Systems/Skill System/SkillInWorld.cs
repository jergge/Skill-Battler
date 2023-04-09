using System.Collections;
using System.Collections.Generic;
using Jergge.Extensions;
using SkillSystem;
using UnityEngine;

public class SkillInWorld : MonoBehaviour
{
    public Skill skill;
    List<SkillManager> alreadyTaught = new List<SkillManager>();

    float drop = .5f;
    float rise = .5f;
    float cycleTime = 1.5f;
    float innitialHeight;
    // Start is called before the first frame update
    void Start()
    {
        innitialHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Cos(Time.time);
        y = Remap(y, -1, 1, innitialHeight - drop, innitialHeight + rise);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        
        // Debug.Log(other.gameObject.name + " collided with SkillInWorld");

        SkillManager skillManager;
        if ( other.gameObject.TryGetComponentInParent<SkillManager>(out skillManager) && !alreadyTaught.Contains(skillManager) )
        {
            skillManager.Aquire(skill, true);
            alreadyTaught.Add(skillManager);
            // Debug.Log(other.transform.parent.gameObject.name + " has aquired a new skill: " + skill.name);
        }

        return;

    }

    public static float Remap(float input, float inputMin, float inputMax, float outputMin, float outputMax)
    {
            float noramlised = Mathf.InverseLerp(inputMin, inputMax, input);

            float output = Mathf.Lerp(outputMin, outputMax, noramlised);

            return output;
    }
}
