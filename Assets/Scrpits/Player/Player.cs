using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using UnityEngine.UI;

//The script for player controlled characters, now using the new Unity input system
[RequireComponent(typeof(PlayerController), typeof(SkillManager), typeof(PlayerCamera))]
public class Player : LivingEntity, IHaveTargetInfo
{
    public Camera playerCamera;
    public PlayerCamera playerCameraController; 
    public PlayerController playerController;

    TargetInfo targetInfo = new TargetInfo();
    public LayerMask targetInfoLayers;
    public TargetInfo GetTargetInfo() => targetInfo;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

    }

    void Update()
    {
        UpdateTargetInfo();
    }

    void UpdateTargetInfo()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(playerCamera.pixelWidth/2, playerCamera.pixelHeight/2, 0));
        Debug.DrawRay(ray.origin, ray.direction*90, Color.red);

        Vector3 targetPosition;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetInfoLayers))
        {
            targetPosition = hit.point;
            Ray ray2 = new Ray(skillManager.skillSpawnLocation.position, transform.forward);
                Debug.DrawRay(ray2.origin, ray2.direction * 40, Color.yellow);
                Debug.DrawLine(skillManager.skillSpawnLocation.position, targetPosition, Color.blue);
            targetInfo.position = hit.point;
            targetInfo.distanceToTarget = Vector3.Distance(hit.point, skillManager.skillSpawnLocation.position);
            targetInfo.target = hit.transform.gameObject;
            targetInfo.direction = transform.forward;
        } else {
            targetInfo.position = transform.position + transform.forward * 60;
            targetInfo.distanceToTarget = null;
            targetInfo.target = null;
            targetInfo.direction = transform.forward;
        }

    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawSphere(targetInfo.position, .3f);
    }

}
