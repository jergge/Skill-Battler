using System;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//The script for player controlled characters, now using the new Unity input system
[RequireComponent(typeof(PlayerController), typeof(SkillManager), typeof(PlayerCamera))]
[RequireComponent(typeof(TargetInfoSetter), typeof(PlayerInput))]
public class Player : LivingEntity, ISendToastNotifications
{
    public Camera playerCamera;
    public PlayerCamera playerCameraController; 
    public PlayerController playerController;
    public TargetInfoSetter targetInfoSetter;

    public PlayerInput playerInput;

    public event Action<ToastNotificationInfo> PushToast;

    //TargetInfo targetInfo = new TargetInfo();
    //public LayerMask targetInfoLayers;

}
