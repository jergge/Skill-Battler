using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class LivingEntityAnimator : MonoBehaviour
{
    public List<AnimationBinds> animationBinds = new List<AnimationBinds>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}


[System.Serializable]
public struct AnimationBinds
{
    public string variableName;
    public MonoBehaviour source;
}