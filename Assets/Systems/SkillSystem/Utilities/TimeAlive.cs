using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAlive : MonoBehaviour
{
    float remaingTime;
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        remaingTime = duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (remaingTime <= 0)
        {
            Destroy(gameObject);
        }

        remaingTime -= Time.deltaTime;
    }
}
