using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompasUI : MonoBehaviour
{
    public float numberOfPixelsNorthToNorth;
    public GameObject target;
    Vector3 startPosition;
    float rationAngleToPixel;
    public bool reversePan;

    void Start()
    {
        startPosition = transform.position;
        rationAngleToPixel = numberOfPixelsNorthToNorth / 360f;
    }
 
    void Update ()
    {
        Vector3 perp = Vector3.Cross(Vector3.forward, target.transform.forward);
        float dir = Vector3.Dot(perp, Vector3.up);
        float pan = Vector3.Angle(target.transform.forward, Vector3.forward) * Mathf.Sign(dir) * rationAngleToPixel;
        if (reversePan)
        {
            pan *= -1;
        }
        transform.position = startPosition + (new Vector3(pan, 0, 0));
    }
}
