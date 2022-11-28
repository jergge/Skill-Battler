using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testray : MonoBehaviour
{
    Vector3 rayDir = Vector3.down;
    public float rayLength = 50f;
    public LayerMask layers;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        RaycastHit hitInfo;

        if ( Physics.Raycast(transform.position, Vector3.down, out hitInfo, rayLength, layers, QueryTriggerInteraction.UseGlobal))
        {
            Gizmos.DrawRay(transform.position, Vector3.down * 200);
            Gizmos.DrawLine(transform.position, hitInfo.point);
            Gizmos.DrawSphere(hitInfo.point, 5f);
        } else {
        Gizmos.color = Color.red;
            //Debug.Log("no layer interaction found");
            Gizmos.DrawLine(transform.position, transform.position + (transform.up * -rayLength));
        }
    }
}
