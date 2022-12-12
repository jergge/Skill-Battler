using System;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;
using UnityEngine.UI;

//The script for player controlled characters, now using the new Unity input system
[RequireComponent(typeof(PlayerController), typeof(SkillManager), typeof(PlayerCamera))]
[RequireComponent(typeof(TargetInfoSetter))]
public class Player : LivingEntity
{
    public Camera playerCamera;
    public PlayerCamera playerCameraController; 
    public PlayerController playerController;
    public TargetInfoSetter targetInfoSetter;

    //TargetInfo targetInfo = new TargetInfo();
    //public LayerMask targetInfoLayers;
}
