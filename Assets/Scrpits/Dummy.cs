using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : LivingEntity
{
    public bool basicCircularMovement = false;
    public float basicCircleMovementRadius = 10f;
    private Vector3 initialLocation;
    public void Test()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        initialLocation = transform.position;
    }

    // Update is called once per frame
    void Update() 
    {
        BasicCircularMovement();

        Debug.DrawRay(transform.position, transform.forward, Color.magenta, .5f);
    }

    void BasicCircularMovement()
    {
        if (basicCircularMovement)
        {
            Vector3 la = new Vector3(Mathf.Sin(Time.time), 0, Mathf.Cos(Time.time));
            //Debug.Log(la);

            transform.rotation = Quaternion.LookRotation(la.normalized,Vector3.up);
            transform.position = transform.position + transform.forward * 3 * Time.deltaTime;
        }
    }
}
