using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;

public class TargetInfoSetter : MonoBehaviour, IHaveTargetInfo
{
    public Player player;
    TargetInfo targetInfo = new TargetInfo(null, 0, Vector3.zero);
    public LayerMask targetInfoLayers;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            if (gameObject.TryGetComponent<Player>(out player))
            {

            } else
            {
                Destroy(this);
            }
        }

        Update();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = player.playerCamera.ScreenPointToRay(new Vector3(player.playerCamera.pixelWidth/2, player.playerCamera.pixelHeight/2, 0));
        Debug.DrawRay(ray.origin, ray.direction*90, Color.red);

        Vector3 targetPosition;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetInfoLayers))
        {
            targetPosition = hit.point;
            Ray ray2 = new Ray(player.skillManager.skillSpawnLocation.position, transform.forward);
                Debug.DrawRay(ray2.origin, ray2.direction * 40, Color.yellow);
                Debug.DrawLine(player.skillManager.skillSpawnLocation.position, targetPosition, Color.blue);
            targetInfo.position = hit.point;
            targetInfo.distanceToTarget = Vector3.Distance(hit.point, player.skillManager.skillSpawnLocation.position);
            targetInfo.target = hit.transform.gameObject;
            targetInfo.direction = transform.forward;
        } else {
            targetInfo.position = transform.position + transform.forward * 60;
            targetInfo.distanceToTarget = null;
            targetInfo.target = null;
            targetInfo.direction = transform.forward;
        }
    }

    public TargetInfo GetTargetInfo()
    {
        return targetInfo;
    }
}
