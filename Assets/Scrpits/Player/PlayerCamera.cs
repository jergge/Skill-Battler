using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public List<CameraPreset> presets = new List<CameraPreset>();

    public Player player;

    public void SetLocation(CameraPreset preset)
    {
        transform.localPosition = preset.position;
        // Debug.Log("Camera at new position: " + targetLocation);

        transform.LookAt(player.transform.position + player.transform.forward * preset.lookTargetScale, Vector3.up);
    }
}

[System.Serializable]
public struct CameraPreset
{
    public Vector3 position;
    public float lookTargetScale;
}
