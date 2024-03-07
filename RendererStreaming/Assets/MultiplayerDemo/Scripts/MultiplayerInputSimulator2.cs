using System.Collections;
using System.Collections.Generic;
using Unity.RenderStreaming.Samples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class MultiplayerInputSimulator2 : MultiplayerInputSimulator
{

    public Canvas canvas;
    
    protected override void Start()
    {
        if (!Application.isPlaying)
            return;
        
        base.Start();
        if (canvas == null)
            canvas = this.GetComponent<Canvas>();

        var user = this.InputUser;
        foreach (var device in user.pairedDevices)
        {
            if (device is ICustomDevice customDevice)
            {
                customDevice.UserId = (int)user.id;
            }
        }

        var raycasters = canvas.GetComponentsInChildren<GraphicRaycaster>(true);
        foreach (var raycaster in raycasters)
        {
            UGCMultiplayerEventSystem.Instance.Register(raycaster, (int)user.id);
        }
    }
}
